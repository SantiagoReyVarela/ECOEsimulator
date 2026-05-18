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
    private ToggleGroup grupo;

    public void CrearPregunta(string pregunta, List<string> opciones, bool multiple)
    {
        preguntaText.text = pregunta;

        foreach (Transform child in contenedorOpciones)
        {
            Destroy(child.gameObject);
        }

        toggles.Clear();

        if (!multiple)
        {
            if (grupo == null)
                grupo = contenedorOpciones.gameObject.AddComponent<ToggleGroup>();
        }

        for (int i = 0; i < opciones.Count; i++)
        {
            GameObject obj = Instantiate(prefabToggle, contenedorOpciones);

            Toggle toggle = obj.GetComponent<Toggle>();
            TMP_Text txt = obj.GetComponentInChildren<TMP_Text>();

            txt.text = opciones[i];

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
                seleccionados.Add(i);
        }

        return seleccionados;
    }

    public void Resetear()
    {
        foreach (var t in toggles)
            t.isOn = false;
    }
}