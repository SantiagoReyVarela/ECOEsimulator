using UnityEngine;

[ExecuteInEditMode]
public class ModularColorTint : MonoBehaviour
{
    private static readonly int TintPropID = Shader.PropertyToID("_BaseTint");
    public enum RoomType { Clinica, Pasillo }
    public RoomType ubicacion;

    [Header("Colores Predefinidos")]
    public Color colorClinica = new Color(0.1f, 0.2f, 0.5f); // Azul Marino
    public Color colorPasillo = new Color(0.1f, 0.5f, 0.4f); // Azul Verdoso

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    // Esta función se activa SOLAMENTE cuando tocas algo en el Inspector
    void OnValidate()
    {
        UpdateColor();
    }

    // Esta se activa cuando el objeto aparece en escena
    void Awake()
    {
        UpdateColor();
    }

    public void UpdateColor()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

        _renderer.GetPropertyBlock(_propBlock);

        Color colorFinal = (ubicacion == RoomType.Clinica)
        ? colorFinal = this.colorClinica
        : colorFinal = this.colorPasillo;

        _propBlock.SetColor(TintPropID, colorFinal);
        _renderer.SetPropertyBlock(_propBlock);
    }
}