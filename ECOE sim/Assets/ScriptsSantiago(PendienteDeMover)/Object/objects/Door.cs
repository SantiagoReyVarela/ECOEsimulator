using UnityEngine;
using System.Collections;

public class Door : ObjInteractive
{
    private bool isOpen = false;
<<<<<<<< HEAD:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/store/Door.cs
    private bool isMoving = false;

    [SerializeField] private Transform doorPivot;
    [SerializeField] private float openAngle = -90f; 
    //cambiarlo entre positivo/negativo si quieres que gire al otro lado

========

    [SerializeField] private Transform doorPivot;
    [SerializeField] private float openAngle = -90f;
>>>>>>>> santiago:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/objects/Door.cs
    [SerializeField] private float speed = 5f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

<<<<<<<< HEAD:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/store/Door.cs
========
    private Coroutine currentRotationCoroutine;

>>>>>>>> santiago:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/objects/Door.cs
    private void Start()
    {
        closedRotation = doorPivot.rotation;
        openRotation = Quaternion.Euler(
            doorPivot.eulerAngles + new Vector3(0, openAngle, 0)
        );
    }

    public override void Interact()
    {
<<<<<<<< HEAD:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/store/Door.cs
        if (!canInteract || isMoving) return;

        isOpen = !isOpen;
        StartCoroutine(RotateDoor(isOpen ? openRotation : closedRotation));
========
        if (!canInteract) return;

        // Alternamos estado
        isOpen = !isOpen;

        // Si ya hay una animación en curso, la detenemos
        if (currentRotationCoroutine != null)
        {
            StopCoroutine(currentRotationCoroutine);
        }

        // Iniciamos nueva animación hacia el nuevo estado
        currentRotationCoroutine = StartCoroutine(
            RotateDoor(isOpen ? openRotation : closedRotation)
        );
>>>>>>>> santiago:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/objects/Door.cs
    }

    private IEnumerator RotateDoor(Quaternion targetRotation)
    {
<<<<<<<< HEAD:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/store/Door.cs
        isMoving = true;

========
>>>>>>>> santiago:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/objects/Door.cs
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
<<<<<<<< HEAD:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/store/Door.cs
        isMoving = false;
========
        currentRotationCoroutine = null;
>>>>>>>> santiago:ECOE sim/Assets/ScriptsSantiago(PendienteDeMover)/Object/objects/Door.cs
    }
}