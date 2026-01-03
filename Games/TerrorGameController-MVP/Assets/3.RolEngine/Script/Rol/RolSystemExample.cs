using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RolEngine;

/// <summary>
/// Clase de ejemplo que demuestra cómo usar el sistema de rol CMICILSPSystem.
/// Muestra diferentes casos de uso y patrones comunes.
/// </summary>
public class RolSystemExample : MonoBehaviour
{
    [Header("Configuración de Ejemplo")]
    [SerializeField] private bool autoInitializeOnStart = true;
    [SerializeField] private CMICILSPSystem.StatTemplate initialTemplate;

    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;

    private void Start()
    {
        if (autoInitializeOnStart)
        {
            InitializeExample();
        }
    }

    /// <summary>
    /// Ejemplo 1: Inicialización básica del sistema
    /// </summary>
    public void InitializeExample()
    {
        if (showDebugLogs)
            Debug.Log("=== EJEMPLO 1: Inicialización Básica ===");

        // Obtener la instancia del sistema (Singleton)
        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;

        // Opción A: Inicializar con valores personalizados
        rolSystem.InitializeStats(sa: 7, ch: 6, wi: 8, wil: 5, em: 6);
        if (showDebugLogs)
            Debug.Log("Stats inicializados manualmente");

        // Opción B: Aplicar un template predefinido
        rolSystem.ApplyTemplate(rolSystem.Detective);
        if (showDebugLogs)
            Debug.Log($"Template aplicado: {rolSystem.CurrentStatsTemplate.Name}");

        // Mostrar todas las stats
        ShowAllStats();
    }

    /// <summary>
    /// Ejemplo 2: Leer estadísticas del sistema
    /// </summary>
    public void ShowAllStats()
    {
        if (showDebugLogs)
            Debug.Log("=== EJEMPLO 2: Lectura de Stats ===");

        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;

        // Método 1: Obtener stat por enum
        int sanity = rolSystem.GetStat(CMICILSPSystem.Stats.Sanity);
        int charm = rolSystem.GetStat(CMICILSPSystem.Stats.Charm);
        int wits = rolSystem.GetStat(CMICILSPSystem.Stats.Wits);
        int composure = rolSystem.GetStat(CMICILSPSystem.Stats.Composure);
        int empathy = rolSystem.GetStat(CMICILSPSystem.Stats.Empathy);

        if (showDebugLogs)
        {
            Debug.Log($"Sanity: {sanity}");
            Debug.Log($"Charm: {charm}");
            Debug.Log($"Wits: {wits}");
            Debug.Log($"Composure: {composure}");
            Debug.Log($"Empathy: {empathy}");
        }

        // Método 2: Obtener stat por nombre (string)
        int sanityByName = rolSystem.GetStatByName("Sanity");
        if (showDebugLogs)
            Debug.Log($"Sanity (por nombre): {sanityByName}");

        // Método 3: Obtener stat por índice
        int charmByIndex = rolSystem.GetStatByIndex(1); // Charm es el índice 1
        if (showDebugLogs)
            Debug.Log($"Charm (por índice): {charmByIndex}");

        // Método 4: Obtener el template actual
        CMICILSPSystem.StatTemplate currentTemplate = rolSystem.GetStatTemplate();
        if (showDebugLogs && currentTemplate != null)
        {
            Debug.Log($"Template actual: {currentTemplate.Name}");
            rolSystem.PrintStats(currentTemplate);
        }
    }

    /// <summary>
    /// Ejemplo 3: Modificar estadísticas
    /// </summary>
    public void ModifyStatsExample()
    {
        if (showDebugLogs)
            Debug.Log("=== EJEMPLO 3: Modificación de Stats ===");

        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;

        // Aumentar una stat
        int oldCharm = rolSystem.GetStat(CMICILSPSystem.Stats.Charm);
        rolSystem.IncreaseStat(CMICILSPSystem.Stats.Charm, 2);
        int newCharm = rolSystem.GetStat(CMICILSPSystem.Stats.Charm);
        if (showDebugLogs)
            Debug.Log($"Charm aumentado: {oldCharm} -> {newCharm}");

        // Disminuir una stat
        int oldSanity = rolSystem.GetStat(CMICILSPSystem.Stats.Sanity);
        rolSystem.DecreaseStat(CMICILSPSystem.Stats.Sanity, 1);
        int newSanity = rolSystem.GetStat(CMICILSPSystem.Stats.Sanity);
        if (showDebugLogs)
            Debug.Log($"Sanity disminuido: {oldSanity} -> {newSanity}");

        // Establecer un valor específico
        rolSystem.SetStat(CMICILSPSystem.Stats.Wits, 9);
        if (showDebugLogs)
            Debug.Log($"Wits establecido a: {rolSystem.GetStat(CMICILSPSystem.Stats.Wits)}");
    }

    /// <summary>
    /// Ejemplo 4: Aplicar diferentes templates
    /// </summary>
    public void ApplyTemplatesExample()
    {
        if (showDebugLogs)
            Debug.Log("=== EJEMPLO 4: Aplicar Templates ===");

        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;

        // Aplicar template específico
        rolSystem.ApplyTemplate(rolSystem.FemmeFatale);
        if (showDebugLogs)
            Debug.Log($"Template aplicado: {rolSystem.CurrentStatsTemplate.Name}");

        // Aplicar template aleatorio
        CMICILSPSystem.StatTemplate randomTemplate = rolSystem.GetRandomTemplate();
        rolSystem.ApplyTemplate(randomTemplate);
        if (showDebugLogs)
            Debug.Log($"Template aleatorio aplicado: {randomTemplate.Name}");
    }

    /// <summary>
    /// Ejemplo 5: Sistema de checks de stats (para diálogos/decisiones)
    /// </summary>
    public bool CheckStatRequirement(CMICILSPSystem.Stats stat, int requiredValue)
    {
        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;
        int currentValue = rolSystem.GetStat(stat);
        bool passed = currentValue >= requiredValue;

        if (showDebugLogs)
        {
            Debug.Log($"=== Check de {stat} ===");
            Debug.Log($"Valor requerido: {requiredValue}");
            Debug.Log($"Valor actual: {currentValue}");
            Debug.Log($"Resultado: {(passed ? "ÉXITO" : "FALLO")}");
        }

        return passed;
    }

    /// <summary>
    /// Ejemplo 6: Sistema de decisiones basadas en stats
    /// </summary>
    public string MakeDecisionBasedOnStats()
    {
        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;

        int charm = rolSystem.GetStat(CMICILSPSystem.Stats.Charm);
        int wits = rolSystem.GetStat(CMICILSPSystem.Stats.Wits);
        int empathy = rolSystem.GetStat(CMICILSPSystem.Stats.Empathy);

        if (showDebugLogs)
            Debug.Log("=== EJEMPLO 6: Decisión basada en Stats ===");

        // Lógica de decisión basada en las stats más altas
        if (charm >= wits && charm >= empathy)
        {
            if (showDebugLogs)
                Debug.Log("Decisión: Usar carisma para convencer");
            return "Charm";
        }
        else if (wits >= empathy)
        {
            if (showDebugLogs)
                Debug.Log("Decisión: Usar astucia para resolver");
            return "Wits";
        }
        else
        {
            if (showDebugLogs)
                Debug.Log("Decisión: Usar empatía para entender");
            return "Empathy";
        }
    }

    /// <summary>
    /// Ejemplo 7: Sistema de eventos que modifican stats
    /// </summary>
    public void HandleEventExample(string eventType)
    {
        if (showDebugLogs)
            Debug.Log($"=== EJEMPLO 7: Manejo de Evento: {eventType} ===");

        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;

        switch (eventType)
        {
            case "Trauma":
                // Evento traumático reduce Sanity y Composure
                rolSystem.DecreaseStat(CMICILSPSystem.Stats.Sanity, 2);
                rolSystem.DecreaseStat(CMICILSPSystem.Stats.Composure, 1);
                if (showDebugLogs)
                    Debug.Log("Evento traumático: Sanity -2, Composure -1");
                break;

            case "SocialSuccess":
                // Éxito social aumenta Charm y Empathy
                rolSystem.IncreaseStat(CMICILSPSystem.Stats.Charm, 1);
                rolSystem.IncreaseStat(CMICILSPSystem.Stats.Empathy, 1);
                if (showDebugLogs)
                    Debug.Log("Éxito social: Charm +1, Empathy +1");
                break;

            case "IntellectualBreakthrough":
                // Descubrimiento intelectual aumenta Wits
                rolSystem.IncreaseStat(CMICILSPSystem.Stats.Wits, 2);
                if (showDebugLogs)
                    Debug.Log("Descubrimiento intelectual: Wits +2");
                break;

            default:
                if (showDebugLogs)
                    Debug.LogWarning($"Tipo de evento desconocido: {eventType}");
                break;
        }
    }

    /// <summary>
    /// Ejemplo 8: Comparar stats entre diferentes templates
    /// </summary>
    public void CompareTemplatesExample()
    {
        if (showDebugLogs)
            Debug.Log("=== EJEMPLO 8: Comparar Templates ===");

        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;

        // Comparar Detective vs FemmeFatale
        int detectiveCharm = rolSystem.Detective.BaseStats[CMICILSPSystem.Stats.Charm];
        int femmeFataleCharm = rolSystem.FemmeFatale.BaseStats[CMICILSPSystem.Stats.Charm];

        if (showDebugLogs)
        {
            Debug.Log($"Detective Charm: {detectiveCharm}");
            Debug.Log($"FemmeFatale Charm: {femmeFataleCharm}");
            Debug.Log($"Diferencia: {femmeFataleCharm - detectiveCharm}");
        }
    }

    // ============================================
    // MÉTODOS DE TESTING (para usar en Update o con inputs)
    // ============================================

    private void Update()
    {
        // Ejemplos de testing con teclas (solo para desarrollo)
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitializeExample();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ShowAllStats();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ModifyStatsExample();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ApplyTemplatesExample();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CheckStatRequirement(CMICILSPSystem.Stats.Charm, 7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            MakeDecisionBasedOnStats();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            HandleEventExample("Trauma");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CompareTemplatesExample();
        }
        #endif
    }

    // ============================================
    // MÉTODOS PÚBLICOS PARA USO EXTERNO
    // ============================================

    /// <summary>
    /// Obtiene un resumen completo del estado actual del sistema
    /// </summary>
    public string GetSystemSummary()
    {
        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;
        CMICILSPSystem.StatTemplate template = rolSystem.CurrentStatsTemplate;

        string summary = "=== RESUMEN DEL SISTEMA DE ROL ===\n";
        summary += $"Template Actual: {(template != null ? template.Name : "Ninguno")}\n\n";
        summary += "Estadísticas Actuales:\n";
        summary += $"  - Sanity: {rolSystem.GetStat(CMICILSPSystem.Stats.Sanity)}\n";
        summary += $"  - Charm: {rolSystem.GetStat(CMICILSPSystem.Stats.Charm)}\n";
        summary += $"  - Wits: {rolSystem.GetStat(CMICILSPSystem.Stats.Wits)}\n";
        summary += $"  - Composure: {rolSystem.GetStat(CMICILSPSystem.Stats.Composure)}\n";
        summary += $"  - Empathy: {rolSystem.GetStat(CMICILSPSystem.Stats.Empathy)}\n";

        return summary;
    }

    /// <summary>
    /// Verifica si el jugador puede realizar una acción basada en múltiples stats
    /// </summary>
    public bool CanPerformAction(Dictionary<CMICILSPSystem.Stats, int> requirements)
    {
        CMICILSPSystem rolSystem = CMICILSPSystem.Instance;

        foreach (var requirement in requirements)
        {
            int currentValue = rolSystem.GetStat(requirement.Key);
            if (currentValue < requirement.Value)
            {
                if (showDebugLogs)
                    Debug.Log($"No se puede realizar acción: {requirement.Key} insuficiente ({currentValue} < {requirement.Value})");
                return false;
            }
        }

        if (showDebugLogs)
            Debug.Log("Acción permitida: todos los requisitos cumplidos");
        return true;
    }
}

