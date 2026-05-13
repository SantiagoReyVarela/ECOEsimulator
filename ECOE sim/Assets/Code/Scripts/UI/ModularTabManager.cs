using UnityEngine;
using UnityEngine.UI;

public class ModularTabManager : MonoBehaviour
{
    [Header("Componente Visual Principal")]
    [Tooltip("La imagen grande del cuerpo de la carpeta que cambiará de textura")]
    public Image folderBodyImage;

    [Header("Configuración de Pestañas")]
    [Tooltip("Añade aquí todas las pestañas que necesites")]
    public TabData[] tabs; // Array modular que aparecerá en el Inspector

    private void Start()
    {
        // 1. Conectar los botones automáticamente por código
        for (int i = 0; i < tabs.Length; i++)
        {
            int index = i; // Guardamos el índice actual para evitar problemas de memoria

            // Le decimos al botón que, cuando lo pulsen, ejecute SwitchTab con su número
            tabs[i].tabButton.onClick.AddListener(() => SwitchTab(index));
        }

        // 2. Iniciar activando solo la primera pestaña (índice 0)
        if (tabs.Length > 0)
        {
            SwitchTab(0);
        }
    }

    // Función universal para cambiar de pestaña
    private void SwitchTab(int selectedIndex)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            // ¿Es esta la pestaña que acabamos de pulsar?
            bool isSelected = (i == selectedIndex);

            // Encendemos su contenido si está seleccionada, lo apagamos si no
            tabs[i].contentPanel.SetActive(isSelected);

            // Si es la seleccionada, le ponemos su textura a la carpeta
            if (isSelected)
            {
                folderBodyImage.sprite = tabs[i].folderSprite;
            }

            // Opcional (pero muy pro): Desactivar la interacción del botón si ya estás en esa pestaña
            tabs[i].tabButton.interactable = !isSelected;
        }
    }
}

// -----------------------------------------------------------------
// Esta clase crea un "bloque" personalizado en el Inspector de Unity
// -----------------------------------------------------------------
[System.Serializable]
public class TabData
{
    public string tabName = "Nueva Pestaña"; // Solo para que lo veáis bien en el Inspector
    public Button tabButton;                 // El botoncito de arriba
    public Sprite folderSprite;              // La textura grande del cuerpo para esta pestaña
    public GameObject contentPanel;          // El GameObject con el contenido (Sliders, texto...)
}