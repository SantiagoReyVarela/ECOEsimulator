using UnityEngine;
using UnityEditor;

public class MaterialFixer : EditorWindow
{
    [MenuItem("Tools/Limpiar Instancias de Materiales")]
    public static void CleanSceneMaterials()
    {
        // Buscamos todos los MeshRenderers de la escena
        Renderer[] allRenderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        int count = 0;

        foreach (Renderer r in allRenderers)
        {
            // Forzamos a que el objeto use el material compartido (el del archivo)
            // Esto elimina la copia "(Instance)" que vive en la jerarquía
            Material sharedMat = r.sharedMaterial;
            if (sharedMat != null)
            {
                r.sharedMaterial = sharedMat;
                count++;
            }
        }
        Debug.Log($"<color=green>Limpieza completada:</color> {count} objetos han vuelto a su material base.");
    }
}