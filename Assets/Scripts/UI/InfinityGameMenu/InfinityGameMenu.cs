using UnityEngine;
using UnityEngine.UI;
using System;
using UnityJSON;
using UnityEngine.SceneManagement;

public class InfinityGameMenu : MonoBehaviour
{
    [Header("Modes")]
    public InfinityGameMode ModeGame1;    
    public InfinityGameMode ModeGame4;
    public Button BtnPlay;

    private InfinityGameMode[] modes;
    private InfinityGameMode _activeMode;

    private void Awake()
    {
        BtnPlay.onClick.AddListener(Play);
    }

    private void Start()
    {
        modes = new[] { ModeGame1, ModeGame4 };
        for (var i = 0; i < modes.Length; i++)
        {
            modes[i].Selected = false;
        }
        CheckAndSetMode();
    }

    public void StateUpdate(InfinityGameMode mode)
    {
        if (mode.Selected)
        {
            for (var i = 0; i < modes.Length; i++) 
                modes[i].Selected = false;
            mode.Selected = true;
        }
        CheckAndSetMode();
    }

    private void CheckAndSetMode()
    {
        InfinityGameMode mode = null;
        for (var i = 0; i < modes.Length; i++)
        {
            if (modes[i].Selected && modes[i].IsValid)
            {
                mode = modes[i];
                break;
            }
        }
        
        if(mode != null)
        {
            _activeMode = mode;
            BtnPlay.interactable = true;
        }
        else
        {
            _activeMode = null;
            BtnPlay.interactable = false;
        }
    }

    private void Play()
    {
        if (_activeMode == null || !_activeMode.IsValid) return;
        if (_activeMode is InfinityGame1)
        {
            var settings = (_activeMode as InfinityGame1).GetSettings();
            LevelManager.InfinityGameSettings = JSON.Serialize(settings);
            SceneManager.LoadScene(_activeMode.scene);
        }
        else if (_activeMode is InfinityGame4)
            SceneManager.LoadScene(_activeMode.scene);
        else throw new NotImplementedException();
    }
}

