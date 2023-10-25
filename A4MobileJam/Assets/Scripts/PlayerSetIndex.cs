using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerSetIndex : MonoBehaviour
{
    private int _index;

    public void SetIndex(int index) => _index = index;

    public void OpenSelectBall() => FindObjectOfType<GlobalManager>().ShowBallSelect(_index);
    public void CloseSelectBall() => FindObjectOfType<GlobalManager>().HideBallSelect(_index);
    public void TryBuyBall() => FindObjectOfType<GlobalManager>().TryBuyBall(_index);

}
