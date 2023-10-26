using UnityEngine;
using UnityEngine.UI;

public class Lose : MonoBehaviour
{
    public Button BtnRestart;
    public Button BtnHome;

    public AudioSource lose;

    private LevelUIs mainManager;

    private InterstitialAds interstitialAds;

    private void Start()
    {
        mainManager = FindAnyObjectByType<LevelUIs>();

        BtnRestart.onClick.AddListener(Restart);
        BtnHome.onClick.AddListener(Home);

        var ads = GameObject.FindWithTag("Ads");
        interstitialAds = ads.GetComponent<InterstitialAds>();
    }


    public void Home()
    {
        interstitialAds.ShowAd();
        StartCoroutine(interstitialAds.WaitAdsCoroutine(mainManager.MainMenu));
    }

    public void Restart()
    {
        interstitialAds.ShowAd();
        StartCoroutine(interstitialAds.WaitAdsCoroutine(mainManager.Restart));
    }

    public void Show()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        lose.Play();
    }
}
