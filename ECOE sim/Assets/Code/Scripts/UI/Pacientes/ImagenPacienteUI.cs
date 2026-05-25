using UnityEngine;
using UnityEngine.UI;

public class ImagenPacienteUI : MonoBehaviour
{
    public Image imagenPaciente;

    public Sprite hombreJoven;
    public Sprite hombreMayor;

    public Sprite mujerJoven;
    public Sprite mujerMayor;

    public void MostrarImagen(CasoClinico caso)
    {
        string sexo = caso.informacionPaciente.sexo.ToLower();

        int edad = caso.informacionPaciente.edad;

        bool mayor = edad >= 50;

        if (sexo.Contains("masculino") || sexo.Contains("hombre"))
        {
            imagenPaciente.sprite =
                mayor ? hombreMayor : hombreJoven;
        }
        else
        {
            imagenPaciente.sprite =
                mayor ? mujerMayor : mujerJoven;
        }
    }
}