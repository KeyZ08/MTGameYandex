using Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public MapLevel[] levels;
    public GameObject[] backgrounds;

    private void Update()
    {
        if (Input.GetAxis("Cancel") != 0)
        {
            Back();
        }
    }

    private void Awake()
    {
        if (PlayerPrefs.GetInt("FirstGame", 0) == 0)
        {
            PlayerPrefs.SetInt("FirstGame", 1);
            SceneManager.LoadScene(CutsceneManager.CutScenes["Start"]);
        }
    }

    private void Start()
    {

        var last = LevelManager.LastLevel;
        if (last < 0) throw new ArgumentException("LastLevel не может быть < 0, текущее значение: " + last);
        for (var i = 0; i < levels.Length; i++)
        {
            var n = i;
            if (last >= i)
            {
                levels[i].IsOpened = true;
                levels[i].btn.onClick.AddListener(() => { Click(n); });
            }
        }
        SetBackground();
    }

    public void Click(int level)
    {
        LevelManager.ActiveLevel = level;
        var l = Map.Levels[level];

        if (level == LevelManager.LastLevel && LevelManager.LastTraining < level)//LevelManager.LastTraining можно использовать как индикатор что мы это уже смотрели)
        {
            var cutscene = CutsceneManager.GetCutscene(level);
            if (cutscene != null)
            {
                SceneManager.LoadScene(cutscene);
                return;
            }
        }

        var interactiveScene = InteractiveTrainingManager.GetScene(level);
        if (interactiveScene != null && LevelManager.LastInteractiveTraining < level)
        {
            SceneManager.LoadScene(interactiveScene);
            return;
        }

        SceneManager.LoadScene(l.SceneName);
    }

    private void Back()
    {
        SceneManager.LoadScene("Menu");
    }

    private void SetBackground()
    {
        var level = LevelManager.ActiveLevel;
        var canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        var l = 0;
        if (level > 4)
            l += 1;
        if (level > 9)
            l += 1;
        if (level > 13)
            l += 1;
        if (level > 19)
            l += 1;
        if (level > 24)
            l += 1;

        Instantiate(backgrounds[l], canvas.transform);
    }
}
