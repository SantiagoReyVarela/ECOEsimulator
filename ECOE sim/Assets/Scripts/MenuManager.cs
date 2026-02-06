using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void SceneInicio()
    {
        Debug.Log("Intentando cargar escena: Inicio");
        SceneManager.LoadScene("Inicio");
        Debug.Log("Carga de escena Inicio ejecutada");
    }

    public void SceneSeleccion()
    {
        Debug.Log("Intentando cargar escena: Seleccion");
        SceneManager.LoadScene("Seleccion");
        Debug.Log("Carga de escena Seleccion ejecutada");
    }

    public void SceneCreditos()
    {
        Debug.Log("Intentando cargar escena: Creditos");
        SceneManager.LoadScene("Creditos");
        Debug.Log("Carga de escena Creditos ejecutada");
    }

    public void SceneConfig()
    {
        Debug.Log("Intentando cargar escena: Config");
        SceneManager.LoadScene("Config");
        Debug.Log("Carga de escena Config ejecutada");
    }

    public void ScenePartida()
    {
        Debug.Log("Intentando cargar escena: Partida");
        SceneManager.LoadScene("Partida");
        Debug.Log("Carga de escena Partida ejecutada");
    }

    public void QuitGame()
    {
        Debug.Log("Botón salir pulsado");
        Application.Quit();
        Debug.Log("Application.Quit() ejecutado");
    }
}
