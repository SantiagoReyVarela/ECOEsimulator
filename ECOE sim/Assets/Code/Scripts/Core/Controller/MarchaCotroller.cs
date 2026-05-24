using UnityEngine;
using System.Collections;

public class MarchaController : MonoBehaviour
{
    public Animator animator;

    public float velocidad = 1.5f;

    public float duracion = 6f;

    private CasoClinico casoActual;

    public void ConfigurarCaso(CasoClinico caso)
    {
        casoActual = caso;

        // Idle al cargar escena
        animator.SetInteger("Marcha", 0);
    }

    public void IniciarMarcha()
    {
        if (casoActual == null)
            return;

        string titulo =
            casoActual.titulo.ToLower();

        int marcha = 0;

        if (titulo.Contains("hemiplejica"))
            marcha = 1;

        else if (titulo.Contains("festinante"))
            marcha = 2;

        else if (titulo.Contains("ataxica"))
            marcha = 3;

        else if (titulo.Contains("estepage"))
            marcha = 4;

        animator.SetInteger("Marcha", marcha);

        StopAllCoroutines();

        StartCoroutine(Caminar());
    }

    IEnumerator Caminar()
    {
        float tiempo = 0;

        while (tiempo < duracion)
        {
            transform.Translate(
                -Vector3.forward *
                velocidad *
                Time.deltaTime
            );

            tiempo += Time.deltaTime;

            yield return null;
        }

        // volver a idle
        animator.SetInteger("Marcha", 0);
    }
}