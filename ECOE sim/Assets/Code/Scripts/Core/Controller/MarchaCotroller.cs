using UnityEngine;
using System.Collections;

public class MarchaController : MonoBehaviour
{
    public Animator animator;

    public float velocidad = 1.5f;

    public float duracion = 5f;

    private bool caminando = false;

    public void ReproducirMarcha(CasoClinico caso)
    {
        string titulo =
            caso.titulo.ToLower();

        int marcha = 0;

        if (titulo.Contains("hemiplejica"))
            marcha = 0;

        else if (titulo.Contains("festinante"))
            marcha = 1;

        else if (titulo.Contains("ataxica"))
            marcha = 2;

        else if (titulo.Contains("estepage"))
            marcha = 3;

        animator.SetInteger("Marcha", marcha);

        StartCoroutine(Caminar());
    }

    IEnumerator Caminar()
    {
        caminando = true;

        float tiempo = 0;

        while (tiempo < duracion)
        {
            transform.Translate(
                Vector3.forward *
                velocidad *
                Time.deltaTime);

            tiempo += Time.deltaTime;

            yield return null;
        }

        caminando = false;

        animator.SetInteger("Marcha", -1);
    }
}