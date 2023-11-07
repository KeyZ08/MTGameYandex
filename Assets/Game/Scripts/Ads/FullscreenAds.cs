using System;
using System.Collections;
using UnityEngine;
using YG;

public class FullscreenAds : MonoBehaviour
{
    public AdsInitializer instance;
    private DateTime lastShow;

    private void OnEnable()
    {
        YandexGame.CloseFullAdEvent += ()
            => { EndShow(); Debug.Log("FullscreenAd Close"); };

        YandexGame.ErrorFullAdEvent += ()
            => { EndShow(); Debug.Log("Fullscreen Error"); };

        YandexGame.OpenFullAdEvent += ()
            => { StartShow(); Debug.Log("FullscreenAd Show"); };
    }

    public void ShowAd()
    {
        Debug.Log("между рекламами прошло: " + Math.Abs((DateTime.UtcNow - lastShow).TotalSeconds) + " секунд");
        if (Math.Abs((DateTime.UtcNow - lastShow).TotalSeconds) > 150)
        {
            YandexGame.FullscreenShow();
        }
    }

    private void StartShow() => instance.StartShow();

    private void EndShow() => instance.EndShow();

    public IEnumerator WaitAdsCoroutine(Action action)
    {
        while (instance.IsPlayed)
        {
            //Debug.Log("WaitAds");
            yield return null;
        }
        action.Invoke();
        yield return null;
    }
}