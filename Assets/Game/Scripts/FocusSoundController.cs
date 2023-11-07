using UnityEngine;

public class FocusSoundController : MonoBehaviour
{
    private AudioManager audioManager;

    private void OnEnable()
    {
        audioManager = GetComponent<AudioManager>();
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Silence(!hasFocus);
    }

    void OnApplicationPause(bool isPaused)
    {
        Silence(isPaused);
    }

    private void Silence(bool isPaused)
    {
        var pause = FindAnyObjectByType<Pause>()?.IsOpen;
        if (pause.GetValueOrDefault())
            audioManager.MasterVolumeChange(isPaused ? -80 : 0);
        else
        {
            Time.timeScale = isPaused ? 0 : 1;
            audioManager.MasterVolumeChange(isPaused ? -80 : 0);
        }
    }
}