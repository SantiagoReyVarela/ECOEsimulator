using UnityEngine;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public const int MAX_SAVES = 5;
    public List<SaveData> partidas = new List<SaveData>();

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        CargarTodas();
    }

    // ---------- CONSULTAS ----------
    public bool HayPartidas()
    {
        return partidas.Count > 0;
    }

    public bool EstaLleno()
    {
        return partidas.Count >= MAX_SAVES;
    }

    // ---------- CREAR ----------
    public void CrearNuevaPartida(string nombreJugador, string nombrePartida)
    {
        SaveData data = new SaveData
        {
            nombreJugador = nombreJugador,
            nombrePartida = nombrePartida
        };

        partidas.Add(data);
        GuardarTodas();
    }

    // ---------- BORRAR ----------
    public void BorrarPartida(int index)
    {
        partidas.RemoveAt(index);
        GuardarTodas();
    }

    // ---------- GUARDADO ----------
    void GuardarTodas()
    {
        PlayerPrefs.SetInt("SaveCount", partidas.Count);

        for (int i = 0; i < partidas.Count; i++)
        {
            PlayerPrefs.SetString($"Save_{i}", JsonUtility.ToJson(partidas[i]));
        }

        PlayerPrefs.Save();
    }

    void CargarTodas()
    {
        partidas.Clear();
        int count = PlayerPrefs.GetInt("SaveCount", 0);

        for (int i = 0; i < count; i++)
        {
            string json = PlayerPrefs.GetString($"Save_{i}");
            partidas.Add(JsonUtility.FromJson<SaveData>(json));
        }
    }
}
