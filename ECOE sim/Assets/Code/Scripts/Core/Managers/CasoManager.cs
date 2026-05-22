using UnityEngine;

public class CasoManager : MonoBehaviour
{
    public InfoUI infoUI;
    public ValidadorRespuestas validador;
    public ImagenPacienteUI imagenUI;
    public MarchaController personaje;

    public void CargarCaso(CasoClinico caso)
    {
        infoUI.Mostrar(caso);
        
        imagenUI.MostrarImagen(caso);

        validador.Configurar(caso);

        personaje.ConfigurarCaso(caso);

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