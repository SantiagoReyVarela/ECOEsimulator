using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class MarchaController : MonoBehaviour
{
    public Animator animator;
    public float velocidad = 1.0f;
    private int marchaActual;

    private Vector3 posicionInicial;
    private Quaternion rotacionInicial;
    private bool ejecutando = false;
    public bool IsEjecutando => ejecutando;

    public void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
    }

    public void ConfigurarCaso(CasoClinico caso)
    {
        marchaActual = caso.patronMarcha.correcta + 1;

        animator.SetInteger("MarchaID", 0);
    }

    public void IniciarMarcha()
    {
        // 🔒 BLOQUEO: evita re-ejecución
        if (ejecutando)
            return;

        StartCoroutine(CicloMarcha());
    }

    IEnumerator CicloMarcha()
    {
        ejecutando = true; //espera activa

        // 1. ACTIVAR ANIMACION
        animator.SetInteger("MarchaID", marchaActual);

        // =========================
        // 2. IR HACIA DELANTE (3s)
        // =========================
        yield return StartCoroutine(Mover(-Vector3.forward, 3f));

        // 3. GIRAR 180°
        transform.rotation = rotacionInicial * Quaternion.Euler(0, 180f, 0);

        yield return new WaitForSeconds(0.2f);

        // =========================
        // 4. VOLVER (3s)
        // =========================
        yield return StartCoroutine(Mover(-Vector3.forward, 3f));

        // 5. ESPERAR
        yield return new WaitForSeconds(0.1f);

        // 6. VOLVER A IDLE
        animator.SetInteger("MarchaID", 0);

        transform.rotation = rotacionInicial;

        ejecutando = false;
    }

    IEnumerator Mover(Vector3 direccion, float duracion)
    {
        float t = 0;

        while (t < duracion)
        {
            transform.Translate(
                direccion * velocidad * Time.deltaTime
            );

            t += Time.deltaTime;
            yield return null;
        }
    }
}