using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void SceneInicio()
    {
        SceneManager.LoadScene("Inicio"); 
        // nombre de la escena del juego
    }

    public void SceneSeleccion()
    {
        SceneManager.LoadScene("Seleccion"); 
        // nombre de la escena del juego
    }

    public void SceneCreditos()
    {
        SceneManager.LoadScene("Creditos"); 
        // nombre de la escena del juego
    }

    public void SceneConfig()
    {
        SceneManager.LoadScene("Config"); 
        // nombre de la escena del juego
    }

    public void ScenePartida()
    {
        SceneManager.LoadScene("Partida"); 
        // nombre de la escena del juego
    }





    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Juego cerrado");
    }
}
