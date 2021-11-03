using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase.RemoteConfig;

public class GOAD : MonoBehaviour
{
    public enum BANNER_TYPE
    {
        NODE,
        REMOVEADS,
        ERROR,
        WAITING,
        SHOW
    }

    [Header(" KEY MAX APPLOVIN ")] public string MaxSdkKey = "";
    public string Max_banner = "";
    public string Max_Instertitial = "";
    public string Max_VideoReward = "";
    [SerializeField] GameObject _loadingAds;
    //[SerializeField] vMessengeManager vMessengeManager;
    int countLoadIntertitical = 0;
    bool watchVideoSucc = false;
    Coroutine _timerOutVideoReward = null;
    System.Action<bool> onComplete;
    [SerializeField] Button btn_X_ads;

    [SerializeField] bool isTimerAD = false;
    int replayCountIntertitical = 0;
    public GameObject loadAd_ResumeApp;

    public int AdSetting_Resume = 0;
    public static GOAD Instance = null;
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //initRemoteConfig();
    }

    void Start()
    {
        countLoadIntertitical = 0;
        MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
        {
            // AppLovin SDK is initialized, configure and start loading ads.
            Debug.Log("MAX SDK Initialized");
            InitializeBannerAds();
            InitializeInterstitialAds();
            InitializeRewardedAds();
            
            //MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            //{
            //    // Show Mediation Debugger
            //MaxSdk.ShowMediationDebugger();
            //};
        };

        
        MaxSdk.SetSdkKey(MaxSdkKey);
        MaxSdk.InitializeSdk();
        if (btn_X_ads == null) return;
        btn_X_ads.onClick.AddListener(() =>
        {
            if (_loadingAds.activeInHierarchy)
            {
                StopLoadingAds();
                _loadingAds.SetActive(false);
            }
        });
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.LogError("FOCUS");
                // RequestBanner();
                showBanner();
            }
        }
    }

    #region Banner

     private void InitializeBannerAds()
    {
        // Attach Callbacks
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
        MaxSdk.CreateBanner(Max_banner, MaxSdkBase.BannerPosition.BottomCenter);
        // Set background or background color for banners to be fully functional.
        MaxSdk.SetBannerBackgroundColor(Max_banner, Color.black);
    }




     bool isLoadingBanner = false;


    /// <summary>
    /// checked xem banner dax show chua
    /// </summary>
    /// <returns> true la da show / false la chua show </returns>

    public void showBanner()
    {
        if (!RootManager.Instance.isRemoveAds)
        {
            MaxSdk.ShowBanner(Max_banner);
            if (!UtilGame.IsConnectionNetwork()){
         
                MaxSdk.HideBanner(Max_banner);
            }

        }
        else
        {
            
        }
    }

    #endregion

    // ReSharper disable Unity.PerformanceAnalysis
    public void showIntertital(bool isInGame, bool isReplay, string namemethod = "BACK")
    {
        if (UtilGame.IsConnectionNetwork())
        {
            if (!RootManager.Instance.isRemoveAds)
            {
                if (MaxSdk.IsInterstitialReady(Max_Instertitial))
                {
                    //bool isRep = CheckedShowAD(isInGame, isReplay);
                    //if (isRep)
                    //{
                        MaxSdk.ShowInterstitial(Max_Instertitial);
                    //}
                }
                else
                {
                    LoadInterstitial();
                }
            }
        }
    }
    #region dung cho resume Application

    public void showIntertitialFocus()
    {
        loadAd_ResumeApp.SetActive(false);
        if (!RootManager.Instance.isTestAD)
        {
            if (UtilGame.IsConnectionNetwork())
            {
                Debug.Log("LOADED ADS : " + UtilGame.GetBool(UtilGame.KEY_REMOVEADS.ToString(), false));
                //if (!UtilGame.GetBool(UtilGame.KEY_REMOVEADS.ToString(), false))
                if(!RootManager.Instance.isRemoveAds)
                {
                   // Debug.Log("LOADED ADS : " + interstitial.IsLoaded());
                    if (MaxSdk.IsInterstitialReady(Max_Instertitial))
                    {
                        if (loadingResumeApp != null)
                        {
                            StopCoroutine(loadingResumeApp);
                        }
                        loadAd_ResumeApp.SetActive(true);
                        loadingResumeApp = StartCoroutine(WaitLoadingAdResumeApp(() =>
                        {
                            loadAd_ResumeApp.SetActive(false);
                        }));
                    }
                    else
                    {
                        Debug.Log("LOADED ADS : NEW");
                        LoadInterstitial();
                    }
                }
            }
        }
    }

    Coroutine loadingResumeApp;
    private Action _completeResumeApp;

    IEnumerator WaitLoadingAdResumeApp(Action com)
    {
        yield return new WaitForSeconds(0.5f);
        if ( MaxSdk.IsInterstitialReady(Max_Instertitial))
        {
            MaxSdk.ShowInterstitial(Max_Instertitial);
            this._completeResumeApp = com;
        }
        else
        {
            loadAd_ResumeApp.SetActive(false);
        }
    }

    #endregion
    


    public void ShowVideoReward(System.Action<bool> action)
    {
        Debug.Log("XEMVIDEO");
        this.onComplete = action;
        if (RootManager.Instance.isTestAD)
        {
            this.onComplete(true);
            this.onComplete = null;
        }
        else
        {
            if (UtilGame.IsConnectionNetwork())
            {
                if (isAvableVideo())
                {
                    if (_loadingAds != null) _loadingAds.SetActive(false);
                    MaxSdk.ShowRewardedAd(Max_VideoReward);
                    Debug.Log("SHOW VIDEO: 01 " + watchVideoSucc);
                }
                else
                {
                    //rewardedAd.LoadAd(this.CreateAdRequest());
                    if (_loadingAds != null) _loadingAds.gameObject.SetActive(true);
                    if (_timerOutVideoReward != null)
                    {
                        StopCoroutine(_timerOutVideoReward);
                        _timerOutVideoReward = null;
                    }

                    _timerOutVideoReward = StartCoroutine(WaitLoadingAds());
                    Debug.Log("SHOW VIDEO: 02 " + watchVideoSucc);
                }
            }
            else
            {

                this.onComplete = null;
            }
        }
    }


    public bool isAvableVideo()
    {
        return MaxSdk.IsRewardedAdReady(Max_VideoReward);
    }

   


 

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator WaitLoadingAds()
    {
        LoadRewardedAd();
        watchVideoSucc = false;
        float time = 5;
        while (time > 0)
        {
            Debug.Log("TIMER_RUN : " + time);
            time -= Time.fixedDeltaTime;
            yield return null;
            if (MaxSdk.IsRewardedAdReady(Max_VideoReward))
            {
                watchVideoSucc = true;
               MaxSdk.ShowRewardedAd((Max_VideoReward));
               if (_loadingAds != null) _loadingAds.SetActive(false);
                Debug.Log("STATE: 01 ");
                // time = -1;
                break;
            }
        }
        if (time <= 0)
        {
            if (MaxSdk.IsRewardedAdReady(Max_VideoReward) && !watchVideoSucc)
            {
                MaxSdk.ShowRewardedAd(Max_VideoReward);
                if (_loadingAds != null) _loadingAds.SetActive(false);
                Debug.Log("STATE: 02 ");
        
            }
            else
            {
                //Không thể load được ad
                Debug.Log("STATE: 03 ");
                if (_loadingAds != null) _loadingAds.SetActive(false);
                watchVideoSucc = false;
                if (onComplete != null)
                {
                    onComplete(false);
                }
                // if (vMessengeManager != null)
                // {
                //     vMessengeManager.show(false);
                // }
            }
        }
        yield return null;
    }


    void StopLoadingAds()
    {
        watchVideoSucc = false;
        if (_timerOutVideoReward != null)
        {
            StopCoroutine(_timerOutVideoReward);
            _timerOutVideoReward = null;
        }

        Debug.Log("STATE: 05 " + watchVideoSucc);
    }


    #region Firebase Remote Config

     const string KEY_AdSetting_time_normal = "AdSetting_time_normal";
    const string KEY_AdSetting_play = "AdSetting_play";
    const string KEY_AdSetting_level = "AdSetting_level";
    private const string KEY_AdSetting_ResumeGame = "AdSetting_ResumeGame";


    string AdSetting_time_normal = "45,45,50,55,55";
    string AdSetting_play = "3,1,1,1,1";
    string AdSetting_level = "2,15,35,60,100";
    List<int> SettingTimeNormal = new List<int>();
    List<int> SettingPlay = new List<int>();
    List<int> SettingLevel = new List<int>();

    public void initRemoteConfig()
    {
        SettingTimeNormal = ConvertStringToInt(AdSetting_time_normal);
        SettingPlay = ConvertStringToInt(AdSetting_play);
        SettingLevel = ConvertStringToInt(AdSetting_level);
        replayCountIntertitical = 0;
        isTimerAD = false;
        if (!UtilGame.GetBool(UtilGame.KEY_REMOVEADS.ToString(), false))
        {
            if (coroutineADTime != null)
            {
                StopCoroutine(coroutineADTime);
                coroutineADTime = null;
            }

            //int ADlevel = GameManager.Instance.CurLevel;
            //TESST
            int ADlevel = GameData.HighestLevel;
            int ADtimeNormal = GetValueData(ADlevel, SettingTimeNormal);
            coroutineADTime = StartCoroutine(TimerAD(ADtimeNormal));
            Debug.Log("CHAY TIME ");
        }

        Dictionary<string, object> defaults = new Dictionary<string, object>();
        defaults.Add(KEY_AdSetting_time_normal, AdSetting_time_normal);
        defaults.Add(KEY_AdSetting_play, AdSetting_play);
        defaults.Add(KEY_AdSetting_level, AdSetting_level);
        defaults.Add(KEY_AdSetting_ResumeGame, 0);
        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

        FetchDataAsync();
    }

    // Start a fetch request.
    Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        System.Threading.Tasks.Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    void FetchComplete(Task fetchTask)
    {
        var info = FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus)
        {
            case LastFetchStatus.Success:
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                //Lay thong tin 
                AdSetting_level = FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_AdSetting_level).StringValue;
                AdSetting_play = FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_AdSetting_play).StringValue;
                AdSetting_time_normal = FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_AdSetting_time_normal)
                    .StringValue;
                AdSetting_Resume = int.Parse(FirebaseRemoteConfig.DefaultInstance.GetValue(KEY_AdSetting_ResumeGame)
                    .StringValue);


                SettingTimeNormal = ConvertStringToInt(AdSetting_time_normal);
                SettingPlay = ConvertStringToInt(AdSetting_play);
                SettingLevel = ConvertStringToInt(AdSetting_level);
                replayCountIntertitical = 0;
                isTimerAD = false;
                if (!UtilGame.GetBool(UtilGame.KEY_REMOVEADS.ToString(), false))
                {
                    if (coroutineADTime != null)
                    {
                        StopCoroutine(coroutineADTime);
                        coroutineADTime = null;
                    }
                    int ADlevel = GameData.HighestLevel;
                    int ADtimeNormal = GetValueData(ADlevel, SettingTimeNormal);
                    coroutineADTime = StartCoroutine(TimerAD(ADtimeNormal));
                    Debug.Log("CHAY TIME _0101");
                }

                break;
        }
    }

    List<int> ConvertStringToInt(string value)
    {
        List<int> vs = new List<int>();
        string[] bs = value.Split(',');
        for (int i = 0; i < bs.Length; i++)
        {
            vs.Add(int.Parse(bs[i].Trim()));
        }

        return vs;
    }
    #endregion




    Coroutine coroutineADTime;

    bool CheckedShowAD(bool isInGame = false, bool isReplay = false)
    {
        int ADlevel = GameData.HighestLevel;
        int ADtimeNormal = GetValueData(ADlevel, SettingTimeNormal);
        int ADReplay = GetValueData(ADlevel, SettingPlay);

        Debug.Log("AD DATA: " + ADlevel + "--" + ADtimeNormal + "--" + ADReplay);
        Debug.Log("isInGame : " + isInGame + "    isReplay: " + isReplay +"     LEVEL: "+ ADlevel);
        
        if (isInGame)
        {
            if (ADlevel > SettingLevel[0])
            {
                // choi trong man game
                if (isReplay)
                {
                    replayCountIntertitical++;
                }

                Debug.Log("01_replayCountIntertitical : " + replayCountIntertitical + "   isTimerAD: " + isTimerAD);

                if (replayCountIntertitical >= ADReplay || isTimerAD)
                {
                    replayCountIntertitical = 0;
                    isTimerAD = false;
                    if (coroutineADTime != null)
                    {
                        StopCoroutine(coroutineADTime);
                        coroutineADTime = null;
                    }

                    coroutineADTime = StartCoroutine(TimerAD(ADtimeNormal));
                    Debug.Log("SHOW ADS INAGME");
                    // show dc ad
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("02_replayCountIntertitical : " + replayCountIntertitical + "   isTimerAD: " + isTimerAD);
            // cac back cac man khac ve home
            if (replayCountIntertitical >= ADReplay || isTimerAD)
            {
                replayCountIntertitical = 0;
                isTimerAD = false;
                if (coroutineADTime != null)
                {
                    StopCoroutine(coroutineADTime);
                    coroutineADTime = null;
                }

                coroutineADTime = StartCoroutine(TimerAD(ADtimeNormal));
                Debug.Log("SHOW ADS BACK");
                // show dc ad
                return true;
            }
        }


        return false;
    }


    int GetValueData(int level, List<int> data)
    {
        int index = GetIndexLevel(level, data);
        //   Debug.LogError("INDEX_ADS: " + index + "  COUNT: " + data.Count);
        if (index >= data.Count)
        {
            return data[0];
        }
        else
        {
            return data[index];
        }
    }

    int GetIndexLevel(int level, List<int> data)
    {
        for (int i = 0; i < SettingLevel.Count - 1; i++)
        {
            if (level >= data[i] && level < data[i + 1])
            {
                return i;
            }
        }

        return 0;
    }
    IEnumerator HandleClose()
    {
        yield return null;
        LoadRewardedAd();

        if (onComplete != null && !watchVideoSucc)
        {
            onComplete = null;
        }
        watchVideoSucc = false;
        if (_timerOutVideoReward != null)
        {
            StopCoroutine(_timerOutVideoReward);
            _timerOutVideoReward = null;
        }
    }


    IEnumerator TimerAD(float vaTimer)
    {
        isTimerAD = false;
        float timeWaiting = vaTimer;
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (timeWaiting > 0)
        {
            timeWaiting--;
            if (timeWaiting <= 0)
            {
                isTimerAD = true;
            }

            // Debug.Log("ADTIME: " + timeWaiting);
            yield return waitForSeconds;
        }

        yield return null;
    }

    #region MAX APPLOVIN APP SETTING

    // ReSharper disable Unity.PerformanceAnalysis
    void InitializeInterstitialAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;

        // Load the first interstitial
        // LoadInterstitial();
        MaxSdk.LoadInterstitial(Max_Instertitial);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(Max_Instertitial);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        countLoadIntertitical = 0;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        countLoadIntertitical++;
        if (countLoadIntertitical < 2)
        {
            LoadInterstitial();
        }
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
        MaxSdkBase.AdInfo adInfo)
    {
    }

    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
    }

    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        HandleFireBase.Instance.LogEventWithString(HandleFireBase.INTERSTITIAL_ADS);
    }

    private void InitializeRewardedAds()
    {
        // Attach callbacks
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

        // Load the first RewardedAd
        LoadRewardedAd();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(Max_VideoReward);
    }


    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'

        Debug.Log("Rewarded ad loaded"); // Reset retry attempt
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load. We recommend retrying with exponentially higher delays up to a maximum delay (in this case 64 seconds).
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
        MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
        LoadRewardedAd();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad displayed");
        if (_loadingAds != null) _loadingAds.SetActive(false);
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad clicked");
    }

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is hidden. Pre-load the next ad
        Debug.Log("Rewarded ad dismissed");
        if (_loadingAds != null) _loadingAds.SetActive(false);
        StartCoroutine(HandleClose());
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad was displayed and user should receive the reward
        Debug.Log("Rewarded ad received reward");
        if (onComplete != null)
        {
            onComplete(true);
            onComplete = null;
        }
        watchVideoSucc = true;
        if (_loadingAds != null) _loadingAds.SetActive(false);
        
        
        if (_timerOutVideoReward != null)
        {
            StopCoroutine(_timerOutVideoReward);
            _timerOutVideoReward = null;
        }
        LoadRewardedAd();
        //HandleFireBase.Instance.LogEventWithString("REWARD_VIDEO_SUCC");
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad revenue paid. Use this callback to track user revenue.
        Debug.Log("Rewarded ad revenue paid");
    }
    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad is ready to be shown.
        // If you have already called MaxSdk.ShowBanner(BannerAdUnitId) it will automatically be shown on the next ad refresh.
        Debug.Log("Banner ad loaded");
    }

    private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Banner ad failed to load. MAX will automatically try loading a new ad internally.
        Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Banner ad clicked");
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Banner ad revenue paid. Use this callback to track user revenue.
        Debug.Log("Banner ad revenue paid");

        // Ad revenue
        double revenue = adInfo.Revenue;
        
        // Miscellaneous data
        string countryCode = MaxSdk.GetSdkConfiguration().CountryCode; // "US" for the United States, etc - Note: Do not confuse this with currency code which is "USD" in most cases!
        string networkName = adInfo.NetworkName; // Display name of the network that showed the ad (e.g. "AdColony")
        string adUnitIdentifier = adInfo.AdUnitIdentifier; // The MAX Ad Unit ID
        string placement = adInfo.Placement; // The placement this ad's postbacks are tied to
    }
    #endregion
}
