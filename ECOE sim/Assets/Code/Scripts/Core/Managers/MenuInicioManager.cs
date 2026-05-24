using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicioManager : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelOpciones;
    public GameObject panelCreditos;

    // =========================
    // ESCENAS
    // =========================

    public void Jugar()
    {
        SceneManager.LoadScene("Juego");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
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

        // Para pruebas en el editor
        Debug.Log("Salir del juego");
    }
}