using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PreguntaUI : MonoBehaviour
{
    public TMP_Text textoPregunta;

    public Transform contenedorBotones;

    public GameObject prefabBoton;

    private List<Button> botones = new();

    private List<int> seleccionados = new();

    private bool multipleSeleccion;

    [Header("Colores Examen")]
    public Color colorSeleccionado = new Color(1.0f, 0.7f, 0.4f);

    public void CrearPregunta(
        string pregunta,
        List<string> opciones,
        bool multiple)
    {
        textoPregunta.text = pregunta;

        multipleSeleccion = multiple;

        foreach (Transform child in contenedorBotones)
        {
            Destroy(child.gameObject);
        }

        botones.Clear();

        seleccionados.Clear();

        for (int i = 0; i < opciones.Count; i++)
        {
            int indice = i;

            GameObject obj =
                Instantiate(prefabBoton, contenedorBotones);

            TMP_Text txt =
                obj.GetComponentInChildren<TMP_Text>();

            txt.text = opciones[i];

            Button boton =
                obj.GetComponent<Button>();

            Image imagen =
                obj.GetComponent<Image>();

            Color colorOriginal = imagen.color;

            boton.onClick.AddListener(() =>
            {
                if (multipleSeleccion)
                {
                    if (seleccionados.Contains(indice))
                    {
                        seleccionados.Remove(indice);

                        imagen.color = colorOriginal;
                    }
                    else
                    {
                        seleccionados.Add(indice);

                        imagen.color = colorSeleccionado;
                    }
                }
                else
                {
                    seleccionados.Clear();

                    foreach (Button b in botones)
                    {
                        b.GetComponent<Image>().color = colorOriginal;
                    }

                    seleccionados.Add(indice);

                    imagen.color = colorSeleccionado;
                }
            });

            botones.Add(boton);
        }
    }

    public List<int> ObtenerSeleccionados()
    {
        return seleccionados;
    }
}