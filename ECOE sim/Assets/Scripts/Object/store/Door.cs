using UnityEngine;

public class Door : ObjInteractive
{
    private bool isOpen = false;

    public override void Interact()
    {
        if (!canInteract) return;

        isOpen = !isOpen;
        Debug.Log(isOpen ? "Puerta abierta " : "Puerta cerrada ");
    }
}