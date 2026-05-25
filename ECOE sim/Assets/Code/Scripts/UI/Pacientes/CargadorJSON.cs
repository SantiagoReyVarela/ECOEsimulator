using UnityEngine;

public class CargadorJSON : MonoBehaviour
{
    public TextAsset archivoJSON;

    public CasoManager casoManager;

    private Root datos;

    void Start()
    {
        CargarJSON();

        CargarCasoAleatorio();
    }

    void CargarJSON()
    {
        datos =
            JsonUtility.FromJson<Root>(
                archivoJSON.text);
    }

    public void CargarCasoAleatorio()
    {
        if (datos == null || datos.casos.Count == 0)
        {
            Debug.LogError("No hay casos");

            return;
        }

        int indice =
            Random.Range(0, datos.casos.Count);

        CasoClinico caso =
            datos.casos[indice];

        Debug.Log(
            "Caso cargado: "
            + caso.titulo);

        casoManager.CargarCaso(caso);
    }
}