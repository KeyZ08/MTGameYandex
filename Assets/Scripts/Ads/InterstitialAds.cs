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
        Debug.Log("����� ��������� ������: " + Math.Abs((DateTime.UtcNow - lastShow).TotalSeconds) + " ������");
        if (Math.Abs((DateTime.UtcNow - lastShow).TotalSeconds) > 150)
        {
            Advertisement.Show(adID, this);
            audioManager.MasterVolumeChange(-80);
            isPlayed = true;
        }
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("������� ���������: " + placementId);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"������ �������� �������: {error.ToString()} - {message}");
        Advertisement.Load(adID, this);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"������ ������ �������: {error.ToString()} - {message}");
        Close();
        Advertisement.Load(adID, this);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("����� ������ �������: " + placementId);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("���� �� �������: " + placementId);
        Close();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("����� �������� ����� �������.");
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