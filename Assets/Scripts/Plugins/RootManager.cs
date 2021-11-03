using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager : MonoBehaviour
{
    public bool UnlockLevel = false;
    public bool isInapp = false;
    public bool isTestNotifile = false;
    public bool enableLog = false;
    public bool deleteData = false;
    public bool isTestSute = false;
    public bool isTestAD = false;
    public bool isRemoveAds = false;
    public string version = "1.0.0";

/// <summary>
///  true la ban moi
///  False : da la ban cu update len 
/// </summary>
    public bool InNewVersion = false;

    public static RootManager Instance = null;
    public void Awake()
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
        
        Debug.LogError("VERSION: " + Application.version);
        version = Application.version.Replace(".", "");
        Debug.LogError("VERSION1: " + version);
        Debug.unityLogger.logEnabled = !enableLog;
        if (deleteData)
        {
            PlayerPrefs.DeleteAll();
        }

        isRemoveAds = UtilGame.GetDataBool(UtilGame.KEY_REMOVEADS, false);
        if (!UtilGame.isHashkeyData(UtilGame.KEY_NEWVERSION))
        {
            UtilGame.SetString(UtilGame.KEY_NEWVERSION, version.ToString());
            InNewVersion = true;
        }
        else
        {
            InNewVersion = false;
        }
        
        
        

    }
    #region Application check Resume

    bool isFocus = false;

    private void OnApplicationFocus(bool focus)
    {
        this.isFocus = focus;
    }

    private void OnApplicationPause(bool pause)
    {
        if (GOAD.Instance != null)
        {
            if (GOAD.Instance.AdSetting_Resume == 1)
            {
                Debug.LogError("OnApplicationPause: " + pause);
                if (!pause)
                    Debug.LogError("FOCUS: " + isFocus);
                if (isFocus)
                {
                    Debug.LogError("isPaused: " + pause);

                    if (GOAD.Instance != null)
                    {
                        Debug.LogError("OnRESUME: SHOW AD");
                        GOAD.Instance.showIntertitialFocus();
                    }
                    // isFocus = false;
                }
            }
        }
    }

    #endregion
}
