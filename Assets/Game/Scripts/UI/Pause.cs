using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(AudioManager))]
public class Pause : MonoBehaviour
{
    [Header("Buttons")]
    public Button BtnPlay;
    public Button BtnHome;
    public Button BtnTraining;
    private Button BtnPause;

    [Header("Audio")]
    public AudioSource BtnClick;
    public SwitchHandler Sounds;
    public SwitchHandler Music;
    private AudioManager audioManager;


    private FullscreenAds interstitialAds;

    public bool IsOpen { get; private set; }

    private void Awake()
    {
        BtnPause = GameObject.FindGameObjectWithTag("PauseBtn").GetComponent<Button>();
        BtnPause.onClick.AddListener(Show);
        audioManager = GetComponent<AudioManager>();

        var ads = GameObject.FindWithTag("Ads");
        interstitialAds = ads == null ? null : ads.GetComponent<FullscreenAds>();
    }

    private void Start()
    {
        var book = FindAnyObjectByType<TrainingBook>(FindObjectsInactive.Include);
        BtnTraining.onClick.AddListener(() => { book.Open(); BtnClick.Play(); }) ;
        BtnPlay.onClick.AddListener(Play);
        BtnHome.onClick.AddListener(Home);

        Sounds.onClick.AddListener(audioManager.SoundsVolumeChange);
        Music.onClick.AddListener(audioManager.MusicVolumeChange);
        Sounds.SetValue(audioManager.SoundIsPlayed);
        Music.SetValue(audioManager.MusicIsPlayed);

        Close();
    }

    public void Home()
    {
        BtnClick.Play();
        Time.timeScale = 1;
        audioManager.ScaledSoundsChange(0);
        if (interstitialAds != null)
        {
            interstitialAds.ShowAd();
            StartCoroutine(interstitialAds.WaitAdsCoroutine(() => { SceneManager.LoadScene("Menu"); }));
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }

    public void Play()
    {
        BtnClick.Play();
        Close();
    }

    public void Show()
    {
        Time.timeScale = 0;
        audioManager.ScaledSoundsChange(-80);
        gameObject.SetActive(true);
        BtnPause.gameObject.SetActive(false); 
        IsOpen = true;
        BtnClick.Play();
    }

    public void Close()
    {
        Time.timeScale = 1;
        audioManager.ScaledSoundsChange(0);
        gameObject.SetActive(false);
        BtnPause.gameObject.SetActive(true);
        IsOpen = false;
    }
}
