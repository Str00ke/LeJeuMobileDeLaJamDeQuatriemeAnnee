using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

public class RemoteConfig : MonoBehaviour {

    public struct userAttributes {
      // Optionally declare variables for any custom user attributes:
        public bool expansionFlag;
    }

    public struct appAttributes {
      // Optionally declare variables for any custom app attributes:
        public int level;
        public int score;
        public string appVersion;
    }

    public struct filterAttributes {
      // Optionally declare variables for attributes to filter on any of following parameters:
        public string[] key;
        public string[] type;
        public string[] schemaId;
    }

    // Optionally declare a unique assignmentId if you need it for tracking:
    public string assignmentId;

    // Declare any Settings variables you’ll want to configure remotely:
    public  List<Image> btnImgs;

    public float btnsColorsR = 1.0f;
    public float btnsColorsG = 1.0f;
    public float btnsColorsB = 1.0f;


    // The Remote Config package depends on Unity's authentication and core services.
    // These dependencies require a small amount of user code for proper configuration.
    async Task InitializeRemoteConfigAsync()
    {
            // initialize handlers for unity game services
            await UnityServices.InitializeAsync();

            // options can be passed in the initializer, e.g if you want to set AnalyticsUserId or an EnvironmentName use the lines from below:
            // var options = new InitializationOptions()
            // .SetEnvironmentName("testing")
            // .SetAnalyticsUserId("test-user-id-12345");
            // await UnityServices.InitializeAsync(options);

            // remote config requires authentication for managing environment information
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
    }

    async Task Awake () {
        // initialize Unity's authentication and core services
        await InitializeRemoteConfigAsync();

        // Add a listener to apply settings when successfully retrieved:
        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteConfig;

        // you can set the user’s unique ID:
        // RemoteConfigService.Instance.SetCustomUserID("some-user-id");

        // you can set the environment ID:
        // RemoteConfigService.Instance.SetEnvironmentID("an-env-id");

        // Fetch configuration settings from the remote service, they must be called with the attributes structs (empty or with custom attributes) to initiate the WebRequest.
        await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());

        // Example on how to fetch configuration settings using filter attributes:
        // var fAttributes = new filterAttributes();
        // fAttributes.key = new string[] { "sword","cannon" };
        // RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes(), fAttributes);

        // Example on how to fetch configuration settings if you have dedicated configType:
        // var configType = "specialConfigType";
        // Fetch configs of that configType
        // RemoteConfigService.Instance.FetchConfigs(configType, new userAttributes(), new appAttributes());
        // Configuration can be fetched with both configType and fAttributes passed
        // RemoteConfigService.Instance.FetchConfigs(configType, new userAttributes(), new appAttributes(), fAttributes);

        // All examples from above will also work asynchronously, returning Task<RuntimeConfig>
        // await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes());
        // await RemoteConfigService.Instance.FetchConfigsAsync(new userAttributes(), new appAttributes(), fAttributes);
        // await RemoteConfigService.Instance.FetchConfigsAsync(configType, new userAttributes(), new appAttributes());
        // await RemoteConfigService.Instance.FetchConfigsAsync(configType, new userAttributes(), new appAttributes(), fAttributes);

    }

    // Create a function to set your variables to their keyed values:
    void ApplyRemoteConfig (ConfigResponse configResponse) {
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin) {
            case ConfigOrigin.Default:
                Debug.Log ("No settings loaded this session and no local cache file exists; using default values.");
                break;
            case ConfigOrigin.Cached:
                Debug.Log ("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                Debug.Log ("New settings loaded this session; update values accordingly.");
                break;
        }

        btnsColorsR = RemoteConfigService.Instance.appConfig.GetFloat("btnsColorsR");
        btnsColorsG = RemoteConfigService.Instance.appConfig.GetFloat("btnsColorsG");
        btnsColorsB = RemoteConfigService.Instance.appConfig.GetFloat("btnsColorsB");

        assignmentId = RemoteConfigService.Instance.appConfig.assignmentId;
	
	    Color col = new Color(btnsColorsR, btnsColorsG, btnsColorsB, 1f);
        foreach (Image img in btnImgs) img.color = col;
        // These calls could also be used with the 2nd optional arg to provide a default value, e.g:
        // enemyVolume = RemoteConfigService.Instance.appConfig.GetInt("enemyVolume", 100);
    }
}