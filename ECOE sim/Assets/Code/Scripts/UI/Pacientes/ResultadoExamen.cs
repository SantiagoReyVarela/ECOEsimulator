using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultadoExamen : MonoBehaviour
{
    [Header("Paneles y Referencias")]
    public GameObject panelResultado;
    public TMP_Text textoResultado;
    public ValidadorRespuestas validador;

    [Header("Colores para el Papel Blanco (Configura en el Inspector)")]
    public Color colorTextoPrincipal = Color.black;
    public Color colorLineasSeparadoras = Color.gray;
    public Color colorAprobado = Color.blue;
    public Color colorSuspenso = Color.red;
    public Color colorAcierto = Color.green;
    public Color colorError = Color.red;

    public void MostrarResultado()
    {
        // Activamos el folio blanco de resultados
        panelResultado.SetActive(true);

        // Le decimos al otro script que calcule la nota
        validador.Validar();

        bool aprobado = validador.EsCorrecto();
        int nota = validador.PuntuacionFinal;

        // Convertimos los colores del inspector a texto para que TextMeshPro los entienda
        string hexPrincipal = "#" + ColorUtility.ToHtmlStringRGB(colorTextoPrincipal);
        string hexLineas = "#" + ColorUtility.ToHtmlStringRGB(colorLineasSeparadoras);
        string hexAprobado = "#" + ColorUtility.ToHtmlStringRGB(colorAprobado);
        string hexSuspenso = "#" + ColorUtility.ToHtmlStringRGB(colorSuspenso);
        string hexAcierto = "#" + ColorUtility.ToHtmlStringRGB(colorAcierto);
        string hexError = "#" + ColorUtility.ToHtmlStringRGB(colorError);

        // Empezamos a rellenar el folio de papel vacío
        string folio = "";

        // 1. TÍTULO Y CABECERA
        folio += $"<align=center><size=80%><color={hexLineas}>EVALUACIÓN ECOE</color></size></align>\n";
        folio += $"<color={hexLineas}>_____________________________</color>\n\n";

        // 2. DESGLOSE DE PUNTOS (Ponemos el texto principal oscuro para que se vea bien en blanco)
        folio += $"<color={hexPrincipal}><b>INFORME DETALLADO:</b>\n\n";

        // Apartado Marcha
        if (validador.PatronCorrecto)
            folio += $" Análisis de Patrón de Marcha: <color={hexAcierto}><b>\n APTO</b></color> (+4.0 pts)\n";
        else
            folio += $"    Análisis de Patrón de Marcha: <color={hexError}><b>\n  NO APTO</b></color> (+0.0 pts)\n";

        // Apartado Cuadro Patológico
        if (validador.CuadroCorrecto)
            folio += $"    Diagnóstico del Cuadro Patológico: <color={hexAcierto}><b>\n APTO</b></color> (+3.0 pts)\n";
        else
            folio += $"    Diagnóstico del Cuadro Patológico: <color={hexError}><b>\n  NO APTO</b></color> (+0.0 pts)\n";

        // Apartado Síntomas
        if (validador.ManifestacionesCorrectas)
            folio += $"    Identificación de Síntomas: <color={hexAcierto}><b>\n APTO</b></color> (+3.0 pts)\n";
        else
            folio += $"    Identificación de Síntomas: <color={hexError}><b>\n  NO APTO</b></color> (+0.0 pts)\n";

        folio += "</color>"; // Cerramos el color principal

        folio += $"\n<color={hexLineas}>_____________________________</color>\n\n";

        // 3. NOTA FINAL Y DIÁLOGO DE VEREDICTO
        if (aprobado)
        {
            folio += $"<align=center><size=140%><color={hexAprobado}><b>CALIFICACIÓN: APTO </b></color></size></align>\n";
            folio += $"<align=center><size=160%><color={hexAprobado}><b>NOTA FINAL: {nota}.0 / 10</b></color></size></align>\n\n";
            folio += $"<align=center><size=85%><color={hexPrincipal}>El alumno demuestra las competencias clínicas mínimas requeridas.</color></size></align>";
        }
        else
        {
            folio += $"<align=center><size=140%><color={hexSuspenso}><b>CALIFICACIÓN: NO APTO  </b></color></size></align>\n";
            folio += $"<align=center><size=160%><color={hexSuspenso}><b>NOTA FINAL: {nota}.0 / 10</b></color></size></align>\n\n";
            folio += $"<align=center><size=85%><color={hexPrincipal}>Se recomienda revisar el manual técnico de la consulta médica.</color></size></align>";
        }

        // Imprimimos todo el texto acumulado en la pantalla de Unity
        textoResultado.text = folio;
    }
}