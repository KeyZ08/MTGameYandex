using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Lose : MonoBehaviour
{
    public Button BtnRestart;
    public Button BtnHome;
    public Button BtnLoseReset;
    public AudioSource lose;

    private LevelUIs mainManager;
    private FullscreenAds fullscreenAds;
    private RewardedAds rewardedAds;

    private bool isWaitLoseClick;
    private bool isResetLose;
    private Action resetLoseAction;
    private Action notResetLose;

    private void Start()
    {
        mainManager = FindAnyObjectByType<LevelUIs>();

        BtnRestart.onClick.AddListener(Restart);
        BtnHome.onClick.AddListener(Home);
        BtnLoseReset.onClick.AddListener(() => { resetLoseAction(); });

        var ads = GameObject.FindWithTag("Ads");
        fullscreenAds = ads.GetComponent<FullscreenAds>();
        rewardedAds = ads.GetComponent<RewardedAds>();

        resetLoseAction = new Action(() => { isResetLose = true; isWaitLoseClick = false; });
        notResetLose = new Action(() => { isResetLose = false; isWaitLoseClick = false; });
    }

    public IEnumerator WaitClickResetLoseCoroutine(Action ifResetLose)
    {
        isWaitLoseClick = true;
        while (isWaitLoseClick)
        {
            yield return null;
        }
        if (isResetLose)
        {
            rewardedAds.ShowAd();
            yield return StartCoroutine(rewardedAds.WaitAdsCoroutine(
                () => { ifResetLose(); Close(); },
                () => { BtnLoseReset.gameObject.SetActive(false); })
                );
        }
        isWaitLoseClick = false;
    }

    public void Home()
    {
        notResetLose();
        fullscreenAds.ShowAd();
        StartCoroutine(fullscreenAds.WaitAdsCoroutine(mainManager.MainMenu));
    }

    public void Restart()
    {
        notResetLose();
        fullscreenAds.ShowAd();
        StartCoroutine(fullscreenAds.WaitAdsCoroutine(mainManager.Restart));
    }

    public void Show(bool resetLoseBtnActive)
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
        lose.Play();
        if (resetLoseBtnActive)
        {
            BtnLoseReset.gameObject.SetActive(true);
        }
        else
        {
            BtnLoseReset.gameObject.SetActive(false);
        }
    }

    public void Close()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        lose.Stop();
    }
}
