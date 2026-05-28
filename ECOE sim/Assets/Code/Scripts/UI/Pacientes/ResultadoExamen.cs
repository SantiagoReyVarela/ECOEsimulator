using System.Text;
using TMPro;
using UnityEngine;

public class ResultadoExamen : MonoBehaviour
{
    public GameObject panelResultado;
    public TMP_Text textoResultado;
    public ValidadorRespuestas validador;

    [Header("Configuración de Colores (Paleta Sanitaria)")]
    [SerializeField] private string colorAprobado = "#1E4620";   // Verde institucional oscuro
    [SerializeField] private string colorSuspenso = "#C43B3B";   // Rojo Coral de alerta
    [SerializeField] private string colorAcierto = "#2E7D32";    // Verde para desgloses
    [SerializeField] private string colorError = "#C62828";      // Rojo para desgloses
    [SerializeField] private string colorFondoBanner = "#F4F4F5"; // Gris claro para destacar textos

    public void MostrarResultado()
    {
        panelResultado.SetActive(true);

        // Forzamos la validación de los datos en el validador
        validador.Validar();

        bool aprobado = validador.EsCorrecto();
        int nota = validador.PuntuacionFinal;

        // Construimos el documento usando secuencias de escape Unicode seguras
        StringBuilder documento = new StringBuilder();

        // 1. ENCABEZADO INSTITUCIONAL DE LA REY JUAN CARLOS
        documento.AppendLine("<align=center><size=110%><b>UNIVERSIDAD REY JUAN CARLOS</b></size></align>");
        documento.AppendLine("<align=center><size=80%><color=#666666>FACULTAD DE CIENCIAS DE LA SALUD \u2014 EVALUACIÓN ECOE</color></size></align>");
        documento.AppendLine("<voffset=0.5em> </voffset>");
        documento.AppendLine("<color=#cccccc>__________________________________________________</color>");
        documento.AppendLine("<voffset=0.5em> </voffset>");

        // 2. DETALLES DE LA ESTACIÓN CLÍNICA
        documento.AppendLine("<b>INFORME TÉCNICO DETALLADO:</b>");
        documento.AppendLine("<size=90%><color=#333333>");

        // Desglose 1: Patrón de Marcha (\u2714 es el check verde, \u2718 es el aspa roja)
        string estadoPatron = validador.PatronCorrecto
            ? $"<color={colorAcierto}><b>\u2714 APTO</b></color> <color=#666666>(+4.0 pts)</color>"
            : $"<color={colorError}><b>\u2718 NO APTO</b></color> <color=#666666>(+0.0 pts)</color>";
        documento.AppendLine($"  \u2022 Análisis de Patrón de Marcha: {estadoPatron}");

        // Desglose 2: Cuadro Patológico
        string estadoCuadro = validador.CuadroCorrecto
            ? $"<color={colorAcierto}><b>\u2714 APTO</b></color> <color=#666666>(+3.0 pts)</color>"
            : $"<color={colorError}><b>\u2718 NO APTO</b></color> <color=#666666>(+0.0 pts)</color>";
        documento.AppendLine($"  \u2022 Diagnóstico del Cuadro Patológico: {estadoCuadro}");

        // Desglose 3: Manifestaciones
        string estadoManifestaciones = validador.ManifestacionesCorrectas
            ? $"<color={colorAcierto}><b>\u2714 APTO</b></color> <color=#666666>(+3.0 pts)</color>"
            : $"<color={colorError}><b>\u2718 NO APTO</b></color> <color=#666666>(+0.0 pts)</color>";
        documento.AppendLine($"  \u2022 Identificación de Síntomas: {estadoManifestaciones}");
        documento.AppendLine("</color></size>");

        documento.AppendLine("<color=#cccccc>__________________________________________________</color>");
        documento.AppendLine("<voffset=1em> </voffset>");

        // 3. CALIFICACIÓN GLOBAL Y VEREDICTO (Formato de sello/tarjeta)
        if (aprobado)
        {
            documento.AppendLine($"<align=center><size=130%><color={colorAprobado}><b>CALIFICACIÓN: APTO (\u2714)</b></color></size></align>");
            documento.AppendLine($"<align=center><size=150%><color={colorAprobado}><b>NOTA FINAL: {nota}.0 / 10</b></color></size></align>");
            documento.AppendLine("<voffset=0.5em> </voffset>");
            documento.AppendLine("<align=center><size=80%><color=#555555>El candidato demuestra las competencias clínicas requeridas para superar esta estación.</color></size></align>");
        }
        else
        {
            documento.AppendLine($"<align=center><size=130%><color={colorSuspenso}><b>CALIFICACIÓN: NO APTO (\u2718)</b></color></size></align>");
            documento.AppendLine($"<align=center><size=150%><color={colorSuspenso}><b>NOTA FINAL: {nota}.0 / 10</b></color></size></align>");
            documento.AppendLine("<voffset=0.5em> </voffset>");
            documento.AppendLine("<align=center><size=80%><color=#555555>Se recomienda revisar el manual de procedimiento y la guía de diagnóstico diferencial de la consulta.</color></size></align>");
        }

        // Asignamos el flujo de texto formateado al TextMeshPro de la UI
        textoResultado.text = documento.ToString();
    }
}