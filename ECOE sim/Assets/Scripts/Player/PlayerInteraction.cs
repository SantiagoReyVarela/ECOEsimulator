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
        if (other.TryGetComponent(out ObjInteractive obj))
        {
            currentObject = obj;
            obj.OnPlayerEnter();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ObjInteractive obj))
        {
            if (currentObject == obj)
            {
                obj.OnPlayerExit();
                currentObject = null;
            }
        }
    }
}
