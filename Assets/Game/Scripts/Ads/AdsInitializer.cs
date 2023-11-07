using UnityEngine;
using YG;

public class AdsInitializer : MonoBehaviour
{
    private bool isPlayed;
    private AudioManager audioManager;

    public bool IsPlayed { get { return isPlayed; } }

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
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
        if (YandexGame.auth)
            Debug.Log("User auth OK");
        else
        {
            Debug.Log("User auth ERROR");
            //YandexGame.AuthDialog();
        }
    }

    public void StartShow()
    {
        audioManager.MasterVolumeChange(-80);
        isPlayed = true;
    }

    public void EndShow()
    {
        audioManager.MasterVolumeChange(0);
        isPlayed = false;
    }
}
