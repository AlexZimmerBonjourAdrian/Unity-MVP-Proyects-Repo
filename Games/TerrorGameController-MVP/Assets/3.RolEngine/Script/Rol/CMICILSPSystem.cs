using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RolEngine
{
    /// <summary>
    /// Sistema de rol principal - Mantenible, desacoplado y reutilizable.
    /// Implementa IStatSystem para integración con otros sistemas.
    /// </summary>
    public class CMICILSPSystem : MonoBehaviour, IStatSystem<CMICILSPSystem.Stats>
    {
        //SINGLETON
        private static CMICILSPSystem _instance;

        public static CMICILSPSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject("CMICILSPSystem");
                    _instance = obj.AddComponent<CMICILSPSystem>();
                    DontDestroyOnLoad(obj); 
                }
                return _instance;
            }
        }

        [Header("Configuration")]
        [SerializeField] private int minStatValue = 1;
        [SerializeField] private int maxStatValue = 10;
        [SerializeField] private bool enableNotifications = true;
        [SerializeField] private bool enablePersistence = true;
        [SerializeField] private bool autoLoadOnStart = true;

        [Header("Persistence")]
        [SerializeField] private bool usePersistence = true;

        public StatTemplate CurrentStatsTemplate { get; private set; }

        // Eventos para integración con otros sistemas
        public event Action<StatChangedEvent> OnStatChanged;
        public event Action<TemplateAppliedEvent> OnTemplateApplied;

        // Persistencia (inyección de dependencia)
        private IStatPersistence persistence;

        public StatTemplate GetStatTemplate()
        {
            return CurrentStatsTemplate;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);

            // Inicializar persistencia
            if (usePersistence)
            {
                persistence = new PlayerPrefsStatPersistence();
            }
        }

        private void Start()
        {
            if (autoLoadOnStart && usePersistence && persistence != null && persistence.HasSavedData())
            {
                LoadStats();
            }
        }

    public enum Stats
    {
        Sanity,
        Charm,
        Wits,
        Composure,
        Empathy
    }

     private Dictionary<Stats, int> currentStats = new Dictionary<Stats, int>()
    {
        { Stats.Sanity, 5 },
        { Stats.Charm, 5 },
        { Stats.Wits, 5 },
        { Stats.Composure, 5 },
        { Stats.Empathy, 5 }
    };


    // Constructor para inicializar los atributos (opcional)
    public void InitializeStats(int sa = 5, int ch = 5, int wi = 5, int wil = 5, int em = 5)
    {
        SetStat(Stats.Sanity, sa);
        SetStat(Stats.Charm, ch);
        SetStat(Stats.Wits, wi);
        SetStat(Stats.Composure, wil);
        SetStat(Stats.Empathy, em);
    }

    // [YarnParameter]
    //   [YarnAction]
    //     [YarnFunction]
    //       [YarnNode]
    //         [YarnStateInjector]
    //           [YarnCommand]

           
   public virtual int GetStat(Stats stat)
    {
        return currentStats[stat];
    }

   // [YarnFunction("GetStatByName")]
   //[YarnParameter("statName")] 
    public  virtual  int GetStatByName(string statName)
    {
        // Convertir el nombre de la estadística a su valor enum
        if (System.Enum.TryParse<Stats>(statName, out Stats stat))
        {
            return currentStats[stat];
        }
        else
        {
            Debug.LogError("Stat no encontrada: " + statName);
            return -1; // O cualquier otro valor que indique un error
        }
    }


  // [YarnParameter("statIndex")] 
 
public virtual  int GetStatByIndex(int statIndex)
{
    // Asegurarse de que el índice esté dentro del rango válido
    if (statIndex >= 0 && statIndex < System.Enum.GetValues(typeof(Stats)).Length)
    {
        return currentStats[(Stats)statIndex];
    }
    else
    {
        Debug.LogError("Índice de stat inválido: " + statIndex);
        return -1; 
    }
}

    public virtual  void PrintStats(StatTemplate template)
    {
        Debug.Log("============================");
        Debug.Log("Stats for " + template.Name + ":");

        foreach (var stat in template.BaseStats)
        {
            Debug.Log(stat.Key + ": " + stat.Value);
        }


    }
    public virtual void SetStat(Stats stat, int value)
    {
        int oldValue = currentStats[stat];
        int newValue = Mathf.Clamp(value, minStatValue, maxStatValue);
        
        currentStats[stat] = newValue;

        // Notificar cambios
        if (enableNotifications)
        {
            NotifyStatChanged(stat, oldValue, newValue);
        }
    }

    public virtual void IncreaseStat(Stats stat, int amount)
    {
        // Arreglado: ahora usa SetStat que hace clamp correctamente
        SetStat(stat, currentStats[stat] + amount);
    }

    public virtual void DecreaseStat(Stats stat, int amount)
    {
        SetStat(stat, currentStats[stat] - amount);
    }

    /// <summary>
    /// Verifica si una estadística cumple un requisito mínimo
    /// </summary>
    public bool CheckStatRequirement(Stats stat, int requiredValue)
    {
        int currentValue = GetStat(stat);
        bool passed = currentValue >= requiredValue;

        if (enableNotifications)
        {
            var checkEvent = new StatRequirementCheckEvent(stat.ToString(), requiredValue, currentValue, passed);
            // Puedes agregar notificación aquí si lo necesitas
        }

        return passed;
    }

    /// <summary>
    /// Obtiene todas las estadísticas actuales
    /// </summary>
    public Dictionary<Stats, int> GetAllStats()
    {
        return new Dictionary<Stats, int>(currentStats);
    }

    /// <summary>
    /// Notifica cambios de estadísticas a observers y eventos
    /// </summary>
    private void NotifyStatChanged(Stats stat, int oldValue, int newValue)
    {
        // Notificar a observers específicos
        switch (stat)
        {
            case Stats.Sanity:
                StatObservers.SanityChanged.SetValue(newValue);
                break;
            case Stats.Charm:
                StatObservers.CharmChanged.SetValue(newValue);
                break;
            case Stats.Wits:
                StatObservers.WitsChanged.SetValue(newValue);
                break;
            case Stats.Composure:
                StatObservers.ComposureChanged.SetValue(newValue);
                break;
            case Stats.Empathy:
                StatObservers.EmpathyChanged.SetValue(newValue);
                break;
        }

        // Notificar evento
        var statEvent = new StatChangedEvent(stat.ToString(), oldValue, newValue);
        OnStatChanged?.Invoke(statEvent);
    }

    public  class StatTemplate
    {
        public string Name;
        public Dictionary<Stats, int> BaseStats;

        public StatTemplate(string name, Dictionary<Stats, int> baseStats)
        {
            Name = name;
            BaseStats = baseStats;
        }
    }

    // Plantillas de atributos
    public StatTemplate Detective = new StatTemplate("Detective", new Dictionary<Stats, int>() {
        { Stats.Sanity, 7 },
        { Stats.Charm, 5 },
        { Stats.Wits, 8 },
        { Stats.Composure, 6 },
        { Stats.Empathy, 4 }
    });

    public StatTemplate NinaMimada = new StatTemplate("NiñaMimada", new Dictionary<Stats, int>() {
        { Stats.Sanity, 6 },
        { Stats.Charm, 7 },
        { Stats.Wits, 4 },
        { Stats.Composure, 4 },
        { Stats.Empathy, 6 }
    });

    public StatTemplate HeroinaDeCapaBlanca = new StatTemplate("HeroínadeCapa Blanca", new Dictionary<Stats, int>() {
        { Stats.Sanity, 8 },
        { Stats.Charm, 8 },
        { Stats.Wits, 6 },
        { Stats.Composure, 7 },
        { Stats.Empathy, 8 }
    });

    public StatTemplate LenguaDePlata = new StatTemplate("LenguadePlata", new Dictionary<Stats, int>() {
        { Stats.Sanity, 6 },
        { Stats.Charm, 9 },
        { Stats.Wits, 7 },
        { Stats.Composure, 5 },
        { Stats.Empathy, 6 }
    });

    public StatTemplate FemmeFatale = new StatTemplate("FemmeFatale", new Dictionary<Stats, int>() {
        { Stats.Sanity, 6 },
        { Stats.Charm, 9 },
        { Stats.Wits, 7 },
        { Stats.Composure, 6 },
        { Stats.Empathy, 5 }
    });

    public StatTemplate MonstruoSinCorazon = new StatTemplate("MonstruoSinCorazón", new Dictionary<Stats, int>() {
        { Stats.Sanity, 2 },
        { Stats.Charm, 3 },
        { Stats.Wits, 6 },
        { Stats.Composure, 8 },
        { Stats.Empathy, 2 }
    });

    public StatTemplate LocaPerturbada = new StatTemplate("LocaPerturbada", new Dictionary<Stats, int>() {
        { Stats.Sanity, 1 },
        { Stats.Charm, 4 },
        { Stats.Wits, 7 },
        { Stats.Composure, 3 },
        { Stats.Empathy, 3 }
    });

    public StatTemplate HijaDePolitico = new StatTemplate("HijadePolítico", new Dictionary<Stats, int>() {
        { Stats.Sanity, 7 },
        { Stats.Charm, 8 },
        { Stats.Wits, 6 },
        { Stats.Composure, 7 },
        { Stats.Empathy, 4 }
    });

    /// <summary>
    /// Aplica un template (compatibilidad con código existente)
    /// </summary>
    public virtual void ApplyTemplate(StatTemplate template)
    {
        if (template == null) return;

        CurrentStatsTemplate = template;

        // Initialize currentStats based on the template
        foreach (var stat in template.BaseStats)
        {
            SetStat(stat.Key, stat.Value); // Usa SetStat para notificaciones
        }

        // Notificar cambio de template
        if (enableNotifications)
        {
            Dictionary<string, int> newStats = new Dictionary<string, int>();
            foreach (var stat in template.BaseStats)
            {
                newStats[stat.Key.ToString()] = stat.Value;
            }

            var templateEvent = new TemplateAppliedEvent(template.Name, newStats);
            OnTemplateApplied?.Invoke(templateEvent);
            StatObservers.TemplateChanged.SetValue(template.Name);
        }
    }

    /// <summary>
    /// Aplica un template desde ScriptableObject (nuevo método)
    /// </summary>
    public virtual void ApplyTemplate(StatTemplateSO templateSO)
    {
        if (templateSO == null) return;
        ApplyTemplate(templateSO.ToStatTemplate());
    }

    public virtual StatTemplate GetRandomTemplate()
    {
        // Array with all your templates
        StatTemplate[] templates = new StatTemplate[] { 
            Detective, NinaMimada, HeroinaDeCapaBlanca, 
            LenguaDePlata, FemmeFatale, MonstruoSinCorazon, 
            LocaPerturbada, HijaDePolitico 
        };

        int randomIndex = UnityEngine.Random.Range(0, templates.Length);
        return templates[randomIndex];
    }

    #region Persistence

    /// <summary>
    /// Guarda las estadísticas actuales
    /// </summary>
    public void SaveStats()
    {
        if (!usePersistence || persistence == null) return;

        Dictionary<string, int> statsToSave = new Dictionary<string, int>();
        foreach (var stat in currentStats)
        {
            statsToSave[stat.Key.ToString()] = stat.Value;
        }

        persistence.SaveStats(statsToSave);
    }

    /// <summary>
    /// Carga las estadísticas guardadas
    /// </summary>
    public void LoadStats()
    {
        if (!usePersistence || persistence == null || !persistence.HasSavedData()) return;

        Dictionary<string, int> loadedStats = persistence.LoadStats();

        foreach (var stat in loadedStats)
        {
            if (Enum.TryParse<Stats>(stat.Key, out Stats statEnum))
            {
                SetStat(statEnum, stat.Value);
            }
        }
    }

    /// <summary>
    /// Elimina los datos guardados
    /// </summary>
    public void ClearSavedData()
    {
        if (persistence != null)
        {
            persistence.ClearSavedData();
        }
    }

    #endregion

    #region Lifecycle

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && usePersistence)
        {
            SaveStats();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && usePersistence)
        {
            SaveStats();
        }
    }

    private void OnDestroy()
    {
        if (usePersistence)
        {
            SaveStats();
        }
    }

    #endregion
    }
}


