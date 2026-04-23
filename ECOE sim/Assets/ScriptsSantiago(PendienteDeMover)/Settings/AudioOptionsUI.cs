using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioOptionsUI : MonoBehaviour
{
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    SettingsManager settings;

    void Start()
    {
        settings = SettingsManager.Instance;

        LoadToUI();

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    void LoadToUI()
    {
        masterSlider.value = settings.masterVolume;
        sfxSlider.value = settings.sfxVolume;
        musicSlider.value = settings.musicVolume;
    }

    // -------------------------
    // MASTER
    // -------------------------
    public void SetMasterVolume(float value)
    {
        settings.masterVolume = value;

        audioMixer.SetFloat("MasterVolume", LinearToDecibel(value));
    }

    // -------------------------
    // SFX
    // -------------------------
   public void SetSFXVolume(float value)
    {
        settings.sfxVolume = value;

        float dbValue = LinearToDecibel(value);
        audioMixer.SetFloat("SFXVolume", dbValue);

        Debug.Log("[AUDIO] SFX Volume → Linear: " + value + " | dB: " + dbValue);
    }

    // -------------------------
    // MUSIC
    // -------------------------
    public void SetMusicVolume(float value)
    {
        settings.musicVolume = value;

        float dbValue = LinearToDecibel(value);
        audioMixer.SetFloat("MusicVolume", dbValue);

        Debug.Log("[AUDIO] Music Volume → Linear: " + value + " | dB: " + dbValue);
    }

    // -------------------------
    // UTILIDAD: 0–1 → dB
    // -------------------------
    float LinearToDecibel(float value)
    {
        if (value <= 0.0001f)
            return -80f;

        return Mathf.Log10(value) * 20f;
    }
}