using UnityEngine;

public class ObjInteractive : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool canInteract = true;
    public string interactionMessage = "Pulsa E para interactuar";

    protected bool playerInRange = false;

    // 🔗 Referencia opcional al objeto que realmente ejecuta la interacción
    [SerializeField] private MonoBehaviour interactionTarget;

    private IInteractable interactable;

    private void Awake()
    {
        // Intentamos convertir el target a IInteractable
        if (interactionTarget != null)
        {
            interactable = interactionTarget as IInteractable;
        }
    }

    public virtual void Interact()
    {
        if (!canInteract) return;

        // Si hay un target (ej: Door), usamos su lógica
        if (interactable != null)
        {
            interactable.Interact();
        }
        else
        {
            Debug.Log($"Interacción con {gameObject.name}");
        }
    }

    public virtual void OnPlayerEnter()
{
    Debug.Log("OnPlayerEnter llamado");

    if (InteractionPromptUI.Instance == null)
    {
        Debug.LogError("Instance es NULL");
    }
    else
    {
        Debug.Log("Instance OK");
        InteractionPromptUI.Instance.Show(interactionMessage);
    }
}

    public virtual void OnPlayerExit()
    {
        playerInRange = false;

        if (InteractionPromptUI.Instance != null)
        {
            InteractionPromptUI.Instance.Hide();
        }
    }
}