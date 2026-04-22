using UnityEngine;

public class ApplySettingsUI : MonoBehaviour
{
    SettingsManager settings;

    void Start()
    {
        settings = SettingsManager.Instance;
    }

    public void ApplySettings()
    {
        settings.SaveSettings();
        Debug.Log("Configuración guardada correctamente");
    }
}