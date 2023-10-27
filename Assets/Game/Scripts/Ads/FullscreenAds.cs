using System;
using System.Collections;
using UnityEngine;
using YG;

public class FullscreenAds : MonoBehaviour
{
    public AdsInitializer instance;
    private DateTime lastShow;

    public void ShowAd()
    {
        Debug.Log("����� ��������� ������: " + Math.Abs((DateTime.UtcNow - lastShow).TotalSeconds) + " ������");
        if (Math.Abs((DateTime.UtcNow - lastShow).TotalSeconds) > 150)
        {
            YandexGame.FullscreenShow();
        }
    }

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