using System.Collections.Generic;
using UnityEngine;

public class ValidadorRespuestas : MonoBehaviour
{
    public PreguntaUI patronUI;

    public PreguntaUI cuadroUI;

    public PreguntaUI manifestacionesUI;

    CasoClinico casoActual;

    public void Configurar(CasoClinico caso)
    {
        casoActual = caso;
    }

    public void Validar()
    {
        bool patronCorrecto =
            patronUI.ObtenerSeleccionados()[0]
            == casoActual.patronMarcha.correcta;

        bool cuadroCorrecto =
            cuadroUI.ObtenerSeleccionados()[0]
            == casoActual.cuadroPatologico.correcta;

        List<int> seleccionadas =
            manifestacionesUI.ObtenerSeleccionados();

        bool manifestacionesCorrectas =
            CompararListas(
                seleccionadas,
                casoActual.manifestacionesPropias.correctas);

        Debug.Log(
            patronCorrecto &&
            cuadroCorrecto &&
            manifestacionesCorrectas
            ? "CORRECTO"
            : "INCORRECTO");
    }

    bool CompararListas(List<int> a, List<int> b)
    {
        if (a.Count != b.Count)
            return false;

        foreach (int n in b)
        {
            if (!a.Contains(n))
                return false;
        }

        return true;
    }
}