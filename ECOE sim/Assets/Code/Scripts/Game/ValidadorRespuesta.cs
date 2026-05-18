using System.Collections.Generic;
using UnityEngine;

public class ValidadorRespuestas : MonoBehaviour
{
    public PreguntaUI patronUI;
    public PreguntaUI cuadroUI;
    public PreguntaUI manifestacionesUI;

    private CasoClinico casoActual;

    public void Configurar(CasoClinico caso)
    {
        casoActual = caso;
    }

    public void Validar()
    {
        var patronSel = patronUI.ObtenerSeleccionados();
        var cuadroSel = cuadroUI.ObtenerSeleccionados();
        var maniSel = manifestacionesUI.ObtenerSeleccionados();

        if (patronSel.Count == 0 || cuadroSel.Count == 0)
        {
            Debug.Log("Faltan respuestas en preguntas simples");
            return;
        }

        bool patronCorrecto =
            patronSel[0] == casoActual.patronMarcha.correcta;

        bool cuadroCorrecto =
            cuadroSel[0] == casoActual.cuadroPatologico.correcta;

        bool manifestacionesCorrectas =
            CompararListas(maniSel, casoActual.manifestacionesPropias.correctas);

        Debug.Log(
            (patronCorrecto && cuadroCorrecto && manifestacionesCorrectas)
            ? "CORRECTO"
            : "INCORRECTO"
        );
    }

    private bool CompararListas(List<int> a, List<int> b)
    {
        HashSet<int> setA = new(a);
        HashSet<int> setB = new(b);

        return setA.SetEquals(setB);
    }
}