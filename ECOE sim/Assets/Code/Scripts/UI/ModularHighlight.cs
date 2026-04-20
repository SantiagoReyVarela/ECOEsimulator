using UnityEngine;

public class ModularHighlight : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;
    private static readonly int IntensityID = Shader.PropertyToID("_HighlightIntensity");

    void Awake()
    {
        // Al ser malla única, solo necesitamos el Renderer principal
        _renderer = GetComponent<Renderer>();

        // El MaterialPropertyBlock se crea una sola vez (Caching)
        _propBlock = new MaterialPropertyBlock();
    }

    public void SetHighlight(bool estado)
    {
        if (_renderer == null) return;

        float valor = estado ? 1.0f : 0.0f;

        // Recuperamos, modificamos el valor cacheado y aplicamos
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat(IntensityID, valor);
        _renderer.SetPropertyBlock(_propBlock);
    }
}