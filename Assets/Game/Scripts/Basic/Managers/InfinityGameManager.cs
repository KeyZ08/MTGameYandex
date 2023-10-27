using Game;
using UnityJSON;
using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class InfinityGameManager : GameManagerSetParams
{
    protected override void Awake()
    {
        base.Awake();
        CreateUIs();
        SetLevel();
    }

    private void SetLevel()
    {
        var lastLevel = Map.Levels[_lastLevel];
        SetTrainingBook(lastLevel.Training, out TrainingBook book);
        win.Init();
        win.SetParams(0, "Молодец!", true);

        if (_activeScene == "Game1Infinity")
        {
            int[] usedNums = JSON.Deserialize<int[]>(LevelManager.InfinityGameSettings);
            if (usedNums == null) usedNums = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            SetParams(new LevelGame1(Random.Range(0, Backgrounds.Length), Map.Levels[_lastLevel].Training, new WinSettings(10), usedNums, int.MaxValue, false));
        }
        else if (_activeScene == "Game4Infinity")
        {
            SetParams(new LevelGame4(Map.Levels[_lastLevel].Training, new WinSettings(10), int.MaxValue));
        }
        else throw new NotImplementedException();
    }

    protected override void SetTrainingBook(TrainingSettings trainingSettings, out TrainingBook bookObj)
    {
        base.SetTrainingBook(trainingSettings, out TrainingBook book);

        book.ActivePage = trainingSettings.ActivePage;
        book.IsActiveOnLoad = false;
        bookObj = book;
    }

    public override void Restart()
    {
        SceneManager.LoadScene(_activeScene);
    }

    public override void NextLevel()
    {
        throw new NotImplementedException();
    }

    public override void SaveProgress()
    {
        throw new NotImplementedException();
    }
}
