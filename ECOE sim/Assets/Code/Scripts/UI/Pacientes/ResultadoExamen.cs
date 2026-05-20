using TMPro;
using UnityEngine;

public class ResultadoExamen : MonoBehaviour
{
    public GameObject panelResultado;

    public TMP_Text textoResultado;

    public ValidadorRespuestas validador;

    public void MostrarResultado()
    {
        panelResultado.SetActive(true);

        bool aprobado = validador.EsCorrecto();

        textoResultado.text =
            aprobado
            ? "EXAMEN APROBADO"
            : "EXAMEN SUSPENDIDO";
    }
}