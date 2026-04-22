using UnityEngine;
using UnityEngine.UI;

public class GameplayOptionsUI : MonoBehaviour
{
    public Toggle hudToggle;
    public Toggle controlsToggle;

    public GameObject hud;
    public GameObject controlsPanel;

    SettingsManager settings;

    void Start()
    {
        settings = SettingsManager.Instance;
        LoadToUI();
    }

    void LoadToUI()
    {
        hudToggle.isOn = settings.showHUD;
        controlsToggle.isOn = settings.showControls;

        if (hud != null)
            hud.SetActive(settings.showHUD);

        if (controlsPanel != null)
            controlsPanel.SetActive(settings.showControls);

        Debug.Log("Opciones de gameplay cargadas");
    }

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

        Debug.Log("Panel de controles " + (value ? "mostrado" : "ocultado"));
    }
}