using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicioManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject menuPrincipal;
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
        menuPrincipal.SetActive(false);
        panelOpciones.SetActive(true);
    }

    public void CerrarOpciones()
    {
        panelOpciones.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    // =========================
    // CREDITOS
    // =========================

    public void AbrirCreditos()
    {
        menuPrincipal.SetActive(false);
        panelCreditos.SetActive(true);
    }

    public void CerrarCreditos()
    {
        panelCreditos.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    // =========================
    // SALIR
    // =========================

    public void Salir()
    {
        Application.Quit();

        Debug.Log("Saliendo del juego...");
    }
}