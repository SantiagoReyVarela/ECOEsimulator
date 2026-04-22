using UnityEngine;
using TMPro;

public class DisplayOptionsUI : MonoBehaviour
{
    public TMP_Dropdown screenDropdown;

    SettingsManager settings;

    void Start()
    {
        settings = SettingsManager.Instance;
        LoadToUI();
    }

    void LoadToUI()
    {
        screenDropdown.value = GetScreenModeIndex(settings.screenMode);
        Debug.Log("Modo de pantalla cargado: " + settings.screenMode);
    }

    public void SetScreenMode(int index)
    {
        FullScreenMode mode = FullScreenMode.FullScreenWindow;

        switch (index)
        {
            case 0: mode = FullScreenMode.ExclusiveFullScreen; break;
            case 1: mode = FullScreenMode.Windowed; break;
            case 2: mode = FullScreenMode.FullScreenWindow; break;
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
}