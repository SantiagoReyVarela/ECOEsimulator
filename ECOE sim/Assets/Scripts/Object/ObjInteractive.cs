using UnityEngine;

public abstract class ObjInteractive : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool canInteract = true;
    public string interactionMessage = "Pulsa E para interactuar";

    protected bool playerInRange = false;

    public virtual void Interact()
    {
        if (!canInteract) return;
        Debug.Log($"Interacción con {gameObject.name}");
    }

    public virtual void OnPlayerEnter()
    {
        playerInRange = true;

        // ← Aquí mostramos el mensaje
        if (InteractionPromptUI.Instance != null)
        {
            InteractionPromptUI.Instance.Show(interactionMessage);
        }

        Debug.Log(interactionMessage);
    }

    public virtual void OnPlayerExit()
    {
        playerInRange = false;

        // ← Aquí ocultamos el mensaje
        if (InteractionPromptUI.Instance != null)
        {
            InteractionPromptUI.Instance.Hide();
        }
    }
}