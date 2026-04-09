using UnityEngine;

public class NumberController : MonoBehaviour
{
    [Header("Configuración de la Cuadrícula")]
    public int filas = 4;
    public int columnas = 4;

    [Header("Número a mostrar")]
    [Range(1, 16)] public int numeroActual = 1;

    private Renderer _renderer;
    private MaterialPropertyBlock _propBlock;

    void OnValidate()
    {
        ActualizarNumero();
    }

    void ActualizarNumero()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

        _renderer.GetPropertyBlock(_propBlock);

        // 1. Convertimos el número (1-16) a un índice base cero (0-15)
        int indice = numeroActual - 1;

        // 2. Calculamos columna (X) y fila (Y)
        int col = indice % columnas;    // Resultado: 0, 1, 2, 3
        int fila = indice / columnas;   // Resultado: 0, 1, 2, 3

        // 3. Cálculo de Offset
        float offsetX = ((float)col / columnas) ;

        // Invertimos la fila: 
        // Cuando fila es 0 (arriba), el offset debe ser 0.75
        // Cuando fila es 3 (abajo), el offset debe ser 0.0
        float offsetY = 1 - ((float)(fila) / filas) ;

        // 4. Aplicamos al Shader
        _propBlock.SetVector("_Offset", new Vector2(offsetX, offsetY));
        _renderer.SetPropertyBlock(_propBlock);
    }
}