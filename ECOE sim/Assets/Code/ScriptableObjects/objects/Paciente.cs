using UnityEngine;
using UnityEngine.SceneManagement;

public class Paciente : ObjInteractive, IInteractable
{
    public override void Interact()
    {
        if (!canInteract) return;

        SceneManager.LoadScene("PruebaPacientes");
    }
}