using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BotonMarchaUI : MonoBehaviour
{
    [Header("Referencias del Sistema")]
    public MarchaController marchaController;
    public Button botonInteractuable;

    [Header("Componentes Visuales del Botón")]
    public TextMeshProUGUI textoBoton;
    public Image imageIcono;

    [Header("Sprites de Estado")]
    public Sprite iconoPlay;
    public Sprite iconoPausa;

    [Header("Configuración de Textos Médicos")]
    [SerializeField] private string textoIdle = "Iniciar Evaluación";
    [SerializeField] private string textoEvaluando = "Evaluando...";

    private bool estadoVisualInterno = false;

    void Start()
    {
        if (botonInteractuable == null)
            botonInteractuable = GetComponent<Button>();

        if (botonInteractuable != null)
            botonInteractuable.onClick.AddListener(AlPulsarBoton);

        SincronizarInterfaz(false);
    }

    void Update()
    {
        if (marchaController != null)
        {
            if (marchaController.IsEjecutando != estadoVisualInterno)
            {
                estadoVisualInterno = marchaController.IsEjecutando;
                SincronizarInterfaz(estadoVisualInterno);
            }
        }
    }

    private void AlPulsarBoton()
    {
        if (marchaController == null) return;

        if (!marchaController.IsEjecutando)
        {
            marchaController.IniciarMarcha();
        }
    }

    private void SincronizarInterfaz(bool estaAnimando)
    {
        if (estaAnimando)
        {
            textoBoton.text = textoEvaluando;
            if (iconoPausa != null) imageIcono.sprite = iconoPausa;

            textoBoton.color = new Color(0.35f, 1.0f, 0.35f);
        }
        else
        {
            textoBoton.text = textoIdle;
            if (iconoPlay != null) imageIcono.sprite = iconoPlay;

            textoBoton.color = Color.black;
        }
    }   
}