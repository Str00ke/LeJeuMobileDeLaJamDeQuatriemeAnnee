using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using GameAnalyticsSDK;
using UnityEngine.UI;

public class GlobalManager : MonoBehaviour
{
    int pNbr = 1;

    [SerializeField] Text pNbrTxt;

    [SerializeField] GameObject _playerData;
    [SerializeField] GameObject _setPlayersPanel;
    [SerializeField] VerticalLayoutGroup _layoutGroup;

    [SerializeField] List<Material> _balls;
    [SerializeField] List<Sprite> _ballsSpr;
    [SerializeField] List<int> _ballsIndexPossessed = new();


    [SerializeField] GameObject _ballSelect;
    [SerializeField] Text _diamondsTxt;
    int _currBallSelectIndex = 0;

    static int _diamonds = 0;
    static string _ballsPossStr = "";
    int prevDiamond = 0;

    public static int GetDiamonds => _diamonds;
    public static void AddDiamonds(int value) => _diamonds += value;
    public static bool TryRemoveDiamonds(int value)
    {
        if(value > _diamonds) return false;
        else _diamonds -= value;
        return true;
    }

    public struct SetP
    {
        public Image _img;
        public InputField _name;
        public Material _mat;
        public Sprite _spr;
        public int _index;
        public string _strName
        {
            get => _name.text;
            set => _name.text = value;
        }

        public SetP(Image image, InputField name, Material mat, Sprite spr, int index)
        {
            _img = image;
            _name = name;
            _mat = mat;
            _spr = spr;
            _index = index;
            _strName = _name.text;
            _name.contentType = InputField.ContentType.Name;
            _name.inputType = InputField.InputType.Standard;
            _name.lineType = InputField.LineType.SingleLine;
            _name.characterValidation = InputField.CharacterValidation.None;
            _name.characterLimit = 12;
        }
    }

    static private List<SetP> _sets = new List<SetP>();
    public List<SetP> Sets => _sets;
    List<GameObject> arrayPSet = new List<GameObject>();
    public void EraseList()
    {
        _sets.Clear();
        foreach (var item in arrayPSet)
        {
            Destroy(item);
        }
        arrayPSet.Clear();
    }

    static public List<SetP> GetList()
    {
        return _sets;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (!PlayerPrefs.HasKey("Diamonds"))
            PlayerPrefs.SetInt("Diamonds", 0);
        else
            _diamonds = PlayerPrefs.GetInt("Diamonds");

        if (!PlayerPrefs.HasKey("Possessed"))
            PlayerPrefs.SetString("Possessed", "0");
        else
            _ballsPossStr = PlayerPrefs.GetString("Possessed");

        _ballsIndexPossessed.Clear();

        for (int i = 0; i < _ballsPossStr.Length; i++)
            _ballsIndexPossessed.Add(_ballsPossStr[i] - '0');

        prevDiamond = _diamonds;
        if (_diamondsTxt != null) _diamondsTxt.text = _diamonds.ToString();

    }

    private void Start()
    {
        GameAnalytics.Initialize();
        GameAnalytics.StartSession();
    }

    private void Update()
    {
        if(_diamonds != prevDiamond) //Shit code yeah I know
        {
            prevDiamond = _diamonds;
            if (_diamondsTxt != null) _diamondsTxt.text = _diamonds.ToString();
        }
    }


    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("Diamonds", _diamonds);
        string str = string.Empty;
        for (int i = 0; i < _ballsIndexPossessed.Count; i++)
            str += _ballsIndexPossessed[i].ToString();


        PlayerPrefs.SetString("Possessed", str);

    }

    public void LoadScene(string sceneName) => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);


    public void SetPlayerNbrValue(Slider s)
    {
        pNbrTxt.GetComponent<Text>().text = s.value.ToString();
        pNbr = (int)s.value;
    }

    public void GenerateSetPlayers()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        List<int> nbrs = new List<int>();
        for (int i = 0; i < _balls.Count; i++)
        {
            nbrs.Add(i);
        }
        _setPlayersPanel.SetActive(false);
        _layoutGroup.transform.parent.gameObject.SetActive(true);
        for (int i = 0; i < pNbr; i++)
        {
            int rndBall = Random.Range(0, nbrs.Count);
            nbrs.RemoveAt(rndBall);
            GameObject p = Instantiate(_playerData, _layoutGroup.transform);
            SetP set = new SetP(p.transform.GetChild(0).GetComponent<Image>(), p.transform.GetChild(1).GetComponent<InputField>(), _balls[rndBall], _ballsSpr[rndBall], i);
            set._strName = "Player_" + (i+1).ToString();
            _sets.Add(set);
            set._img.sprite = _ballsSpr[0];//_ballsSpr[rndBall];
            arrayPSet.Add(p);
            p.GetComponent<PlayerSetIndex>().SetIndex(i);
        }
    }
    
    public void TryBuyBall(int selected)
    {
        if (TryRemoveDiamonds(5))
        {
            _ballsIndexPossessed.Add(selected);
            HideBallSelect(selected);
            Debug.Log(_ballsPossStr);

            //for (int i = 0; i < _ballsIndexPossessed.Count; i++)
            //{
            //    if (_ballsIndexPossessed[i] == selected)
            //        
            //}
        }
    }

    public void ShowBallSelect(int playerIndex)
    {
        _ballSelect.SetActive(true);
        _currBallSelectIndex = playerIndex;
        _ballSelect.GetComponent<SelectBall>().Populate(_ballsSpr, _ballsIndexPossessed);
    }

    public void HideBallSelect(int selected)
    {
        _ballSelect.SetActive(false);
        for (int i = _ballSelect.transform.GetChild(0).childCount - 1; i > 0; i--)
        {
            Destroy(_ballSelect.transform.GetChild(0).GetChild(i).gameObject);
        }
        Destroy(_ballSelect.transform.GetChild(0).GetChild(0).gameObject);

        SetP p = _sets[_currBallSelectIndex];
        p._mat = _balls[selected];
        p._spr = _ballsSpr[selected];
        _sets[_currBallSelectIndex] = p;
        _layoutGroup.transform.GetChild(_currBallSelectIndex).GetChild(0).GetComponent<Image>().sprite = p._spr;
    }
}
