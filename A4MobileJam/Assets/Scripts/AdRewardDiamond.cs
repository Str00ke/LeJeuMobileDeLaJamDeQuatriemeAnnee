using GameAnalyticsSDK;


[System.Serializable]
public class AdRewardDiamond : AdEndCallback
{
    public override void OnAdEnding()
    {
        base.OnAdEnding();
        GlobalManager.AddDiamonds(5);
        GameAnalytics.NewResourceEvent(GAResourceFlowType.Source, "Diamonds", 5, "Diamonds", "Diamonds");
    }
}