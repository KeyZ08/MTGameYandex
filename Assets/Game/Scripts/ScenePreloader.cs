using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScenePreloader
{
    private static AsyncOperation async;

    public static void SceneLoad(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
    }

    public static void SceneActivation()
    {
        async.allowSceneActivation = true;
    }
}