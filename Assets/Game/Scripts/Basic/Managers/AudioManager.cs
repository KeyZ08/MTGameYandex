
using UnityEngine.Audio;
using UnityEngine;
using System;
using YG;

public class AudioManager : MonoBehaviour
{
    [NonSerialized]
    public AudioMixer AudioMixer;

    public bool SoundIsPlayed
    {
        get
        {
            var s = YandexGame.savesData.Sounds;
            return s != -80;
        }
    }

    public bool MusicIsPlayed
    {
        get
        {
            var s = YandexGame.savesData.Music;
            return s != -80;
        }
    }


    private void Awake()
    {
        AudioMixer = Resources.Load<AudioMixer>("MainMixer");
    }

    private void Start()
    {
        var m = YandexGame.savesData.Music;
        var s = YandexGame.savesData.Sounds;
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
        YandexGame.savesData.Sounds = sounds;
        YandexGame.SaveProgress();
    }

    public void MusicVolumeChange()
    {
        AudioMixer.GetFloat("Music", out float value);
        if (value == -80)
            MusicsSetVolume(0);
        else
            MusicsSetVolume(-80);
        AudioMixer.GetFloat("Music", out float music);
        YandexGame.savesData.Music = music;
        YandexGame.SaveProgress();
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
