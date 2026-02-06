using UnityEngine;

public abstract class ObjInteractive : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool canInteract = true;
    public string interactionMessage = "Pulsa E para interactuar";

    protected bool playerInRange = false;

    // Se llama cuando el jugador presiona E
    public virtual void Interact()
    {
        if (!canInteract) return;

        Debug.Log($"Interacción con {gameObject.name}");
    }

    // Se llama cuando el jugador entra en el trigger
    public virtual void OnPlayerEnter()
    {
        playerInRange = true;

        // poner codigo para cuando el jugador esté en el rango

        Debug.Log(interactionMessage);
    }

    // Se llama cuando el jugador sale del trigger
    public virtual void OnPlayerExit()
    {
        playerInRange = false;

        // ocultar lo que sea que se muestre al acercarnos al objeto
    }
}
