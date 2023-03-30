using System.Collections;
using System.Collections.Generic;
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

    public struct SetP
    {
        public Image _img;
        public InputField _name;
        public Material _mat;
        public string _strName
        {
            get => _name.text;
            set => _name.text = value;
        }

        public SetP(Image image, InputField name, Material mat)
        {
            _img = image;
            _name = name;
            _mat = mat;
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
            SetP set = new SetP(p.transform.GetChild(0).GetComponent<Image>(), p.transform.GetChild(1).GetComponent<InputField>(), _balls[rndBall]);
            set._strName = "Player_" + (i+1).ToString();
            _sets.Add(set);
            set._img.sprite = _ballsSpr[rndBall];
            arrayPSet.Add(p);
        }
    }
        
}
