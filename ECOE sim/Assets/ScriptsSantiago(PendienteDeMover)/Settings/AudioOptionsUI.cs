using UnityEngine;
using UnityEngine.UI;

public class AudioOptionsUI : MonoBehaviour
{
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    SettingsManager settings;

    void Start()
    {
        settings = SettingsManager.Instance;
        LoadToUI();
    }

    void LoadToUI()
    {
        masterSlider.value = settings.masterVolume;
        sfxSlider.value = settings.sfxVolume;
        musicSlider.value = settings.musicVolume;

        Debug.Log("Audio cargado en UI");
    }

    public void SetMasterVolume(float value)
    {
        settings.masterVolume = value;
        Debug.Log("Master Volume cambiado a: " + value);
    }

    public void SetSFXVolume(float value)
    {
        settings.sfxVolume = value;
        Debug.Log("SFX Volume cambiado a: " + value);
    }

    public void SetMusicVolume(float value)
    {
        settings.musicVolume = value;
        Debug.Log("Music Volume cambiado a: " + value);
    }
}