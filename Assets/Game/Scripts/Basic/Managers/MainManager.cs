using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;
using System.Collections.Generic;
using YG;

public class MainManager : GameManagerSetParams
{
    /*
        устанавливает:
            - преднастройки дл€ уровней
            - текст и монеты на победном экране
            - LastLevel, тем самым определ€€ какие уровни открыты а какие закрыты
            - LastTraining дл€ того чтобы просмотренное обучение не открывалось заново само
    */

    protected override void Awake()
    {
        base.Awake();
        CreateUIs();
        /*Ёто дл€ отладки, чтобы ничего не ломалось пока*/
        if (_activeScene != Map.Levels[_activeLevel].SceneName)
        {
            SetLevelDebug();
            return;
        }

        Debug.Log("«агружен уровень " + _activeLevel);
        if (_activeScene != Map.Levels[_activeLevel].SceneName)
            throw new Exception($"Ќесоответсвие загруженной сцены и параметров уровн€: уровень {_activeLevel}, " +
                $"его сцена {Map.Levels[_activeLevel].SceneName} , а загружена сцена {_activeScene}") ;
        
        SetLevel();
    }

    public override void NextLevel()
    {
        if (_activeLevel + 1 >= Map.Levels.Count)
        {
            Debug.Log("”ровни кончились");
            if (YandexGame.savesData.EndGame == 0)
            {
                YandexGame.savesData.EndGame = 1;
                YandexGame.SaveProgress();
                SceneManager.LoadScene(CutsceneManager.CutScenes["End"]);
            }
            else
                SceneManager.LoadScene("Menu");
            return;
        }

        LevelManager.ActiveLevel += 1;
        _activeLevel += 1;                

        if (_activeLevel == _lastLevel && LevelManager.LastTraining  < _activeLevel)//LevelManager.LastTraining можно использовать как индикатор что мы это уже смотрели
        {
            var cutscene = CutsceneManager.GetCutscene(_activeLevel);
            if (cutscene != null)
            {
                SceneManager.LoadScene(cutscene);
                return;
            }
        }

        var interactiveScene = InteractiveTrainingManager.GetScene(_activeLevel);
        if (interactiveScene != null && LevelManager.LastInteractiveTraining < _activeLevel)
        {
            SceneManager.LoadScene(interactiveScene);
            return;
        }

        SceneManager.LoadScene(Map.Levels[_activeLevel].SceneName);
    }

    public override void SaveProgress()
    {
        if (_lastLevel < _activeLevel + 1)
        {
            LevelManager.LastLevel = _activeLevel + 1;
            _lastLevel = _activeLevel +1;
        }
    }

    private void SetLevelDebug()
    {
        Level leveln;
        if (_activeScene == "Game1")
            leveln = new LevelGame1(0, new TrainingSettings(10, 11, false), new WinSettings(10), new[] { 0, 1, 10 }, 5, true);
        else if (_activeScene == "Game2")
            leveln = new LevelGame2(1, new TrainingSettings(10, 11, false), new WinSettings(5), (1, 4));
        else if (_activeScene == "Game3")
            leveln = new LevelGame3(new TrainingSettings(10, 11, false), new WinSettings(15, "ѕолучено:", false, "+ ¬етка добра."),
                new[] { (9, int.MinValue, 0), (1, 5, int.MinValue), (int.MinValue, 10, 70) }, Ingredients.Branch);
        else if (_activeScene == "Game4")
            leveln = new LevelGame4(new TrainingSettings(10, 11, true), new WinSettings(25), 6);
        else if (_activeScene == "Game5")
            leveln = new LevelGame5(1, new TrainingSettings(10, 11, false), new WinSettings(40), new List<int>() { 2, 3, 2, 3, 10, 10 }, (3, 2));
        else throw new NotImplementedException("ћини-игра не найдена.");

        SetTrainingBook(leveln.Training, out TrainingBook book);
        SetParams(leveln);
        win.Init();
        if (_realLastLevel == _activeLevel)
            win.SetParams(leveln.Win);
        else
            win.SetParams(0, "ћолодец!", true);
    }

    private void SetLevel()
    {
        var level = Map.Levels[_activeLevel];
        var lastLevel = Map.Levels[_lastLevel];
        SetTrainingBook(lastLevel.Training, out TrainingBook book);
        SetParams(level);
        win.Init();
        if(_realLastLevel == _activeLevel)
            win.SetParams(level.Win);
        else
            win.SetParams(0, "ћолодец!", true);
    }

    protected override void SetTrainingBook(TrainingSettings trainingSettings, out TrainingBook bookObj)
    {
        base.SetTrainingBook(trainingSettings, out TrainingBook book);

        book.ActivePage = trainingSettings.ActivePage;
        book.IsActiveOnLoad = trainingSettings.ActiveOnLoadScene;
        var lastTraining = LevelManager.LastTraining;
        if (lastTraining == _lastLevel)
            book.IsActiveOnLoad = false;
        LevelManager.LastTraining = _lastLevel;
        bookObj = book;
    }

    public override void Restart()
    {
        SceneManager.LoadScene(Map.Levels[_activeLevel].SceneName);
    }
}