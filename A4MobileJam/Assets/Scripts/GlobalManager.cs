using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

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


    [SerializeField] GameObject _ballSelect;
    int _currBallSelectIndex = 0;

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
    }


    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

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
            set._img.sprite = _ballsSpr[rndBall];
            arrayPSet.Add(p);
            p.GetComponent<PlayerSetIndex>().SetIndex(i);
        }
    }
       
    public void ShowBallSelect(int playerIndex)
    {
        _ballSelect.SetActive(true);
        _currBallSelectIndex = playerIndex;
        _ballSelect.GetComponent<SelectBall>().Populate(_ballsSpr);
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
