using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PreguntaUI : MonoBehaviour
{
    public TMP_Text preguntaText;

    public Transform contenedorOpciones;

    public GameObject prefabToggle;

    private List<Toggle> toggles = new();

    public void CrearPregunta(
        string pregunta,
        List<string> opciones,
        bool multiple)
    {
        preguntaText.text = pregunta;

        foreach (Transform child in contenedorOpciones)
        {
            Destroy(child.gameObject);
        }

        toggles.Clear();

        ToggleGroup grupo = null;

        if (!multiple)
        {
            grupo = contenedorOpciones.gameObject.GetComponent<ToggleGroup>();

            if (grupo == null)
            {
                grupo = contenedorOpciones.gameObject.AddComponent<ToggleGroup>();
            }
        }

        foreach (string opcion in opciones)
        {
            GameObject obj =
                Instantiate(prefabToggle, contenedorOpciones);

            Toggle toggle = obj.GetComponent<Toggle>();

            TMP_Text txt =
                obj.GetComponentInChildren<TMP_Text>();

            txt.text = opcion;

            if (!multiple)
            {
                toggle.group = grupo;
            }

            toggles.Add(toggle);
        }
    }

    public List<int> ObtenerSeleccionados()
    {
        List<int> seleccionados = new();

        for (int i = 0; i < toggles.Count; i++)
        {
            if (toggles[i].isOn)
            {
                seleccionados.Add(i);
            }
        }

        return seleccionados;
    }
}