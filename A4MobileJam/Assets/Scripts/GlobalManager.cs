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

    public struct SetP
    {
        public Image _img;
        public InputField _name;
        public string _strName
        {
            get => _name.text;
            set => _name.text = value;
        }

        public SetP(Image image, InputField name)
        {
            _img = image;
            _name = name;
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
        _setPlayersPanel.SetActive(false);
        _layoutGroup.transform.parent.gameObject.SetActive(true);
        for (int i = 0; i < pNbr; i++)
        {
            GameObject p = Instantiate(_playerData, _layoutGroup.transform);
            SetP set = new SetP(p.transform.GetChild(0).GetComponent<Image>(), p.transform.GetChild(1).GetComponent<InputField>());
            set._strName = "Player_" + (i+1).ToString();
            _sets.Add(set);
        }
    }
        
}
