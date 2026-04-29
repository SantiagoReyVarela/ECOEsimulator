// crea un empty en la escena de Inicio
// asi haces que se quede toda la partida y persista pese a las escenas
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string playerName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}