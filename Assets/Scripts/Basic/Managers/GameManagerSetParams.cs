using Game;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class GameManagerSetParams : LevelUIs
{
    [Header("Prefabs")]
    public GameObject[] Backgrounds;
    public GameObject[] AllTrainingBooks;

    protected int _lastLevel;
    protected int _realLastLevel;
    protected int _activeLevel;

    protected virtual void Awake()
    {
        _realLastLevel = LevelManager.LastLevel;
        _lastLevel = _realLastLevel;
        _activeLevel = LevelManager.ActiveLevel;
        _activeScene = SceneManager.GetActiveScene().name;
        Debug.Log("Последний уровень: " + _realLastLevel);
        if (_lastLevel == Map.Levels.Count)
            _lastLevel -= 1;
    }

    protected virtual void SetParams(Level level)
    {
        if (level is LevelGame1)
            SetParams(level as LevelGame1);
        else if (level is LevelGame2)
            SetParams(level as LevelGame2);
        else if (level is LevelGame3)
            SetParams(level as LevelGame3);
        else if (level is LevelGame4)
            SetParams(level as LevelGame4);
        else if (level is LevelGame5)
            SetParams(level as LevelGame5);
        else throw new ArgumentException("Неизвестный уровень");
    }

    protected virtual void SetParams(LevelGame1 l)
    {
        var manager = FindAnyObjectByType<ManagerGame1>();
        manager.SetParams(l.UsedNums, l.ExamplesCount, l.NullInResult);
        var canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        Instantiate(Backgrounds[l.BackgroundImage], canvas.transform);
    }

    protected virtual void SetParams(LevelGame2 l)
    {
        var manager = FindAnyObjectByType<ManagerGame2>();
        manager.SetParams(l.Example);
        var canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        Instantiate(Backgrounds[l.BackgroundImage], canvas.transform);
    }

    protected virtual void SetParams(LevelGame3 l)
    {
        var manager = FindAnyObjectByType<ManagerGame3>();
        manager.SetParams(l.Examples, l.Ingredient);
    }

    protected virtual void SetParams(LevelGame4 l)
    {
        var manager = FindAnyObjectByType<ManagerGame4>();
        manager.SetParams(l.EnemyCount);
    }

    protected virtual void SetParams(LevelGame5 l)
    {
        var manager = FindAnyObjectByType<ManagerGame5>();
        manager.SetParams(l.Choices, l.Bags);
        var canvas = GameObject.FindGameObjectWithTag("MainCanvas");
        Instantiate(Backgrounds[l.BackgroundImage], canvas.transform);
    }

    protected virtual void SetTrainingBook(TrainingSettings trainingSettings, out TrainingBook book)
    {
        if (trainingSettings.TrainingBook > AllTrainingBooks.Length - 1)
            throw new IndexOutOfRangeException("trainingSettings.TrainingBook");
        var bookPrefab = AllTrainingBooks[trainingSettings.TrainingBook];
        if (trainingSettings.ActivePage > bookPrefab.GetComponent<TrainingBook>().Pages.Count - 1 || trainingSettings.ActivePage < 0)
            throw new IndexOutOfRangeException("TrainingSettings.ActivePage");

        var bookObj = Instantiate(bookPrefab);
        bookObj.SetActive(true);
        bookObj.GetComponent<Canvas>().worldCamera = Camera.main;
        book = bookObj.GetComponent<TrainingBook>();
    }
}
