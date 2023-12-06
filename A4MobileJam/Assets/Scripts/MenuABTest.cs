using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuABTest : MonoBehaviour
{
    static MenuABTest instance;


    [SerializeField] List<Image> btnImgs;

    [SerializeField] float btnsColorsR = 1.0f;
    [SerializeField] float btnsColorsG = 1.0f;
    [SerializeField] float btnsColorsB = 1.0f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
        GameAnalytics.GetRemoteConfigsValueAsString("btnsColorsR");
        if (GameAnalytics.IsRemoteConfigsReady())
        {
            Debug.Log("Game Analytics Remote config ready");


            string r = GameAnalytics.GetRemoteConfigsValueAsString("btnsColorsR", "1f");
            string g = GameAnalytics.GetRemoteConfigsValueAsString("btnsColorsG", "1f");
            string b = GameAnalytics.GetRemoteConfigsValueAsString("btnsColorsB", "1f");
            btnsColorsR = float.Parse(r);
            btnsColorsG = float.Parse(g);
            btnsColorsB = float.Parse(b);

            GameAnalytics.OnRemoteConfigsUpdatedEvent += OnRemoteConfigsUpdateFunction;
        }
        else Debug.LogWarning("Game Analytics Remote config NOT ready");

        Color col = new Color(btnsColorsR, btnsColorsG, btnsColorsB, 1f);
        foreach (Image img in btnImgs) img.color = col;
    }

    void Update()
    {
    }

    private static void OnRemoteConfigsUpdateFunction()
    {
        Debug.Log("<color=red>===============UPDATE===============</color>");
        Color col = new Color(instance.btnsColorsR, instance.btnsColorsG, instance.btnsColorsB, 1f);
        foreach (Image img in instance.btnImgs) img.color = col;
    }
}
