using UnityEngine;

public class pruebaInteraccion : ObjInteractive
{
    private bool isOn = false;

    public override void Interact()
    {
        if (!canInteract) return;

        isOn = !isOn;
        Debug.Log(isOn ? "Interacción ejecutada " : "Interacción desactivada ");
    }
}