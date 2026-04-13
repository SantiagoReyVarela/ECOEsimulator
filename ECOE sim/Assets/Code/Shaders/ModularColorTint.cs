using UnityEngine;

[ExecuteInEditMode]
public class ModularColorTint : MonoBehaviour
{
    private static readonly int TintPropID = Shader.PropertyToID("_GlobalColorArquitecture");

    public enum RoomType { Clinica, Pasillo }

    [Header("Configuración de Ubicación")]
    public RoomType ubicacion;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void OnValidate()
    {
        UpdateColor();
    }

    void Awake()
    {
        UpdateColor();
    }

    public void UpdateColor()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

        // 1. Buscamos el color en el Manager
        Color colorFinal = Color.white; // Color por defecto si no hay manager

        if (EnvironmentManager.Instance != null)
        {
            colorFinal = (ubicacion == RoomType.Clinica)
                ? EnvironmentManager.Instance.colorClinica
                : EnvironmentManager.Instance.colorPasillo;
        }

        // 2. Aplicamos el color al material de forma eficiente
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor(TintPropID, colorFinal);
        _renderer.SetPropertyBlock(_propBlock);
    }
}