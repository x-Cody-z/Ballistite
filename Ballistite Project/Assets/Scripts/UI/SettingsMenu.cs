using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSliderMaster;
    public Slider volumeSliderMusic;
    public Slider volumeSliderSFX;
    private float masterValue;
    private float musicValue;
    private float sfxValue;

    private void Awake()
    {
        SetSliderValue("VolumeOfMaster", masterValue, volumeSliderMaster);
        SetSliderValue("VolumeOfMusic", musicValue, volumeSliderMusic);
        SetSliderValue("VolumeOfSFX", sfxValue, volumeSliderSFX);
    }

    // Called when we click the "Return" button.
    public void OnReturnButton()
    {
        gameObject.SetActive(false);
    }

    public void OnSettingsButton()
    {
        gameObject.SetActive(true);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("VolumeOfMaster", volume);
    }
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("VolumeOfMusic", volume);
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("VolumeOfSFX", volume);
    }

    private void SetSliderValue(string name, float value, Slider slider)
    {
        if (audioMixer.GetFloat(name, out value))
        {
            slider.value = value;
        }
    }
}