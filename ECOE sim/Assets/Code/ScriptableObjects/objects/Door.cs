using UnityEngine;
using System.Collections;

public class Door : ObjInteractive, IInteractable
{
    private bool isOpen = false;
    private bool isMoving = false;

    [SerializeField] private Transform doorPivot;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float speed = 5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;
    private Coroutine currentRotationCoroutine;

    void Start()
    {
        // Guardamos la rotación inicial (cerrada)
        closedRotation = doorPivot.localRotation;

        // Calculamos la rotación abierta
        openRotation = Quaternion.Euler(
            doorPivot.localEulerAngles + new Vector3(0, openAngle, 0)
        );
    }

    public override void Interact()
    {
        if (!canInteract) return;

        // Cambiamos estado SIEMPRE (aunque esté animando)
        isOpen = !isOpen;

        // Si hay animación en curso, la detenemos
        if (currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
        }

        // Iniciamos nueva animación desde la rotación actual
        currentRotationCoroutine = StartCoroutine(
            RotateDoor(isOpen ? openRotation : closedRotation)
        );
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
        isMoving = true;

        while (Quaternion.Angle(doorPivot.localRotation, targetRotation) > 0.1f)
        {
            doorPivot.localRotation = Quaternion.Slerp(
                doorPivot.localRotation,
                targetRotation,
                Time.deltaTime * speed
            );

            yield return null;
        }

        // Asegura que llega exacto al final
        doorPivot.localRotation = targetRotation;

        isMoving = false;
        currentRotationCoroutine = null;
    }
}