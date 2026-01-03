using UnityEngine;

namespace DeliriumArchne
{
    /// <summary>
    /// Clase base genérica con comportamiento común para todos los modos de juego.
    /// Proporciona funcionalidad base que puede ser extendida por modos específicos.
    /// </summary>
    public abstract class GameModeStrategyBase : IGameModeStrategy
    {
        protected GameManager gameManager;
        protected bool isInitialized = false;
        protected bool isActive = false;

        public abstract string ModeName { get; }

        public GameModeStrategyBase(GameManager manager)
        {
            gameManager = manager;
        }

        public virtual void Initialize()
        {
            if (isInitialized)
            {
                Debug.LogWarning($"[{ModeName}] Ya está inicializado");
                return;
            }

            OnInitialize();
            isInitialized = true;
        }

        /// <summary>
        /// Método virtual para inicialización específica del modo
        /// </summary>
        protected virtual void OnInitialize()
        {
            // Implementación específica en clases derivadas
        }

        public virtual void Update()
        {
            if (!isActive || !isInitialized)
                return;

            OnUpdate();
        }

        /// <summary>
        /// Método virtual para actualización específica del modo
        /// </summary>
        protected virtual void OnUpdate()
        {
            // Implementación específica en clases derivadas
        }

        public virtual void OnEnter()
        {
            if (!isInitialized)
            {
                Initialize();
            }

            if (isActive)
            {
                Debug.LogWarning($"[{ModeName}] Ya está activo");
                return;
            }

            isActive = true;
            OnEnterMode();
        }

        /// <summary>
        /// Método virtual para entrada específica del modo
        /// </summary>
        protected virtual void OnEnterMode()
        {
            // Implementación específica en clases derivadas
        }

        public virtual void OnExit()
        {
            if (!isActive)
            {
                Debug.LogWarning($"[{ModeName}] No está activo");
                return;
            }

            isActive = false;
            OnExitMode();
        }

        /// <summary>
        /// Método virtual para salida específica del modo
        /// </summary>
        protected virtual void OnExitMode()
        {
            // Implementación específica en clases derivadas
        }

        public virtual void Cleanup()
        {
            if (isActive)
            {
                OnExit();
            }

            OnCleanup();
            isInitialized = false;
        }

        /// <summary>
        /// Método virtual para limpieza específica del modo
        /// </summary>
        protected virtual void OnCleanup()
        {
            // Implementación específica en clases derivadas
        }
    }
}
