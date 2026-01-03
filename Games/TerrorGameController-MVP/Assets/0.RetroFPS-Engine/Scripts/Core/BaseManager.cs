using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Template Method Pattern - Clase base abstracta para todos los managers del juego.
    /// Define el algoritmo general de inicialización y ciclo de vida, permitiendo que
    /// las subclases personalicen pasos específicos sin cambiar la estructura general.
    /// </summary>
    public abstract class BaseManager : MonoBehaviour
    {
        protected bool isInitialized = false;
        protected bool isEnabled = false;
        protected bool isDestroyed = false;

        // ============================================
        // TEMPLATE METHODS - ALGORITMOS PRINCIPALES
        // ============================================

        /// <summary>
        /// Template Method - Algoritmo de inicialización
        /// Define el esqueleto completo del proceso de inicialización.
        /// </summary>
        protected virtual void Awake()
        {
            if (isDestroyed) return;

            LogDebug("Starting initialization...");

            // PASO 1: Validar dependencias críticas
            if (!ValidateDependencies())
            {
                LogError("Dependencies validation failed! Disabling manager.");
                enabled = false;
                return;
            }

            // PASO 2: Inicializar singleton (si aplica)
            InitializeSingleton();

            // PASO 3: Inicialización específica del manager
            OnInitialize();

            // PASO 4: Registrar eventos y observers
            RegisterEvents();

            // PASO 5: Marcar como inicializado
            isInitialized = true;
            LogDebug("Initialization completed successfully");
        }

        /// <summary>
        /// Template Method - Algoritmo de setup inicial
        /// Se ejecuta después de Awake, cuando todos los objetos están inicializados.
        /// </summary>
        protected virtual void Start()
        {
            if (!isInitialized || isDestroyed)
            {
                LogWarning("Cannot start: manager not initialized or destroyed");
                return;
            }

            LogDebug("Starting setup...");

            // PASO 1: Setup específico del manager
            OnSetup();

            // PASO 2: Cargar configuración guardada
            LoadConfiguration();

            // PASO 3: Habilitar funcionalidades
            EnableSystem();

            // PASO 4: Marcar como habilitado
            isEnabled = true;
            LogDebug("Setup completed, manager is now enabled");
        }

        /// <summary>
        /// Template Method - Algoritmo de actualización
        /// Se ejecuta cada frame si el manager está habilitado.
        /// </summary>
        protected virtual void Update()
        {
            if (!isEnabled || !isInitialized || isDestroyed)
                return;

            // PASO 1: Actualización específica del manager
            OnUpdate();

            // PASO 2: Verificaciones periódicas (si corresponde)
            if (ShouldRunPeriodicChecks())
            {
                PerformPeriodicChecks();
            }
        }

        /// <summary>
        /// Template Method - Algoritmo de limpieza
        /// Se ejecuta cuando el objeto es destruido.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (isDestroyed) return;
            isDestroyed = true;

            LogDebug("Starting cleanup...");

            // PASO 1: Deshabilitar sistema
            if (isEnabled)
            {
                DisableSystem();
            }

            // PASO 2: Desregistrar eventos y observers
            UnregisterEvents();

            // PASO 3: Limpieza específica del manager
            OnCleanup();

            // PASO 4: Limpiar singleton (si aplica)
            CleanupSingleton();

            LogDebug("Cleanup completed");
        }

        // ============================================
        // MÉTODOS ABSTRACTOS - DEBEN IMPLEMENTARSE
        // ============================================

        /// <summary>
        /// Inicialización específica del manager.
        /// Aquí va la lógica particular de setup inicial.
        /// </summary>
        protected abstract void OnInitialize();

        /// <summary>
        /// Setup específico del manager.
        /// Se ejecuta después de que todos los objetos están inicializados.
        /// </summary>
        protected abstract void OnSetup();

        /// <summary>
        /// Actualización específica del manager.
        /// Se ejecuta cada frame si el manager está activo.
        /// </summary>
        protected abstract void OnUpdate();

        /// <summary>
        /// Limpieza específica del manager.
        /// Se ejecuta al destruir el objeto.
        /// </summary>
        protected abstract void OnCleanup();

        // ============================================
        // MÉTODOS VIRTUALES - PUEDEN SOBRESCRIBIRSE
        // ============================================

        /// <summary>
        /// Valida dependencias críticas del manager.
        /// Retorna false si faltan dependencias esenciales.
        /// </summary>
        protected virtual bool ValidateDependencies()
        {
            // Por defecto, no requiere dependencias específicas
            return true;
        }

        /// <summary>
        /// Inicializa el patrón Singleton (si aplica).
        /// Sobrescribir en managers que usen Singleton.
        /// </summary>
        protected virtual void InitializeSingleton()
        {
            // Implementación vacía por defecto
        }

        /// <summary>
        /// Limpia referencias del patrón Singleton (si aplica).
        /// Sobrescribir en managers que usen Singleton.
        /// </summary>
        protected virtual void CleanupSingleton()
        {
            // Implementación vacía por defecto
        }

        /// <summary>
        /// Registra eventos y observers.
        /// Sobrescribir para suscribirse a eventos del sistema.
        /// </summary>
        protected virtual void RegisterEvents()
        {
            // Implementación vacía por defecto
        }

        /// <summary>
        /// Desregistra eventos y observers.
        /// Sobrescribir para desuscribirse de eventos del sistema.
        /// </summary>
        protected virtual void UnregisterEvents()
        {
            // Implementación vacía por defecto
        }

        /// <summary>
        /// Carga configuración guardada (PlayerPrefs, archivos, etc.).
        /// Sobrescribir para cargar estado persistente.
        /// </summary>
        protected virtual void LoadConfiguration()
        {
            // Implementación vacía por defecto
        }

        /// <summary>
        /// Habilita el sistema y sus funcionalidades.
        /// Sobrescribir para activar componentes específicos.
        /// </summary>
        protected virtual void EnableSystem()
        {
            // Implementación vacía por defecto
        }

        /// <summary>
        /// Deshabilita el sistema y sus funcionalidades.
        /// Sobrescribir para desactivar componentes específicos.
        /// </summary>
        protected virtual void DisableSystem()
        {
            // Implementación vacía por defecto
        }

        /// <summary>
        /// Determina si se deben ejecutar verificaciones periódicas.
        /// Retorna true si se deben hacer checks adicionales cada frame.
        /// </summary>
        protected virtual bool ShouldRunPeriodicChecks()
        {
            return false; // Por defecto, no ejecutar verificaciones periódicas
        }

        /// <summary>
        /// Realiza verificaciones periódicas del estado del manager.
        /// Sobrescribir para validaciones específicas (conexiones, estado, etc.).
        /// </summary>
        protected virtual void PerformPeriodicChecks()
        {
            // Implementación vacía por defecto
        }

        // ============================================
        // MÉTODOS PÚBLICOS DE UTILIDAD
        // ============================================

        /// <summary>
        /// Verifica si el manager está completamente inicializado.
        /// </summary>
        public bool IsInitialized => isInitialized;

        /// <summary>
        /// Verifica si el manager está habilitado y funcional.
        /// </summary>
        public bool IsEnabled => isEnabled && isInitialized && !isDestroyed;

        /// <summary>
        /// Fuerza la reinicialización del manager.
        /// Útil para testing o cambios de configuración.
        /// </summary>
        public virtual void Reinitialize()
        {
            if (isDestroyed)
            {
                LogWarning("Cannot reinitialize: manager is destroyed");
                return;
            }

            LogDebug("Forcing reinitialization...");

            // Limpiar estado anterior
            OnCleanup();

            // Resetear flags
            isInitialized = false;
            isEnabled = false;

            // Re-ejecutar inicialización
            Awake();
            if (isInitialized)
            {
                Start();
            }
        }

        /// <summary>
        /// Obtiene información detallada de debug del manager.
        /// </summary>
        public virtual string GetDebugInfo()
        {
            return $"{GetType().Name} Debug Info:\n" +
                   $"- Initialized: {isInitialized}\n" +
                   $"- Enabled: {isEnabled}\n" +
                   $"- Destroyed: {isDestroyed}\n" +
                   $"- Active: {gameObject.activeSelf}\n" +
                   $"- GameObject Active: {gameObject.activeInHierarchy}";
        }

        // ============================================
        // MÉTODOS DE LOGGING UNIFICADOS
        // ============================================

        protected void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[{GetType().Name}] {message}");
#endif
        }

        protected void LogWarning(string message)
        {
            Debug.LogWarning($"[{GetType().Name}] {message}");
        }

        protected void LogError(string message)
        {
            Debug.LogError($"[{GetType().Name}] {message}");
        }

        // ============================================
        // EXTENSIONES PARA SUBCLASES
        // ============================================

        /// <summary>
        /// Método helper para validar referencias críticas.
        /// </summary>
        protected bool ValidateReference(Object obj, string name)
        {
            if (obj == null)
            {
                LogError($"Missing reference: {name}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Método helper para validar componentes requeridos.
        /// </summary>
        protected bool ValidateComponent<T>(GameObject obj, string description) where T : Component
        {
            if (obj == null)
            {
                LogError($"Cannot validate component: GameObject is null ({description})");
                return false;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
            {
                LogError($"Missing component {typeof(T).Name} on {obj.name} ({description})");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Método helper para crear objetos hijos organizados.
        /// </summary>
        protected GameObject CreateChildObject(string name, Transform parent = null)
        {
            if (parent == null)
                parent = transform;

            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

            return obj;
        }
    }
}
