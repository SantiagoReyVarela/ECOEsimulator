using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public GameObject hud; // 🔥 referencia directa

    SettingsManager settings;

    void Start()
    {
        settings = FindObjectOfType<SettingsManager>();

        if (settings == null)
        {
            Debug.LogError("SettingsManager no encontrado en la escena");
            return;
        }

        LoadToUI();
    }

    // -------------------------
    // CARGAR VALORES A LA UI
    // -------------------------
    void LoadToUI()
    {
        masterSlider.value = settings.masterVolume;
        sfxSlider.value = settings.sfxVolume;
        musicSlider.value = settings.musicVolume;

        hudToggle.isOn = settings.showHUD;
        controlsToggle.isOn = settings.showControls;

        screenDropdown.value = GetScreenModeIndex(settings.screenMode);

        controlsPanel.SetActive(settings.showControls);

        if (hud != null)
            hud.SetActive(settings.showHUD);
    }

    // -------------------------
    // AUDIO
    // -------------------------
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

        settings.screenMode = mode;
        Screen.fullScreenMode = mode;

        Debug.Log("Modo de pantalla cambiado a: " + mode);
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
        settings.showHUD = value;

        if (hud != null)
            hud.SetActive(value);

        Debug.Log("HUD " + (value ? "activado" : "desactivado"));
    }

    public void ToggleControls(bool value)
    {
        settings.showControls = value;

        if (controlsPanel != null)
            controlsPanel.SetActive(value);

        
    }

    public void ToggleObject(GameObject obj)
    {
        if (obj != null)
            obj.SetActive(!obj.activeSelf);
        
        Debug.Log("Panel de controles " + (value ? "mostrado" : "ocultado"));
    }

        // -------------------------
    // GUARDAR (botón aplicar)
    // -------------------------
    public void ApplySettings()
    {
        settings.SaveSettings();
        Debug.Log("Configuración guardada");
    }
}