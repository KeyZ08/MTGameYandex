using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string androidAdID = "Rewarded_Android";
    [SerializeField] string iOSAdID = "Rewarded_iOS";
    private AudioManager audioManager;
    private string adID;
    private bool isPlayed;

    private void Start()
    {
        adID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSAdID : androidAdID;
        audioManager = GetComponent<AudioManager>();
    }

    public void ShowAd()
    {
        Advertisement.Load(adID, this);
        Advertisement.Show(adID, this);
        audioManager.MasterVolumeChange(-80);
        isPlayed = true;
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("������� ���������: " + placementId);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"������ �������� �������: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"������ ������ �������: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("����� ������ �������: " + placementId); 
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("���� �� �������: " + placementId);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            // ��� ��� ��� ���������� ������� ������.
            Debug.Log("����� �������� ����� �������, � ������� ������ ������.");
        }
        audioManager.MasterVolumeChange(0);
        isPlayed = false;
    }

    public IEnumerator WaitAdsCoroutine(Action action)
    {
        while (isPlayed)
        {
            Debug.Log("WaitAds");
            yield return null;
        }
        action.Invoke();
        yield return null;
    }
}