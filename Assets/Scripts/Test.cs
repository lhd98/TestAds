using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    [SerializeField] private int gold;
    [SerializeField] private Text goldText;
    
    private void Start()
    {
        GOAD.Instance.showBanner();
        goldText.text = $"Gold: {gold}";
    }

    public void TestInterstitial()
    {
        GOAD.Instance.showIntertital(true, false);
    }

    public void TestRewarded()
    {
        GOAD.Instance.ShowVideoReward(b =>
        {
            if (b)
            {
                GetGoldRewarded();
            }
        });
    }

    void GetGoldRewarded()
    {
        gold++;
        goldText.text = $"Gold: {gold}";
    }
}
