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

    [Header("Paleta de Arquitectura")]
    public Color colorClinica = new Color(0.1f, 0.2f, 0.5f);
    public Color colorPasillo = new Color(0.1f, 0.5f, 0.4f);

    [Header("Feedback Visual (Interacción)")]
    [ColorUsage(true, true)] public Color colorSeleccion = Color.cyan;
    [Range(1, 10)] public float fuerzaFresnel = 4f;

    [Header("Variables Globales Shader")]
    [Range(0, 5)] public float intensidadEmisivaGlobal = 1.0f;

    [Header("Post-Procesado")]
    public PostProcessVolume volumenGlobal;
    private Vignette _vignette;
    private ColorGrading _colorGrading;

    // IDs de Shaders cacheados
    private static readonly int EmissionPropID = Shader.PropertyToID("_GlobalEmissionIntensity");
    private static readonly int SelectionColorID = Shader.PropertyToID("_SelectionColor");
    private static readonly int FresnelPowerID = Shader.PropertyToID("_FresnelPower");

    private bool _isDirty = false;
    [SerializeField]  private List<ModularColorTint> _objetosRegistrados = new List<ModularColorTint>();

    void Awake()
    {
        Instance = this;
        InicializarPostProcesado();
        ReescanearEscena();
    }

    void OnValidate() { _isDirty = true; }

    void Update()
    {
        // 1. Animaciones procedimentales (Se ejecutan cada frame solo si es necesario)
        // Solo animamos el post-procesado si el modo alerta está activo o hay estrés
        if (modoAlertaActivo || nivelDeEstres > 0)
        {
            ActualizarEfectosVisuales();
        }

        // 2. Datos estáticos (Solo se ejecutan cuando el usuario toca algo en el Inspector)
        // Esto ahorra miles de operaciones de CPU al no recorrer la lista de paredes innecesariamente
        if (_isDirty)
        {
            ActualizarDatosEstaticos();
            _isDirty = false;
        }
    }

    /// <summary>
    /// Actualiza parámetros que no cambian cada frame (Colores de paredes, Shaders globales).
    /// </summary>
    private void ActualizarDatosEstaticos()
    {
        // Inyectar Valores Globales a la GPU
        Shader.SetGlobalFloat(EmissionPropID, intensidadEmisivaGlobal);
        Shader.SetGlobalColor(SelectionColorID, colorSeleccion);
        Shader.SetGlobalFloat(FresnelPowerID, fuerzaFresnel);

        // Actualizar paredes registradas
        foreach (var pared in _objetosRegistrados)
        {
            if (pared != null) pared.UpdateColor();
        }

        // También actualizamos el estado base del post-procesado aquí
        ActualizarEfectosVisuales();
    }

    /// <summary>
    /// Gestiona las animaciones dinámicas de la cámara.
    /// </summary>
    private void ActualizarEfectosVisuales()
    {
        // Prevención de NullReference: Si el volumen o el perfil se pierden, salimos
        if (volumenGlobal == null || volumenGlobal.profile == null) return;

        // Animación de Vignette (Pulso de alerta)
        if (_vignette != null)
        {
            float baseVignette = modoAlertaActivo ? 0.45f : 0.25f;
            // Solo aplicamos el Sin(Time) si el modo alerta está activo para ahorrar cálculo
            float pulse = modoAlertaActivo ? Mathf.Sin(Time.time * 5f) * 0.05f : 0;
            _vignette.intensity.value = baseVignette + pulse;
        }

        // Color Grading (Virado al rojo por estrés)
        if (_colorGrading != null)
        {
            _colorGrading.colorFilter.value = Color.Lerp(Color.white, new Color(1f, 0.8f, 0.8f), nivelDeEstres);
        }
    }

    // --- Utilidades de Gestión (Optimización de Memoria) ---

    public void ReescanearEscena()
    {
        // Herramienta de depuración manual. No usar en bucles de juego.
        _objetosRegistrados.Clear();
        ModularColorTint[] objetos = Object.FindObjectsByType<ModularColorTint>(FindObjectsSortMode.None);
        foreach (var obj in objetos) RegistrarObjeto(obj);
        _isDirty = true;
    }

    private void InicializarPostProcesado()
    {
        if (volumenGlobal != null && volumenGlobal.profile != null)
        {
            volumenGlobal.profile.TryGetSettings(out _vignette);
            volumenGlobal.profile.TryGetSettings(out _colorGrading);
        }
    }

    public void RegistrarObjeto(ModularColorTint objeto)
    {
        if (!_objetosRegistrados.Contains(objeto)) _objetosRegistrados.Add(objeto);
    }

    public void DesregistrarObjeto(ModularColorTint objeto)
    {
        if (_objetosRegistrados.Contains(objeto)) _objetosRegistrados.Remove(objeto);
    }
}