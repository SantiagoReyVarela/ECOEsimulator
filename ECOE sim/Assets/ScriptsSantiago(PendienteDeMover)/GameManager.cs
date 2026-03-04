// crea un empty en la escena de Inicio
// asi haces que se quede toda la partida y persista pese a las escenas
using UnityEngine;

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Datos del Jugador")]
    public string playerName;

    [Header("Configuración de Partida")]
    public Difficulty difficulty = Difficulty.Normal;

    [Header("Puntuación")]
    public int score = 0;

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

    //  Asegura que el GameManager exista (por si ejecutas escena directamente)
    public static void EnsureExists()
    {
        if (Instance == null)
        {
            GameObject gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
        }
    }

    //  Añadir puntos
    public void AddScore(int amount)
    {
        score += amount;
    }

    //  Reiniciar puntuación (útil para nueva partida)
    public void ResetScore()
    {
        score = 0;
    }
}