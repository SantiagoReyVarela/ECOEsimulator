using UnityEngine;

[ExecuteInEditMode]
public class EnvironmentManager : MonoBehaviour
{
    [Header("Paleta de Colores Global")]
    public Color colorClinica = new Color(0.1f, 0.2f, 0.5f);
    public Color colorPasillo = new Color(0.1f, 0.5f, 0.4f);

    // Instancia estática para que las paredes encuentren al Manager fácilmente
    public static EnvironmentManager Instance;

    void Awake()
    {
        Instance = this;
        ActualizarTodo();
    }

    void OnValidate()
    {
        Instance = this;
        ActualizarTodo();
    }

    // Esta función busca todas las paredes y les ordena actualizarse
    public void ActualizarTodo()
    {
        ModularColorTint[] paredes = Object.FindObjectsByType<ModularColorTint>(FindObjectsSortMode.None);
        foreach (ModularColorTint pared in paredes)
        {
            pared.UpdateColor();
        }
    }
}