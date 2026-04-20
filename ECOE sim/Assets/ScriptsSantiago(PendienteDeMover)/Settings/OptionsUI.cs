using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [Header("Audio")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    [Header("Display")]
    public TMP_Dropdown screenDropdown;

    [Header("Gameplay")]
    public Toggle hudToggle;
    public Toggle controlsToggle;

    [Header("Panels")]
    public GameObject controlsPanel;

    void Start()
    {
        LoadToUI();
    }

    // -------------------------
    // CARGAR VALORES A LA UI
    // -------------------------
    void LoadToUI()
    {
        var s = SettingsManager.Instance;

        masterSlider.value = s.masterVolume;
        sfxSlider.value = s.sfxVolume;
        musicSlider.value = s.musicVolume;

        hudToggle.isOn = s.showHUD;
        controlsToggle.isOn = s.showControls;

        screenDropdown.value = GetScreenModeIndex(s.screenMode);

        controlsPanel.SetActive(s.showControls);
    }

    // -------------------------
    // AUDIO
    // -------------------------
    public void SetMasterVolume(float value)
    {
        SettingsManager.Instance.masterVolume = value;
    }

    public void SetSFXVolume(float value)
    {
        SettingsManager.Instance.sfxVolume = value;
    }

    public void SetMusicVolume(float value)
    {
        SettingsManager.Instance.musicVolume = value;
    }

    // -------------------------
    // PANTALLA
    // -------------------------
    public void SetScreenMode(int index)
    {
        FullScreenMode mode = FullScreenMode.FullScreenWindow;

        switch (index)
        {
            case 0:
                mode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                mode = FullScreenMode.Windowed;
                break;
            case 2:
                mode = FullScreenMode.FullScreenWindow;
                break;
        }

        SettingsManager.Instance.screenMode = mode;
        Screen.fullScreenMode = mode;
    }

    int GetScreenModeIndex(FullScreenMode mode)
    {
        switch (mode)
        {
            case FullScreenMode.ExclusiveFullScreen: return 0;
            case FullScreenMode.Windowed: return 1;
            case FullScreenMode.FullScreenWindow: return 2;
        }
        return 2;
    }

    // -------------------------
    // GAMEPLAY
    // -------------------------
    public void ToggleHUD(bool value)
    {
        SettingsManager.Instance.showHUD = value;

        GameObject hud = GameObject.FindWithTag("HUD");
        if (hud != null)
            hud.SetActive(value);
    }

    public void ToggleControls(bool value)
    {
        SettingsManager.Instance.showControls = value;
        controlsPanel.SetActive(value);
    }

    // -------------------------
    // GUARDAR (botón aplicar)
    // -------------------------
    public void ApplySettings()
    {
        SettingsManager.Instance.SaveSettings();
    }
}