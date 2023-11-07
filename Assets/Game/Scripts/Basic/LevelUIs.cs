using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class LevelUIs : MonoBehaviour
{
    protected GameObject WinPrefab;
    protected GameObject LosePrefab;
    protected GameObject PausePrefab;

    protected Win win;
    protected Lose lose;
    protected Pause pause;

    protected string _activeScene;

    public virtual void Win()
    {
        win.Show();
    }

    public virtual Lose Lose(bool resetLoseBtnActive)
    {
        lose.Show(resetLoseBtnActive);
        return lose;
    }

    public virtual void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public abstract void Restart();
    public abstract void NextLevel();
    public abstract void SaveProgress();

    protected virtual void CreateUIs()
    {
        WinPrefab = Resources.Load<GameObject>("Win");
        LosePrefab = Resources.Load<GameObject>("Lose");
        PausePrefab = Resources.Load<GameObject>("Pause");

        var winObj = Instantiate(WinPrefab);
        var loseObj = Instantiate(LosePrefab);
        var pauseObj = Instantiate(PausePrefab);
        var cam = Camera.main;
        winObj.GetComponent<Canvas>().worldCamera = cam;
        loseObj.GetComponent<Canvas>().worldCamera = cam;
        pauseObj.GetComponent<Canvas>().worldCamera = cam;

        win = winObj.GetComponent<Win>();
        lose = loseObj.GetComponent<Lose>();
        pause = pauseObj.GetComponent<Pause>();

        win.gameObject.SetActive(false);
        lose.gameObject.SetActive(false);
        pause.gameObject.SetActive(true);
    }
}
