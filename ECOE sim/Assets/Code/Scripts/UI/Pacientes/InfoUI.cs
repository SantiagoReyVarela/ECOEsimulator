using TMPro;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    public TMP_Text nombre;
    public TMP_Text edad;
    public TMP_Text sexo;

    public TMP_Text incidentes;
    public TMP_Text sintomas;

    public void Mostrar(CasoClinico caso)
    {
        nombre.text = caso.informacionPaciente.nombre;

        edad.text = caso.informacionPaciente.edad.ToString();

        sexo.text = caso.informacionPaciente.sexo;

        incidentes.text = "";

        foreach (string i in caso.informacionPaciente.incidentesPrevios)
        {
            incidentes.text += "- " + i + "\n";
        }

        sintomas.text = "";

        foreach (string s in caso.sintomas)
        {
            sintomas.text += "- " + s + "\n";
        }
    }
}