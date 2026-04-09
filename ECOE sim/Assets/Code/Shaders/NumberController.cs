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

    void OnValidate() // Esto hace que el número cambie en tiempo real en el editor
    {
        ActualizarNumero();
    }

    void ActualizarNumero()
    {
        if (_renderer == null) _renderer = GetComponent<Renderer>();
        if (_propBlock == null) _propBlock = new MaterialPropertyBlock();

        // 1. Obtener la propiedad del bloque actual
        _renderer.GetPropertyBlock(_propBlock);

        // 2. Lógica matemática para el Offset (Matriz 4x4)
        // Calculamos en qué fila y columna está el número
        int indiceCero = numeroActual; // Pasamos de 1-16 a 0-15
        int col = indiceCero % columnas;
        int fila = indiceCero / columnas;

        // Calculamos el desplazamiento (0.25 si es 4x4)
        float offsetX = (float)col / columnas;
        float offsetY = 1.0f - ((float)fila / filas);
        // El eje Y en las texturas suele ir de abajo a arriba, por eso la resta

        // 3. Enviamos el valor al Shader (usando el nombre de referencia)
        _propBlock.SetVector("_Offset", new Vector2(offsetX, offsetY));

        // 4. Aplicamos el bloque al renderer del objeto
        _renderer.SetPropertyBlock(_propBlock);
    }
}