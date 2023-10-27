using Game;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    private int _activeLevel;

    private void Awake()
    {
        _activeLevel = LevelManager.ActiveLevel;
    }

    public void Next()
    {
        var interactiveScene = InteractiveTrainingManager.GetScene(_activeLevel);
        if (interactiveScene != null && LevelManager.LastInteractiveTraining < _activeLevel)
        {
            SceneManager.LoadScene(interactiveScene);
            return;
        }
        SceneManager.LoadScene(Map.Levels[_activeLevel].SceneName);
    }

    public void OpenMap()
    {
        SceneManager.LoadScene("Map");
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
