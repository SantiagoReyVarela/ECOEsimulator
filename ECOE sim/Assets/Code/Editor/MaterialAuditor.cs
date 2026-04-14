using UnityEngine;
using System.Linq;

public class MaterialAuditor : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) // Pulsa M en el juego para auditar
        {
            var renderers = Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None);
            var instances = renderers.Where(r => r.sharedMaterial.name.Contains("(Instance)")).ToList();

            Debug.Log($"<color=red>AUDITORÍA:</color> Se han encontrado {instances.Count} materiales instanciados.");

            foreach (var r in instances.Take(5)) // Mostramos los 5 primeros para no saturar
            {
                Debug.Log($"Objeto clonado: {r.name} | Material: {r.sharedMaterial.name}", r);
            }
        }
    }
}