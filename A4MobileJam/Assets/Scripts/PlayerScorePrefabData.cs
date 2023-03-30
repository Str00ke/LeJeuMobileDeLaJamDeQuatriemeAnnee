using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct PSPData
{
    public Image pImage;
    public string pName;
    public int pScore;
    public int pPlace;

    
    public PSPData(Image image, string name, int score, int place)
    {
        pImage = image;
        pName = name;   
        pScore = score;
        pPlace = place;
    }
}

public class PlayerScorePrefabData : MonoBehaviour
{
    [SerializeField] Image _pImage;
    [SerializeField] Text _pScore;
    [SerializeField] Text _pName;
    [SerializeField] Text _pPlace;

    public void SetData(PSPData data)
    {
        _pImage = data.pImage;
        _pScore.text = data.pScore.ToString();
        _pName.text = data.pName;
        _pPlace.text = data.pPlace.ToString();
    }
}
