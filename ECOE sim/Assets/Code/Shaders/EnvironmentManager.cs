using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

[ExecuteInEditMode]
public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance;

    [Header("Paleta de Colores Arquitectura")]
    public Color colorClinica = new Color(0.1f, 0.2f, 0.5f);
    public Color colorPasillo = new Color(0.1f, 0.5f, 0.4f);

    [Header("Simulación: Limpieza e Higiene")]
    [Range(0, 2)] public float multiplicadorBrilloSuelo = 1.0f; // _GlobalSmoothness

    [Header("Simulación: Estado de Alerta (Examen)")]
    [Range(0, 1)] public float nivelDeEstres = 0f; // Controla Vignette y Color Filter
    public bool modoAlertaActivo = false;

    [Header("Post-Procesado")]
    public PostProcessVolume volumenGlobal;

    // IDs de Shaders (Más rápido que usar strings)
    private static readonly int IDSmoothness = Shader.PropertyToID("_GlobalSmoothnessMultiplier");
    private static readonly int IDColorArquitectura = Shader.PropertyToID("_GlobalColorArquitecture");

    // Lista persistente de objetos
    private List<ModularColorTint> _objetosRegistrados = new List<ModularColorTint>();
    
    // El "Dirty Flag"
    private bool _isDirty = false;

    void OnEnable() {
        Instance = this;
        MarcarComoSucio();
    }

    // Método para que los objetos se registren (Invocado desde OnEnable de la pared)
    public void RegistrarObjeto(ModularColorTint objeto) {
        if (!_objetosRegistrados.Contains(objeto)) {
            _objetosRegistrados.Add(objeto);
            objeto.UpdateColor(); // Actualiza al aparecer
        }
    }

    void OnValidate() { MarcarComoSucio(); }

    public void MarcarComoSucio() { _isDirty = true; }

    void Update() {
        if (_isDirty) {
            ActualizarTodo();
            _isDirty = false;
        }

        // Efecto dinámico de latido si está en alerta (Feedback visual ECOE)
        if (modoAlertaActivo) {
            SimularLatidoEstres();
        }
    }

    private void ActualizarTodo() {
        // 1. Enviar datos globales a la GPU (Inyección de propiedades)
        Shader.SetGlobalFloat(IDSmoothness, multiplicadorBrilloSuelo);

        // 2. Actualizar objetos registrados (Paredes)
        for (int i = _objetosRegistrados.Count - 1; i >= 0; i--) {
            if (_objetosRegistrados[i] != null) 
                _objetosRegistrados[i].UpdateColor();
            else 
                _objetosRegistrados.RemoveAt(i); // Limpieza de lista si el objeto se borró
        }

        // 3. Actualizar Post-Procesado
        ActualizarPostProcesado();
    }

    private void ActualizarPostProcesado() {
        if (volumenGlobal == null || volumenGlobal.profile == null) return;

        if (volumenGlobal.profile.TryGetSettings(out Vignette vignette)) {
            vignette.intensity.value = Mathf.Lerp(0.25f, 0.5f, nivelDeEstres);
        }
        
        if (volumenGlobal.profile.TryGetSettings(out ColorGrading grading)) {
            // Si hay estrés, desaturamos un poco la escena (visión de túnel)
            grading.saturation.value = Mathf.Lerp(0, -30, nivelDeEstres);
        }
    }

    private void SimularLatidoEstres() {
        if (volumenGlobal.profile.TryGetSettings(out Vignette vig)) {
            float pulso = (Mathf.Sin(Time.time * 5f) + 1f) / 2f;
            vig.intensity.value = 0.4f + (pulso * 0.15f);
        }
    }
}