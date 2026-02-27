using UnityEngine;
using System.Collections;

public class Door : ObjInteractive
{
    private bool isOpen = false;
    private bool isMoving = false;

    [SerializeField] private Transform doorPivot;
    [SerializeField] private float openAngle = -90f; 
    //cambiarlo entre positivo/negativo si quieres que gire al otro lado

    [SerializeField] private float speed = 5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = doorPivot.rotation;
        openRotation = Quaternion.Euler(
            doorPivot.eulerAngles + new Vector3(0, openAngle, 0)
        );
    }

    public override void Interact()
    {
        if (!canInteract || isMoving) return;

        isOpen = !isOpen;
        StartCoroutine(RotateDoor(isOpen ? openRotation : closedRotation));
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isMoving = true;

        while (Quaternion.Angle(doorPivot.rotation, targetRotation) > 0.1f)
        {
            doorPivot.rotation = Quaternion.Slerp(
                doorPivot.rotation,
                targetRotation,
                Time.deltaTime * speed
            );

            yield return null;
        }

        doorPivot.rotation = targetRotation;
        isMoving = false;
    }
}