using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Linq;
using System;
using System.Net;
using System.Globalization;
using TMPro;

public class UtilGame
{
    // DAILY
    public const string DAILY_REWARD_KEY_INDEX = "DAILY_REWARD_KEY_INDEX";
    public const string DAILY_REWARD_KEY_CLAIM = "DAILY_REWARD_KEY_CLAIM";
    public const string DAILY_REWARD_KEY_DATE_SAVE = "DAILY_REWARD_KEY_DATE_SAVE";
    public const string KEY_INSTALL = "KEY_INSTALL";

    public const string KEY_UNLOCK_SKIN = "KEY_UNLOCK_SKIN";
    public const string KEY_CURRENT_SKIN = "KEY_CURRENT_SKIN";
    public const string KEY_CURRENT_GEAR_SKIN = "KEY_CURRENT_GEAR_SKIN";
    public const string KEY_LEVEL_CURRENT = "KEY_LEVEL_CURRENT";
    public const string KEY_LEVEL_CHOOSE = "KEY_LEVEL_CHOOSE";
    public const string KEY_LEVEL_STAR = "KEY_LEVEL_STAR";
    //ACHIVEMENT
    public const string ACHIVEMENT_REWARD_KEY_DATE_SAVE = "ACHIVEMENT_REWARD_KEY_DATE_SAVE";
    public const string ACHIVEMENT_VALUE_KEY_SUPERTHIEF = "ACHIVEMENT_VALUE_KEY_SUPERTHIEF";
    public const string ACHIVEMENT_VALUE_KEY_NAUGHTY = "ACHIVEMENT_VALUE_KEY_NAUGHTY";
    public const string ACHIVEMENT_VALUE_KEY_ASSASSIN = "ACHIVEMENT_VALUE_KEY_ASSASSIN";
    //Shop
    public const string COIN_KEY_INDEX = "COIN_KEY_INDEX";
    public const string HINT_KEY_INDEX = "HINT_KEY_INDEX";
    // Key push Popup

    public const string KEY_NOTIFILECATION = "KEY_NOTIFILECATION_";
    public const string KEY_REMOVEADS = "KEY_REMOVEADS_AAA";
    public const string KEY_MUSIC = "MUSIC";
    public const string KEY_SOUND = "SOUND";
    public const string KEY_VIBRATE = "VIBRATE";
    public const string KEY_LANGUAGE = "language";
    public const string url_support = "";
    public const string KEY_RATE_US = "KEY_RATE_US";
    public const string url_rate = "https://play.google.com/store/apps/details?id=";
    
    // SPIN
    public const string KEY_FREE_SPIN_START_ALL = "KEY_FREE_SPIN_ALL";
    public const string KEY_FREE_SPIN_CLAIM = "KEY_FREE_SPIN_CLAIM";
    public const string KEY_DATE_SPIN = "KEY_DATE_SPIN";
    public const string KEY_DATE_FREECLIAM = "KEY_DATE_FREECLIAM";
    public const string KEY_NEWVERSION = "KEY_NEWVERSION";

    public const float MAX_SLIDER = 600;

    public const string KEY_HIGHEST_LEVEL = "HIGHEST_LEVEL";

    public static void SetDataString(string key, string value, string child = "none")
    {
        ObscuredPrefs.SetString(key + "_" + child + "_" + Application.identifier, value);
        ObscuredPrefs.Save();
    }

    public static string GetDataString(string key, string child = "none", string defualt = "none")
    {
        return ObscuredPrefs.GetString(key + "_" + child + "_" + Application.identifier, defualt);
    }

    public static void SetDataInt(string key, int value, string child = "none")
    {
        ObscuredPrefs.SetInt(key + "_" + child + "_" + Application.identifier, value);
        Debug.Log("KEY_OK : " + (key + "_" + child + "_" + Application.identifier));
        ObscuredPrefs.Save();
    }

    public static int GetDataInt(string key, int defualt = 0, string child = "none")
    {
        return ObscuredPrefs.GetInt(key + "_" + child + "_" + Application.identifier, defualt);
    }

    public static void SetDataFloat(string key, float value)
    {
        ObscuredPrefs.SetFloat(key + "_" + Application.identifier, value);
        ObscuredPrefs.Save();
    }

    public static float GetDataFloat(string key, float defualt = 0, string child = "none")
    {
        return ObscuredPrefs.GetFloat(key + "_" + child + "_" + Application.identifier, defualt);
    }

    public static void SetDataDouble(string key, double value)
    {
        ObscuredPrefs.SetDouble(key + "_" + Application.identifier, value);
        ObscuredPrefs.Save();
    }

    public static double GetDataDouble(string key, float defualt = 0, string child = "none")
    {
        return ObscuredPrefs.GetDouble(key + "_" + child + "_" + Application.identifier, defualt);
    }

    public static void SetDataBool(string key, bool value, string child = "none")
    {
        ObscuredPrefs.SetBool(key + "_" + child + "_" + Application.identifier, value);
        ObscuredPrefs.Save();
    }

    public static bool GetDataBool(string key, bool defualt = false, string child = "none")
    {
        return ObscuredPrefs.GetBool(key + "_" + child + "_" + Application.identifier, defualt);
    }


    public bool HasInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
            return false;
        }

        return true;
    }

    #region Data Save Client

    public static int GetInt(string key, int adefault)
    {
        return ObscuredPrefs.GetInt(key + "_none_" + Application.identifier, adefault);
    }

    public static void SetInt(string key, int value)
    {
        ObscuredPrefs.SetInt(key + "_none_" + Application.identifier, value);
        ObscuredPrefs.Save();
    }

    public static float GetFloat(string key, float adefualt)
    {
        return ObscuredPrefs.GetFloat(key + "_none_" + Application.identifier, adefualt);
    }

    public static void SetFloat(string key, float value)
    {
        ObscuredPrefs.SetFloat(key + "_none_" + Application.identifier, value);
        ObscuredPrefs.Save();
    }

    public static double GetDoubleU(string key, double adefualt)
    {
        return ObscuredPrefs.GetDouble(key + "_none_" + Application.identifier, adefualt);
    }

    public static void SetDouble(string key, double value)
    {
        ObscuredPrefs.SetDouble(key + "_none_" + Application.identifier, value);
        ObscuredPrefs.Save();
    }

    public static bool GetBool(string key, bool adefualt)
    {
        return ObscuredPrefs.GetBool(key + "_none_" + Application.identifier, adefualt);
    }

    public static void SetBool(string key, bool value)
    {
        ObscuredPrefs.SetBool(key + "_none_" + Application.identifier, value);
        ObscuredPrefs.Save();
    }

    public static string GetString(string key, string adefualt)
    {
        return ObscuredPrefs.GetString(key + "_none_" + Application.identifier, adefualt);
    }

    public static void SetString(string key, string value)
    {
        ObscuredPrefs.SetString(key + "_none_" + Application.identifier, value);
        ObscuredPrefs.Save();
    }

    #endregion


    public static bool isHashkey(string key)
    {
        return ObscuredPrefs.HasKey(key + "_none_" + Application.identifier);
    }

    public static bool isHashkeyData(string key, string child = "none")
    {
        return ObscuredPrefs.HasKey(key + "_" + child + "_" + Application.identifier);
    }

    public static void DeleteKey(string key)
    {
        ObscuredPrefs.DeleteKey(key + "_none_" + Application.identifier);
    }

    public static void DeleteKeyData(string key, string child = "none")
    {
        ObscuredPrefs.DeleteKey(key + "_" + child + "_" + Application.identifier);
    }

    public static DateTime NewTimeNow(DateTime dateTime, float minuteNow)
    {
        int hour = dateTime.Hour;
        int minute = dateTime.Minute;
        var day = dateTime.Day;
        if (minute > 0)
        {
            minute = dateTime.Minute + ((int) minuteNow);
            if (minute >= 60)
            {
                hour = hour + 1;
                minute = 0;
            }
        }

        if (hour > 24)
        {
            day += 1;
        }

        return new DateTime(dateTime.Year, dateTime.Month, day, hour, minute, 0);
    }


    public static bool IsConnectionNetwork()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    public static int getSecondsLeft(int hours, int minutes, int seconds)
    {
        //Create Desired time
        DateTime target =
            new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds);
        //Get the current time
        DateTime now = System.DateTime.Now;

        //Convert both to seconds
        int targetSec = target.Hour * 60 * 60 + target.Minute * 60 + target.Second;
        int nowSec = now.Hour * 60 * 60 + now.Minute * 60 + now.Second;

        //Get the difference in seconds
        int diff = targetSec - nowSec;
        Debug.Log("DAY 02: " + diff);
        if (diff > 0)
        {
            return diff + 2;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// True la show dc  / False khong show dc
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static bool CheckedTimer(string date)
    {
        try
        {
            if (date.ToUpper() == "none".ToUpper())
            {
                return true;
            }
            else
            {
                Debug.Log("DATE : parse: " + date);
                DateTime dateTimeOld = DateTime.Parse(date);
                DateTime dateTimeNow = DateTime.Now;

                if (dateTimeNow.Day > dateTimeOld.Day)
                {
                    Debug.Log("DATE: " + dateTimeNow.Day);
                    return true;
                }
                else
                {
                    if (dateTimeNow.Month > dateTimeOld.Month)
                    {
                        Debug.Log("DATE: " + dateTimeNow.Month);
                        return true;
                    }
                    else
                    {
                        if (dateTimeNow.Year > dateTimeOld.Year)
                        {
                            Debug.Log("DATE: " + dateTimeNow.Year);
                            return true;
                        }
                    }
                }
            }
        }
        catch (FormatException e)
        {
            return false;
        }

        return false;
    }


    public static void SetTextColor(TextMeshProUGUI txt, string colors)
    {
        Color myColor = new Color();
        ColorUtility.TryParseHtmlString(colors, out myColor);
        txt.color = myColor;
    }


    //public static int GetNumberOfLevelUnlock()
    //{
    //    const int MAP_NUMBER = 10;

    //    var res = 0;

    //    for (var i = 0; i < MAP_NUMBER; i++)
    //    {
    //        var key = KeyString.Map.ToString() + i;
    //        var mapStatus = ObscuredPrefs.GetString(key);

    //        for (var j = 0; j < mapStatus.Length; j++)
    //        {
    //            res += mapStatus[j] != '0' ? 1 : 0;
    //        }
    //    }

    //    res += 1;

    //    return res;
    //}
}