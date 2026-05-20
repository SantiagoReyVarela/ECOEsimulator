using System.Collections.Generic;
using UnityEngine;

public class ValidadorRespuestas : MonoBehaviour
{
    public PreguntaUI patronUI;
    public PreguntaUI cuadroUI;
    public PreguntaUI manifestacionesUI;

    private CasoClinico casoActual;
    private bool ultimoResultado;

    public void Configurar(CasoClinico caso)
    {
        casoActual = caso;
    }

    public void Validar()
    {
        int puntuacion = 0;

        // -------------------
        // PATRON (4 pts)
        // -------------------
        bool patronCorrecto =
            patronUI.ObtenerSeleccionados().Count > 0 &&
            patronUI.ObtenerSeleccionados()[0] == casoActual.patronMarcha.correcta;

        if (patronCorrecto)
            puntuacion += 4;

        // -------------------
        // CUADRO (3 pts)
        // -------------------
        bool cuadroCorrecto =
            cuadroUI.ObtenerSeleccionados().Count > 0 &&
            cuadroUI.ObtenerSeleccionados()[0] == casoActual.cuadroPatologico.correcta;

        if (cuadroCorrecto)
            puntuacion += 3;

        // -------------------
        // MANIFESTACIONES (3 pts)
        // -------------------
        List<int> seleccionadas =
            manifestacionesUI.ObtenerSeleccionados();

        bool manifestacionesCorrectas =
            CompararListas(
                seleccionadas,
                casoActual.manifestacionesPropias.correctas
            );

        if (manifestacionesCorrectas)
            puntuacion += 3;

        // -------------------
        // RESULTADO FINAL
        // -------------------
        Debug.Log("PUNTUACIÓN FINAL: " + puntuacion + " / 10");
    }

    private bool CompararListas(List<int> a, List<int> b)
    {
        HashSet<int> setA = new(a);
        HashSet<int> setB = new(b);

        return setA.SetEquals(setB);
    }

    public bool EsCorrecto()
    {
        return ultimoResultado;
    }
}