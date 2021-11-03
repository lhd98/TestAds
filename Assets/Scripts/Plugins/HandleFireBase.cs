using System;
using System.Collections;
using System.Threading.Tasks;
using Firebase;
using Firebase.Analytics;
//using UnityEngine.Purchasing;
using Firebase.RemoteConfig;
using UnityEngine.SocialPlatforms;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;

public class HandleFireBase : MonoBehaviour
{
    public const string INTERSTITIAL_ADS = "INTERSTITIAL_ADS";
    public const string LEVEL_PLAY = "LEVEL_PLAY_";
    public const string LEVEL_WIN = "LEVEL_WIN_";
    public const string LEVEL_NEXTLEVEL = "LEVEL_NEXTLEVEL_";
    public const string LEVEL_RATE = "LEVEL_RATE";
    public const string LEVEL_HINT = "LEVEL_HINT_";
    public const string LEVEL_RESTART = "LEVEL_RESTART_";
    

    Coroutine _coroutine;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    public static HandleFireBase Instance =null; 
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
       
    }

    void Start()
    {

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            Debug.Log("FIREBASE STATUS: " + dependencyStatus.ToString());
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    void InitializeFirebase()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(FirebaseAnalytics.UserPropertySignUpMethod, "Google");
        //Set the user ID.

    }
    public void LogEventWithFloat(string eventName, string parameterName, float value)
    {
        FirebaseAnalytics.LogEvent(eventName, parameterName, value);
    }
    public void LogEventParameter(string eventName, Parameter param)
    {
        // FirebaseAnalytics.LogEvent (eventName, param);
    }
    

    public void LogEventWithString(string eventName)
    {
        FirebaseAnalytics.LogEvent(eventName);
    }

    public void LogCurrentScreen(string nameScreen, string screenClass = "main")
    {
        FirebaseAnalytics.SetCurrentScreen(nameScreen, screenClass);
    }
    public void LogLevelStart(int level)
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelStart, new Parameter(FirebaseAnalytics.ParameterLevel, level));
    }
    public void LogLevelEnd(int level)
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLevelUp, new Parameter(FirebaseAnalytics.ParameterLevel, level));
    }
}
