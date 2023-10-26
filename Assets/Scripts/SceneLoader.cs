using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadMap()
    {
        SceneManager.LoadScene("Map");
    }

    public void LoadHome()
    {
        SceneManager.LoadScene("Shop");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
