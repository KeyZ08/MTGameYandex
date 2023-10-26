using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Game;

public class Win : MonoBehaviour
{
    [Header("Buttons")]
    public Button BtnNext;
    public Button BtnHome;

    [Header("Message")]
    public TextMeshProUGUI Message;
    public TextMeshProUGUI SubMessage;
    public TextMeshProUGUI MoneyCount;
    public Image MoneyImg;

    [Header("AudioSource")]
    public AudioSource open;

    private LevelUIs mainManager;
    private int _money;

    private InterstitialAds interstitialAds;

    public void Init()
    {
        BtnNext.onClick.AddListener(Next);
        BtnHome.onClick.AddListener(Home);

        var ads = GameObject.FindWithTag("Ads");
        interstitialAds = ads == null ? null : ads.GetComponent<InterstitialAds>();
    }

    public void SetParams(int moneyCount, string message="Получено:", bool centered = false, string subMessage="")
    {
        if (moneyCount < 0) throw new ArgumentException("moneyCount < 0");
        if (centered) 
            Message.alignment = TextAlignmentOptions.Center;

        Message.text = message;
        _money = moneyCount;
        MoneyCount.text = moneyCount.ToString();
        SubMessage.text = subMessage;
        if (moneyCount == 0)
        {
            MoneyCount.text = "";
            MoneyImg.enabled = false;
            _money = 0;
        }
    }

    public void SetParams(WinSettings settings)
    {
        if (settings.MoneyCount < 0) throw new ArgumentException("moneyCount < 0");
        if (settings.Centered) 
            Message.alignment = TextAlignmentOptions.Center;

        Message.text = settings.Message;
        _money = settings.MoneyCount;
        MoneyCount.text = settings.MoneyCount.ToString();
        SubMessage.text = settings.SubMessage;
        if (settings.MoneyCount == 0)
        {
            MoneyCount.text = "";
            MoneyImg.enabled = false;
            _money = 0;
        }
    }

    public void Home()
    {
        if (interstitialAds != null)
        {
            interstitialAds.ShowAd();
            StartCoroutine(interstitialAds.WaitAdsCoroutine(mainManager.MainMenu));
        }
        else
        {
            mainManager.MainMenu();
        }
    }

    public void Next()
    {
        if (interstitialAds != null)
        {
            interstitialAds.ShowAd();
            StartCoroutine(interstitialAds.WaitAdsCoroutine(mainManager.NextLevel));
        }
        else
        {
            mainManager.NextLevel();
        }
    }

    public void Show()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        mainManager = FindAnyObjectByType<LevelUIs>();
        open.Play();
        mainManager.SaveProgress();
        MoneyManager.Add(_money);
    }
}
