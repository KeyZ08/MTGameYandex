using Game;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractiveTraining : GameManagerSetParams
{
    protected override void Awake()
    {
        base.Awake();
        CreateUIs();
        SetLevel();
    }

    public override void NextLevel()
    {
        SceneManager.LoadScene(Map.Levels[_activeLevel].SceneName);
    }

    private void SetLevel()
    {
        Debug.Log("Загружен уровень " + _activeLevel);
        
        /*Это для отладки, чтобы ничего не ломалось пока*/
        if (_activeScene != Map.Levels[_activeLevel].SceneName)
        {
            SetLevelDebug();
            return;
        }

        Level leveln;
        if (InteractiveTrainingManager.GetScene(_activeLevel) == "Game5Learning")
            leveln = new LevelGame5(1, new TrainingSettings(10, 11, false), new WinSettings(0), new List<int>() { 2, 3, 1 }, (4, 2));
        else throw new NotImplementedException("Интерактивное обучение не надено.");

        SetTrainingBook(leveln.Training, out TrainingBook book);
        SetParams(leveln);
        win.Init();
        win.SetParams(0, "Молодец!", true);
    }

    private void SetLevelDebug()
    {
        Level leveln;
        if (_activeScene == "Game5Learning")
            leveln = new LevelGame5(1, new TrainingSettings(4, 6, true), new WinSettings(0), new List<int>() { 2, 3, 1 }, (4, 2));
        else if (_activeScene == "Game4Learning")
            leveln = new LevelGame4(new TrainingSettings(2, 1, true), new WinSettings(0), 3);
        else throw new NotImplementedException("Интерактивное обучение не найдено.");

        SetTrainingBook(leveln.Training, out TrainingBook book);
        SetParams(leveln);
        win.Init();
        win.SetParams(0, "Молодец!", true);
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
        SceneManager.LoadScene(InteractiveTrainingManager.GetScene(_activeLevel));
    }

    public override void SaveProgress()
    {
        LevelManager.LastInteractiveTraining = _activeLevel;
    }
}
