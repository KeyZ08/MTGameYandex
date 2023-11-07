using System;
using System.Collections;
using UnityEngine;
using YG;

public class RewardedAds : MonoBehaviour
{
    public AdsInitializer instance;
    private bool showComplete;

    private void OnEnable()
    {
        YandexGame.CloseVideoEvent += ()
            => { EndShow(); Debug.Log("RewardedAd Close"); };

        YandexGame.ErrorVideoEvent += ()
            => { EndShow(); Debug.Log("RewardedAd Error"); };

        YandexGame.OpenVideoEvent += ()
            => { Debug.Log("RewardedAd Show"); };

        YandexGame.RewardVideoEvent += (id)
            => { showComplete = true; Debug.Log($"RewardedAd Id={id} Complete"); };
    }

    public void ShowAd()
    {
        YandexGame.RewVideoShow(1);
        instance.StartShow();
    }

    private void EndShow() => instance.EndShow();

    public IEnumerator WaitAdsCoroutine(Action actionComplete, Action actionError)
    {
        while (instance.IsPlayed)
        {
            //Debug.Log("WaitAds");
            yield return null;
        }
        if (showComplete)
            actionComplete.Invoke();
        else
            actionError?.Invoke();
        showComplete = false;
        yield return null;
    }
}