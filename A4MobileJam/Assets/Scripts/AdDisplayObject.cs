using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdDisplayObject : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string myGameIdAndroid = "5225759";
    [SerializeField] private string myGameIdIOS = "5225758";
    [SerializeField] private string adUnitIdAndroid = "Interstitial_Android";
    [SerializeField] private string adUnitIdIOS = "Interstitial_iOS";
    [SerializeField] private string myAdUnitId;
    [SerializeField] private bool adStarted;
    private bool testMode = true;
    void Start()
    {
#if UNITY_IOS
	Advertisement.Initialize(myGameIdIOS, testMode, this);
	myAdUnitId = adUnitIdIOS;
#else //if UNITY_ANDROID
        Advertisement.Initialize(myGameIdAndroid, testMode, this);
        myAdUnitId = adUnitIdAndroid;
#endif
    }

    void Update()
    {
        
    }
    public void ShowAd()
    {
        //if (Advertisement.isInitialized && !adStarted)
        //{
        //    Advertisement.Load(myAdUnitId, this);
        //    Advertisement.Show(myAdUnitId, this);
        //    adStarted = true;
        //}

        Advertisement.Show(myAdUnitId, this);
    }

    public void OnInitializationComplete()
    {
        Advertisement.Load(myAdUnitId, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {

    }

    public void OnUnityAdsAdLoaded(string placementId)
    {

    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {

    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {

    }

    public void OnUnityAdsShowStart(string placementId)
    {

    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {

    }
}
