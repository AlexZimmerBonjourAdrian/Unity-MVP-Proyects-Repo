using UnityEngine;
using System.Collections.Generic;

namespace DeliriumArchne
{
    /// <summary>
    /// GameManager que utiliza el patrón Strategy para gestionar modos de juego.
    /// Permite cambiar entre modos de forma dinámica y extensible.
    /// Diseñado para empezar a crear el juego rápidamente.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Configuración de Modo de Juego")]
        [Tooltip("Modo de juego inicial al empezar")]
        [SerializeField] private GameModeType initialGameMode = GameModeType.Exploration;

        // Estrategia actual (modo de juego activo)
        private IGameModeStrategy currentGameMode;

        // Diccionario de todas las estrategias disponibles
        private Dictionary<GameModeType, IGameModeStrategy> gameModes;

        private void Awake()
        {
            // Singleton pattern
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Inicializar modos de juego
            InitializeGameModes();
        }

        private void Start()
        {
            // Activar modo inicial
            ChangeGameMode(initialGameMode);
        }

        private void Update()
        {
            // Actualizar modo actual
            currentGameMode?.Update();

            // Input para cambiar de modo (solo para testing)
            HandleModeChangeInput();
        }

        /// <summary>
        /// Inicializa todos los modos de juego disponibles
        /// </summary>
        private void InitializeGameModes()
        {
            gameModes = new Dictionary<GameModeType, IGameModeStrategy>
            {
                { GameModeType.Exploration, new ExplorationMode(this) },
                { GameModeType.Horror, new HorrorMode(this) }
            };

            // Inicializar todos los modos
            foreach (var mode in gameModes.Values)
            {
                mode.Initialize();
            }

            Debug.Log($"[GameManager] {gameModes.Count} modos de juego inicializados");
        }

        /// <summary>
        /// Cambia al modo de juego especificado
        /// </summary>
        /// <param name="newMode">Tipo de modo al que cambiar</param>
        public void ChangeGameMode(GameModeType newMode)
        {
            if (!gameModes.ContainsKey(newMode))
            {
                Debug.LogError($"[GameManager] Modo de juego '{newMode}' no encontrado");
                return;
            }

            // Salir del modo actual
            if (currentGameMode != null)
            {
                currentGameMode.OnExit();
            }

            // Cambiar a nuevo modo
            currentGameMode = gameModes[newMode];
            currentGameMode.OnEnter();

            Debug.Log($"[GameManager] Modo cambiado a: {currentGameMode.ModeName}");
        }

        /// <summary>
        /// Obtiene el modo de juego actual
        /// </summary>
        public IGameModeStrategy GetCurrentGameMode()
        {
            return currentGameMode;
        }

        /// <summary>
        /// Obtiene un modo de juego específico sin activarlo
        /// </summary>
        public IGameModeStrategy GetGameMode(GameModeType modeType)
        {
            return gameModes.ContainsKey(modeType) ? gameModes[modeType] : null;
        }

        /// <summary>
        /// Verifica si el modo actual es de terror
        /// </summary>
        public bool IsHorrorMode()
        {
            return currentGameMode is HorrorMode;
        }

        /// <summary>
        /// Verifica si el modo actual es de exploración
        /// </summary>
        public bool IsExplorationMode()
        {
            return currentGameMode is ExplorationMode;
        }

        /// <summary>
        /// Maneja input para cambiar de modo (solo para testing)
        /// </summary>
        private void HandleModeChangeInput()
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChangeGameMode(GameModeType.Exploration);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChangeGameMode(GameModeType.Horror);
            }
            #endif
        }

        private void OnDestroy()
        {
            // Limpiar todos los modos
            if (gameModes != null)
            {
                foreach (var mode in gameModes.Values)
                {
                    mode.Cleanup();
                }
            }
        }
    }

    /// <summary>
    /// Enum que define los tipos de modos de juego disponibles
    /// </summary>
    public enum GameModeType
    {
        Exploration,  // Modo de exploración normal
        Horror        // Modo de terror/peligro
    }
}