using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private ObjInteractive currentObject;

    void Update()
    {
        if (currentObject != null && Input.GetKeyDown(KeyCode.E))
        {
            currentObject.Interact();
        }
    }

    void OnTriggerEnter(Collider other)
{
    Debug.Log("ENTER con: " + other.name);

    ObjInteractive obj = other.GetComponentInParent<ObjInteractive>();

    if (obj != null)
    {
        Debug.Log("Objeto interactivo detectado: " + obj.name);

        currentObject = obj;
        obj.OnPlayerEnter();
    }
}

    void OnTriggerExit(Collider other)
    {
        ObjInteractive obj = other.GetComponentInParent<ObjInteractive>();

        if (obj != null && currentObject == obj)
        {
            obj.OnPlayerExit();
            currentObject = null;
        }
    }

    
}