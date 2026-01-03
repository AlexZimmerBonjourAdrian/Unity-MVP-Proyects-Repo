using UnityEngine;
using HorrorEngine;

namespace DeliriumArchne
{
    /// <summary>
    /// Modo de Exploración: El jugador explora el entorno de forma segura.
    /// Modo por defecto del juego de terror.
    /// </summary>
    public class ExplorationMode : GameModeStrategyBase
    {
        public override string ModeName => "Exploración";

        private HorrorEngine.CInteractRayCast interactSystem;
        private HorrorEngine.CManagerSFX sfxManager;
        private HorrorEngine.CLevelManager levelManager;

        public ExplorationMode(GameManager manager) : base(manager) { }

        protected override void OnInitialize()
        {
            Debug.Log("[Exploración] Modo inicializado");
            InitializeExplorationSystems();
        }

        private void InitializeExplorationSystems()
        {
            // Buscar o crear sistemas de HorrorEngine
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                interactSystem = player.GetComponent<HorrorEngine.CInteractRayCast>();
                if (interactSystem == null)
                {
                    interactSystem = player.AddComponent<HorrorEngine.CInteractRayCast>();
                }
            }

            // Inicializar managers si no existen
            sfxManager = HorrorEngine.CManagerSFX.Inst;
            levelManager = HorrorEngine.CLevelManager.Inst;

            Debug.Log("[Exploración] Sistemas de HorrorEngine inicializados");
        }

        protected override void OnEnterMode()
        {
            Debug.Log("[Exploración] Activando modo de exploración");

            // Habilitar interacciones
            if (interactSystem != null)
            {
                interactSystem.SetInteractionsEnabled(true);
                Debug.Log("[Exploración] Sistema de interacciones activado");
            }

            // Configurar música ambiental
            if (sfxManager != null)
            {
                // sfxManager.PlayAmbientMusic();
                Debug.Log("[Exploración] Música ambiental configurada");
            }

            // Configurar controles
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Debug.Log("[Exploración] Modo de exploración completamente activado");
        }

        protected override void OnUpdate()
        {
            // Verificar triggers para cambiar a modo terror
            CheckForHorrorTriggers();

            // Aquí se pueden agregar otras lógicas de exploración
        }

        private void CheckForHorrorTriggers()
        {
            // Ejemplo: Detectar presencia de enemigos o eventos específicos
            // que podrían cambiar al modo terror

            // Por ahora, podemos agregar una tecla de debug para testing
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gameManager.ChangeGameMode(GameModeType.Horror);
            }
            #endif
        }

        protected override void OnExitMode()
        {
            Debug.Log("[Exploración] Desactivando modo de exploración");

            if (interactSystem != null)
            {
                interactSystem.SetInteractionsEnabled(false);
                Debug.Log("[Exploración] Sistema de interacciones desactivado");
            }

            // Detener música ambiental si es necesario
            // if (sfxManager != null)
            // {
            //     sfxManager.StopAmbientMusic();
            // }
        }

        protected override void OnCleanup()
        {
            Debug.Log("[Exploración] Limpiando recursos");

            // Limpiar referencias
            interactSystem = null;
            sfxManager = null;
            levelManager = null;
        }
    }
}
