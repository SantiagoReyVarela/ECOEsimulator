using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;

    [Header("Configuración de Simulación")]
    public bool modoAlertaActivo = false;
    [Range(0, 1)] public float nivelDeEstres = 0f;

    [Header("Paleta de Colores Arquitectura")]
    public Color colorClinica = new Color(0.1f, 0.2f, 0.5f);
    public Color colorPasillo = new Color(0.1f, 0.5f, 0.4f);

    [Header("Post-Procesado")]
    public PostProcessVolume volumenGlobal;
    private Vignette _vignette;
    private ColorGrading _colorGrading;

    [Header("Variables Globales Shader")]
    [Range(0, 5)] public float intensidadEmisivaGlobal = 1.0f;
    private static readonly int EmissionPropID = Shader.PropertyToID("_GlobalEmissionIntensity");

    // El "Dirty Flag"
    private bool _isDirty = false;
    private List<ModularColorTint> _objetosRegistrados = new List<ModularColorTint>();

    void Awake()
    {
        Instance = this;
        InicializarPostProcesado();
        ReescanearEscena();
    }

    void OnValidate()
    {
        _isDirty = true; // Marcamos que algo ha cambiado
    }

    void Update()
    {
        // Solo trabajamos si el "Dirty Flag" está activo o si hay una animación (estrés)
        if (_isDirty || modoAlertaActivo)
        {
            ActualizarTodo();
            _isDirty = false;
        }
    }
    public void ReescanearEscena()
    {
        _objetosRegistrados.Clear();
        ModularColorTint[] objetos = FindObjectsOfType<ModularColorTint>();
        foreach (var obj in objetos) RegistrarObjeto(obj);
        ActualizarTodo();
    }

    private void InicializarPostProcesado()
    {
        if (volumenGlobal != null && volumenGlobal.profile != null)
        {
            volumenGlobal.profile.TryGetSettings(out _vignette);
            volumenGlobal.profile.TryGetSettings(out _colorGrading);
        }
    }

    public void ActualizarTodo()
    {
        // 1. Actualizar Paredes
        foreach (var pared in _objetosRegistrados)
        {
            if (pared != null) pared.UpdateColor();
        }

        // 2. Actualizar Shaders Globales
        Shader.SetGlobalFloat(EmissionPropID, intensidadEmisivaGlobal);

        // 3. Lógica de Post-Procesado y "Estado de Alerta"
        if (_vignette != null)
        {
            // Si hay estrés, el vignette "late" o aumenta
            float baseVignette = modoAlertaActivo ? 0.45f : 0.25f;
            float pulse = modoAlertaActivo ? Mathf.Sin(Time.time * 5f) * 0.05f : 0;
            _vignette.intensity.value = baseVignette + pulse;
        }

        if (_colorGrading != null)
        {
            // Virar a rojo si hay alerta grave
            _colorGrading.colorFilter.value = Color.Lerp(Color.white, new Color(1f, 0.8f, 0.8f), nivelDeEstres);
        }
    }

    // --- Gestión de Registro de Objetos ---
    public void RegistrarObjeto(ModularColorTint objeto)
    {
        if (!_objetosRegistrados.Contains(objeto)) _objetosRegistrados.Add(objeto);
    }
    public void DesregistrarObjeto(ModularColorTint objeto)
    {
        if (_objetosRegistrados.Contains(objeto)) _objetosRegistrados.Remove(objeto);
    }
}