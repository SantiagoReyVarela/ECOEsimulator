using UnityEngine;

public class CargadorJSON : MonoBehaviour
{
    public CasoManager casoManager;

    private Root datos;

    void Start()
    {
        CargarJSON();
        CargarCasoAleatorio();
    }

    void CargarJSON()
    {
        TextAsset archivo =
            Resources.Load<TextAsset>("casos");

        if (archivo == null)
        {
            Debug.LogError("No se encontró el archivo JSON");
            return;
        }

        datos =
            JsonUtility.FromJson<Root>(archivo.text);

        if (datos == null || datos.casos == null || datos.casos.Count == 0)
        {
            Debug.LogError("El JSON está vacío o mal formado");
            return;
        }

        Debug.Log("Casos cargados: " + datos.casos.Count);
    }

    public void CargarCasoAleatorio()
    {
        if (datos == null || datos.casos.Count == 0)
            return;

        int indice =
            Random.Range(0, datos.casos.Count);

        CasoClinico caso =
            datos.casos[indice];

        Debug.Log("Caso aleatorio: " + caso.titulo);

        casoManager.CargarCaso(caso);
    }
}