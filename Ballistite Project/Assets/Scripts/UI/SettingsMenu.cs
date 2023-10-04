using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Button TutorialFreezeButton;

    private float masterValue;
    private float musicValue;
    private float sfxValue;
    private FreezeDisabler freezeDisablerScript;

    private void Awake()
    {
        SetSliderValue("VolumeOfMaster", masterValue, volumeSliderMaster);
        SetSliderValue("VolumeOfMusic", musicValue, volumeSliderMusic);
        SetSliderValue("VolumeOfSFX", sfxValue, volumeSliderSFX);
    }

    private void Start()
    {
        if (GameObject.Find("TutorialFreezeController"))
            freezeDisablerScript = GameObject.Find("TutorialFreezeController").GetComponent<FreezeDisabler>();

        if (freezeDisablerScript != null)
        {
            setFreezeButton(freezeDisablerScript.masterFreezeEnabled);
        }
        else
        {
            TutorialFreezeButton.image.color = new Color(0.5f, 0, 0);
            TutorialFreezeButton.GetComponentInChildren<TMP_Text>().text = "Err";
        }
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

    public void onToggleFreeze()
    {
        if (freezeDisablerScript != null)
        {
            freezeDisablerScript.masterFreezeEnabled = !freezeDisablerScript.masterFreezeEnabled;
            setFreezeButton(freezeDisablerScript.masterFreezeEnabled);
        }

    }

    private void setFreezeButton(bool b)
    {
        if (TutorialFreezeButton != null)
        {
            if (b)
            {
                TutorialFreezeButton.image.color = new Color(0.75f, 1, 1);
                TutorialFreezeButton.GetComponentInChildren<TMP_Text>().text = "On";
            }
            else
            {
                TutorialFreezeButton.image.color = new Color(0.6f, 0.6f, 0.6f);
                TutorialFreezeButton.GetComponentInChildren<TMP_Text>().text = "Off";
            }
        }
    }
}