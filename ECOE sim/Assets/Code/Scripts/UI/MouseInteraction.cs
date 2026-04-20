using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    public float distanciaMaxima = 5f;
    public LayerMask capaInteractuable;

    private ModularHighlight _objetoActual;
    private Camera _cam; // Referencia cacheada

    void Awake()
    {
        // Cacheamos la cámara una sola vez para evitar buscarla cada frame
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Usamos la referencia _cam directamente
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanciaMaxima, capaInteractuable))
        {
            // Usamos GetComponentInParent por si el collider está en un hijo pero el script en el padre
            ModularHighlight interactuable = hit.collider.GetComponentInParent<ModularHighlight>();

            if (interactuable != null)
            {
                if (_objetoActual != interactuable)
                {
                    ApagarResaltado();
                    _objetoActual = interactuable;
                    _objetoActual.SetHighlight(true);
                }
            }
            else
            {
                ApagarResaltado();
            }
        }
        else
        {
            ApagarResaltado();
        }
    }

    void ApagarResaltado()
    {
        if (_objetoActual != null)
        {
            _objetoActual.SetHighlight(false);
            _objetoActual = null;
        }
    }
}