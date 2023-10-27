using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TrainingBook : MonoBehaviour
{
    public Button BtnNext;
    public Button BtnBack;
    public List<GameObject> Pages;

    public AudioSource paging;
    public AudioMixer AudioMixer;

    public bool IsActiveOnLoad;

    private GameObject _activePage;
    private Pause _pause;

    public int ActivePage
    {
        get { return Pages.IndexOf(_activePage); }
        set
        {
            if (value > Pages.Count || value < 0) 
                throw new IndexOutOfRangeException();
            if (value == Pages.Count)
            {
                Close();
                return;
            }
            if (value == 0)
                BtnBack.gameObject.SetActive(false);
            else  
                BtnBack.gameObject.SetActive(true);

            if (_activePage != null)
                _activePage.SetActive(false);
            _activePage = Pages[value];
            _activePage.SetActive(true);
            paging.Play();
        }
    }


    private void Start()
    {
        _pause = FindAnyObjectByType<Pause>(FindObjectsInactive.Include);
        BtnNext.onClick.AddListener(NextPage);
        BtnBack.onClick.AddListener(BackPage);
        for (int i = 0; i < Pages.Count; i++)
            Pages[i].SetActive(false);
        ActivePage = ActivePage;

        Close();
        if (IsActiveOnLoad)
        {
            Open();
        }
    }

    private void NextPage()
    {
        ActivePage += 1;
    }

    private void BackPage()
    {
        ActivePage -= 1;
    }

    public void Close()
    {
        if (_pause != null && !_pause.IsOpen)
        {
            Time.timeScale = 1;
            AudioMixer.SetFloat("Scaled", 0);
        }
        gameObject.SetActive(false);
    }

    public void Open()
    {
        if (_pause != null && !_pause.IsOpen)
        {
            Time.timeScale = 0;
            AudioMixer.SetFloat("Scaled", -80);
        }
        gameObject.SetActive(true);
    }
}
