using UnityEngine;

public class CasoManager : MonoBehaviour
{
    public InfoUI infoUI;
    public ValidadorRespuestas validador;

    private CasoClinico casoActual;

    public void CargarCaso(CasoClinico caso)
    {
        casoActual = caso;

        infoUI.Mostrar(casoActual);
        validador.Configurar(casoActual);

        validador.patronUI.CrearPregunta(
            caso.patronMarcha.pregunta,
            caso.patronMarcha.opciones,
            false);

        validador.cuadroUI.CrearPregunta(
            caso.cuadroPatologico.pregunta,
            caso.cuadroPatologico.opciones,
            false);

        validador.manifestacionesUI.CrearPregunta(
            caso.manifestacionesPropias.pregunta,
            caso.manifestacionesPropias.opciones,
            true);
    }
}