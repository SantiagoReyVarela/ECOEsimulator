using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerExamen : MonoBehaviour
{
    public TMP_Text textoTimer;

    public float tiempoInicial = 300f;

    private float tiempoActual;

    private bool terminado = false;

    public ResultadoExamen resultado;

    void Start()
    {
        tiempoActual = tiempoInicial;
    }

    void Update()
    {
        if (terminado)
            return;

        tiempoActual -= Time.deltaTime;

        if (tiempoActual <= 0)
        {
            tiempoActual = 0;

            terminado = true;

            resultado.MostrarResultado();
        }

        MostrarTiempo();
    }

    void MostrarTiempo()
    {
        int minutos = Mathf.FloorToInt(tiempoActual / 60);

        int segundos = Mathf.FloorToInt(tiempoActual % 60);

        textoTimer.text =
            minutos.ToString("00") + ":" +
            segundos.ToString("00");
    }
}