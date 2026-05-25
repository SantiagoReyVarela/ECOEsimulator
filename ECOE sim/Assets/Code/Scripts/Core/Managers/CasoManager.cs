using UnityEngine;

public class CasoManager : MonoBehaviour
{
    public InfoUI infoUI;

    public PreguntaUI patronUI;

    public PreguntaUI cuadroUI;

    public PreguntaUI manifestacionesUI;

    public ValidadorRespuestas validador;

    public ImagenPacienteUI imagenUI;

    public MarchaController personaje;

    public void CargarCaso(CasoClinico caso)
    {
        infoUI.Mostrar(caso);
        imagenUI.MostrarImagen(caso);

        Debug.Log("infoUI: " + infoUI);
        Debug.Log("imagenUI: " + imagenUI);
        Debug.Log("validador: " + validador);
        Debug.Log("personaje: " + personaje);

        

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

        // GUARDAR CASO EN PERSONAJE
        personaje.ConfigurarCaso(caso);
    }
}