using System.Collections.Generic;
using UnityEngine;

public class ValidadorRespuestas : MonoBehaviour
{
    public PreguntaUI patronUI;
    public PreguntaUI cuadroUI;
    public PreguntaUI manifestacionesUI;

    private CasoClinico casoActual;
    private bool ultimoResultado;

    // Propiedades públicas para que la UI lea los desgloses
    public int PuntuacionFinal { get; private set; }
    public bool PatronCorrecto { get; private set; }
    public bool CuadroCorrecto { get; private set; }
    public bool ManifestacionesCorrectas { get; private set; }

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
        PatronCorrecto = patronUI.ObtenerSeleccionados().Count > 0 &&
                         patronUI.ObtenerSeleccionados()[0] == casoActual.patronMarcha.correcta;

        if (PatronCorrecto)
            puntuacion += 4;

        // -------------------
        // CUADRO (3 pts)
        // -------------------
        CuadroCorrecto = cuadroUI.ObtenerSeleccionados().Count > 0 &&
                         cuadroUI.ObtenerSeleccionados()[0] == casoActual.cuadroPatologico.correcta;

        if (CuadroCorrecto)
            puntuacion += 3;

        // -------------------
        // MANIFESTACIONES (3 pts)
        // -------------------
        List<int> seleccionadas = manifestacionesUI.ObtenerSeleccionados();

        ManifestacionesCorrectas = CompararListas(seleccionadas, casoActual.manifestacionesPropias.correctas);

        if (ManifestacionesCorrectas)
            puntuacion += 3;

        // -------------------
        // RESULTADO FINAL
        // -------------------
        PuntuacionFinal = puntuacion;

        // CORRECCIÓN: Guardamos si el alumno aprueba (nota >= 5)
        ultimoResultado = PuntuacionFinal >= 5;

        Debug.Log("PUNTUACIÓN FINAL GUARDADA: " + PuntuacionFinal + " / 10");
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