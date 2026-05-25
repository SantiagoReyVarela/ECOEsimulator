using UnityEngine;
using UnityEngine.SceneManagement;

public class Profesor : ObjInteractive, IInteractable
{
    public override void Interact()
    {
        if (!canInteract) return;

        SceneManager.LoadScene("PruebaTest");
    }
}