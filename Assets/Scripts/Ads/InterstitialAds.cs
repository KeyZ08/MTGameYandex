using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string androidAdID = "Interstitial_Android";
    [SerializeField] string iOSAdID = "Interstitial_iOS";
    private AudioManager audioManager;
    private string adID;
    private bool isPlayed;
    private DateTime lastShow;

    private void Start()
    {
        adID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSAdID : androidAdID;
        audioManager = GetComponent<AudioManager>();

        Advertisement.Load(adID, this);
    }

    public void ShowAd()
    {
        Debug.Log("между рекламами прошло: " + Math.Abs((DateTime.UtcNow - lastShow).TotalSeconds) + " секунд");
        if (Math.Abs((DateTime.UtcNow - lastShow).TotalSeconds) > 150)
        {
            Advertisement.Show(adID, this);
            audioManager.MasterVolumeChange(-80);
            isPlayed = true;
        }
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Реклама загружена: " + placementId);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Ошибка загрузки рекламы: {error.ToString()} - {message}");
        Advertisement.Load(adID, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Ошибка показа рекламы: {error.ToString()} - {message}");
        Close();
        Advertisement.Load(adID, this);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Старт показа рекламы: " + placementId);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("клик по рекламе: " + placementId);
        Close();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("Юнити завершил показ рекламы.");
        Close();
        lastShow = DateTime.UtcNow;
        Advertisement.Load(adID, this);
    }

    public IEnumerator WaitAdsCoroutine(Action action)
    {
        while (isPlayed)
        {
            //Debug.Log("WaitAds");
            yield return null;
        }
        action.Invoke();
        yield return null;
    }

    private void Close()
    {
        audioManager.MasterVolumeChange(0);
        isPlayed = false;
    }
}