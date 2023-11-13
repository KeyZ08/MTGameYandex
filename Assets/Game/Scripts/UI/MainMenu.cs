using Game;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(AudioManager))]
public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Button BtnPlay;
    public Button BtnInfinityGame;
    public Button BtnSettings;
    public Button BtnTraining;

    [Header("AudioSwitches")]
    public SwitchHandler Sounds;
    public SwitchHandler Music;

    [Header("TrainingPrefabs")]
    public GameObject[] AllTrainingBooks;

    [Header("Loading")]
    public GameObject loadingPanel;

    private GameObject hero;
    private AudioManager audioManager;


    //private IEnumerator Start()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    YandexGame.ResetSaveProgress();
    //    YandexGame.SaveProgress();
    //    //Обязательно удалить или закомментить! это для отладки
    //    //YandexGame.savesData.Statistic = null;
    //    //YandexGame.ResetSaveProgress();
    //    //YandexGame.savesData.FirstGame = 1;
    //    //YandexGame.savesData.PlayerMoney = 500;
    //    //YandexGame.savesData.LastInteractiveTraining = 0;
    //    //YandexGame.savesData.LastLevel = 9;

    //    //YandexGame.SaveProgress();
    //}

    private IEnumerator Start()
    {
        if (!YandexGame.SDKEnabled)
        {
            loadingPanel.SetActive(true);
            while (!YandexGame.SDKEnabled)
            {
                yield return new WaitForSeconds(0.5f);
            }
            YandexGame.GameReadyAPI();
            loadingPanel.SetActive(false);
        }
        //YandexGame.ResetSaveProgress();
        //YandexGame.SaveProgress();

        var lastLevel = LevelManager.LastLevel;
        if (lastLevel == Map.Levels.Count)
            lastLevel -= 1;
        var level = Map.Levels[lastLevel];
        SetTrainingBook(level.Training);

        audioManager.Init();
        Sounds.SetValue(audioManager.SoundIsPlayed);
        Music.SetValue(audioManager.MusicIsPlayed);
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        audioManager = GetComponent<AudioManager>();
        hero = GameObject.FindGameObjectWithTag("Player");
        hero.GetComponent<Animator>().SetBool("fly", true);
        Time.timeScale = 1;

        BtnPlay.onClick.AddListener(() => { SceneManager.LoadScene("Map"); });
    }

    private void SetTrainingBook(TrainingSettings trainingSettings)
    {
        if (trainingSettings.TrainingBook > AllTrainingBooks.Length - 1)
            throw new IndexOutOfRangeException("TrainingSettings.TrainingBook");
        var bookPrefab = AllTrainingBooks[trainingSettings.TrainingBook];
        if (trainingSettings.ActivePage > bookPrefab.GetComponent<TrainingBook>().Pages.Count - 1 || trainingSettings.ActivePage < 0)
            throw new IndexOutOfRangeException("TrainingSettings.ActivePage");

        var bookObj = Instantiate(bookPrefab);
        bookObj.SetActive(true);
        bookObj.GetComponent<Canvas>().worldCamera = Camera.main;

        var book = bookObj.GetComponent<TrainingBook>();
        book.ActivePage = trainingSettings.ActivePage;
        book.IsActiveOnLoad = false;

        BtnTraining.onClick.AddListener(book.Open);
    }
}
