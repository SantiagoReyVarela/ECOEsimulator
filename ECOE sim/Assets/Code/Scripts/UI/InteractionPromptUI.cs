using UnityEngine;
using TMPro;                     // si usas TextMeshPro
// using UnityEngine.UI;         // si usas Text clásico

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance { get; private set; }

    [SerializeField] private GameObject promptContainer;     // el GameObject que contiene el texto/fondo
    [SerializeField] private TMP_Text promptText;            // o Text si no usas TMP

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);   ← solo si quieres que persista entre escenas
    }

    public void Show(string message)
    {
        if (promptContainer == null)
        {
            Debug.LogError("InteractionPromptUI → promptContainer es NULL");
            return;
        }

        if (promptText == null)
        {
            Debug.LogError("InteractionPromptUI → promptText es NULL");
            return;
        }

        Debug.Log($"Mostrando interfaz: {message}");
        promptText.text = message;
        promptContainer.SetActive(true);
    }

    public void Hide()
    {
        if (promptContainer == null) return;
        Debug.Log("Ocultando interfaz");
        promptContainer.SetActive(false);
    }
}