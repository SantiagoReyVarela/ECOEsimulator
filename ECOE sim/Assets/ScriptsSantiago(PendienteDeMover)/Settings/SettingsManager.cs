using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Audio")]
    public float masterVolume = 1f;
    public float sfxVolume = 1f;
    public float musicVolume = 1f;

    [Header("Display")]
    public FullScreenMode screenMode = FullScreenMode.FullScreenWindow;

    [Header("Gameplay")]
    public bool showHUD = true;
    public bool showControls = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
        ApplySettings();
    }

    public void ApplySettings()
    {
        Screen.fullScreenMode = screenMode;

        GameObject hud = GameObject.FindWithTag("HUD");
        if (hud != null)
            hud.SetActive(showHUD);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        PlayerPrefs.SetInt("ShowHUD", showHUD ? 1 : 0);
        PlayerPrefs.SetInt("ShowControls", showControls ? 1 : 0);

        PlayerPrefs.SetInt("ScreenMode", (int)screenMode);

        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        showHUD = PlayerPrefs.GetInt("ShowHUD", 1) == 1;
        showControls = PlayerPrefs.GetInt("ShowControls", 0) == 1;

        screenMode = (FullScreenMode)PlayerPrefs.GetInt(
            "ScreenMode",
            (int)FullScreenMode.FullScreenWindow
        );
    }
}