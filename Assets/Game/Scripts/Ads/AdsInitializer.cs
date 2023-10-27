using UnityEngine;
using YG;

public class AdsInitializer : MonoBehaviour
{
    private bool isPlayed;
    public bool IsPlayed { get { return isPlayed; } }
    private AudioManager audioManager;

    private void Awake()
    {
        var obj = GameObject.FindGameObjectsWithTag("Ads");
        if (obj.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        if (YandexGame.SDKEnabled)
            CheckSDK();

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
    private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;

    private void CheckSDK()
    {
        YandexGame.CloseFullAdEvent += () 
            => { CloseShow(); Debug.Log("FullscreenAd Close"); };

        YandexGame.ErrorFullAdEvent += () 
            => { CloseShow(); Debug.Log("Fullscreen Error"); };

        YandexGame.OpenFullAdEvent += ()
            => { StartShow(); Debug.Log("FullscreenAd Show"); };

        if (YandexGame.auth)
            Debug.Log("User auth OK");
        else
        {
            Debug.Log("User auth ERROR");
            YandexGame.AuthDialog();
        }
    }

    public void StartShow()
    {
        audioManager.MasterVolumeChange(-80);
        isPlayed = true;
    }

    private void CloseShow()
    {
        audioManager.MasterVolumeChange(0);
        isPlayed = false;
    }
}
