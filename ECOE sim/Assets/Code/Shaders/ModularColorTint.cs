using UnityEngine;

public class ModularColorTint : MonoBehaviour
{
    // Tu variable única del Shader
    private static readonly int TintPropID = Shader.PropertyToID("_GlobalColorArquitecture");

    public enum RoomType { Clinica, Pasillo }
    [Header("Configuración")]
    public RoomType ubicacion;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void OnEnable()
    {
        // Se apunta a la lista del Manager al activarse
        if (EnvironmentManager.Instance != null)
            EnvironmentManager.Instance.RegistrarObjeto(this);

        UpdateColor();
    }

    void OnDisable()
    {
        // Se borra de la lista al destruirse o desactivarse
        if (EnvironmentManager.Instance != null)
            EnvironmentManager.Instance.DesregistrarObjeto(this);
    }

    void OnValidate()
    {
        UpdateColor();
    }

    public void UpdateColor()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

        Color colorFinal = Color.white;

        if (EnvironmentManager.Instance != null)
        {
            colorFinal = (ubicacion == RoomType.Clinica)
                ? EnvironmentManager.Instance.colorClinica
                : EnvironmentManager.Instance.colorPasillo;
        }

        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor(TintPropID, colorFinal);
        _renderer.SetPropertyBlock(_propBlock);
    }
}