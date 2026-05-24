using UnityEngine;

public class CasoManager : MonoBehaviour
{
    public InfoUI infoUI;

    public PreguntaUI patronUI;

    public PreguntaUI cuadroUI;

    public PreguntaUI manifestacionesUI;

    public ValidadorRespuestas validador;

    public ImagenPacienteUI imagenUI;

    public void CargarCaso(CasoClinico caso)
    {
        infoUI.Mostrar(caso);

        imagenUI.MostrarImagen(caso);

        patronUI.CrearPregunta(
            caso.patronMarcha.pregunta,
            caso.patronMarcha.opciones,
            false
        );

        cuadroUI.CrearPregunta(
            caso.cuadroPatologico.pregunta,
            caso.cuadroPatologico.opciones,
            false
        );

        manifestacionesUI.CrearPregunta(
            caso.manifestacionesPropias.pregunta,
            caso.manifestacionesPropias.opciones,
            true
        );

        validador.Configurar(caso);
    }
}