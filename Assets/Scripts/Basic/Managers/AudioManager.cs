
using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [NonSerialized]
    public AudioMixer AudioMixer;

    public bool SoundIsPlayed
    {
        get
        {
            var s = PlayerPrefs.GetFloat("Sounds");
            return s != -80;
        }
    }

    public bool MusicIsPlayed
    {
        get
        {
            var s = PlayerPrefs.GetFloat("Music");
            return s != -80;
        }
    }


    private void Awake()
    {
        AudioMixer = Resources.Load<AudioMixer>("MainMixer");
    }

    private void Start()
    {
        var m = PlayerPrefs.GetFloat("Music");
        var s = PlayerPrefs.GetFloat("Sounds");
        MusicsSetVolume(m);
        SoundsSetVolume(s);
    }

    public void SoundsVolumeChange()
    {
        AudioMixer.GetFloat("Sounds", out float value);
        if (value == -80)
            SoundsSetVolume(0);
        else
            SoundsSetVolume(-80);
        AudioMixer.GetFloat("Sounds", out float sounds);
        PlayerPrefs.SetFloat("Sounds", sounds);
    }

    public void MusicVolumeChange()
    {
        AudioMixer.GetFloat("Music", out float value);
        if (value == -80)
            MusicsSetVolume(0);
        else
            MusicsSetVolume(-80);
        AudioMixer.GetFloat("Music", out float music);
        PlayerPrefs.SetFloat("Music", music);
    }

    public void ScaledSoundsChange(float volume)
    {
        AudioMixer.SetFloat("Scaled", volume);
    }

    private void SoundsSetVolume(float volume)
    {
        AudioMixer.SetFloat("Sounds", volume);
    }


    private void MusicsSetVolume(float volume)
    {
        AudioMixer.SetFloat("Music", volume);
    }

    public void MasterVolumeChange(float volume)
    {
        AudioMixer.SetFloat("Master", volume);
    }
}
