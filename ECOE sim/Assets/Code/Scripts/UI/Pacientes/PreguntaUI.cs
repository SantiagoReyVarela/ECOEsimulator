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

    private bool multiple;

    public void CrearPregunta(
        string pregunta,
        List<string> opciones,
        bool multipleSeleccion)
        {
            textoPregunta.text = pregunta;

            multiple = multipleSeleccion;

            seleccionados.Clear();

            foreach (Transform child in contenedorBotones)
            {
                Destroy(child.gameObject);
            }

            botones.Clear();

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

                boton.onClick.AddListener(() =>
                {
                    Seleccionar(indice, boton);
                });

                botones.Add(boton);
            }

            Debug.Log("PREGUNTA: " + pregunta);
            Debug.Log("NUM OPCIONES: " + opciones.Count);

            foreach (var op in opciones)
            {
                Debug.Log(op);
            }
        }

    void Seleccionar(int indice, Button boton)
    {
        Image imagen = boton.GetComponent<Image>();

        if (multiple)
        {
            if (seleccionados.Contains(indice))
            {
                seleccionados.Remove(indice);

                imagen.color = Color.white;
            }
            else
            {
                seleccionados.Add(indice);

                imagen.color = Color.green;
            }
        }
        else
        {
            seleccionados.Clear();

            foreach (Button b in botones)
            {
                b.GetComponent<Image>().color = Color.white;
            }

            seleccionados.Add(indice);

            imagen.color = Color.green;
        }
    }

    public List<int> ObtenerSeleccionados()
    {
        return seleccionados;
    }
}