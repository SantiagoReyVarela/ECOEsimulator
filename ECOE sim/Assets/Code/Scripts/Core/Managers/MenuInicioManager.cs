using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicioManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelOpciones;
    public GameObject panelCreditos;

    // =========================
    // JUGAR
    // =========================

    public void Jugar()
    {
        SceneManager.LoadScene("Partida");
    }

    // =========================
    // OPCIONES
    // =========================

    public void AbrirOpciones()
    {
        panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
    }

    // =========================
    // CREDITOS
    // =========================

    public void AbrirCreditos()
    {
        panelCreditos.SetActive(true);
    }

    public void CerrarCreditos()
    {
        panelCreditos.SetActive(false);
    }

    // =========================
    // SALIR
    // =========================

    public void Salir()
    {
        Application.Quit();

        // Solo visible en el editor
        Debug.Log("Saliendo del juego...");
    }
}