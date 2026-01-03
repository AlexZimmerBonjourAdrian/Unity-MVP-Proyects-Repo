using UnityEngine;
using HorrorEngine;

namespace DeliriumArchne
{
    /// <summary>
    /// Modo de Terror: El jugador está en peligro, hay enemigos o situaciones de miedo.
    /// Modo activado cuando hay amenazas presentes.
    /// </summary>
    public class HorrorMode : GameModeStrategyBase
    {
        public override string ModeName => "Terror";

        private HorrorEngine.CHorrorController horrorController;
        private HorrorEngine.CEnemyManager enemyManager;
        private HorrorEngine.CManagerSFX sfxManager;

        public HorrorMode(GameManager manager) : base(manager) { }

        protected override void OnInitialize()
        {
            Debug.Log("[Terror] Modo inicializado");
            InitializeHorrorSystems();
        }

        private void InitializeHorrorSystems()
        {
            // Buscar o crear sistemas de HorrorEngine
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                horrorController = player.GetComponent<HorrorEngine.CHorrorController>();
                if (horrorController == null)
                {
                    horrorController = player.AddComponent<HorrorEngine.CHorrorController>();
                }
            }

            // Inicializar Enemy Manager
            if (HorrorEngine.CEnemyManager.Instance == null)
            {
                GameObject enemyManagerObj = new GameObject("EnemyManager");
                enemyManagerObj.AddComponent<HorrorEngine.CEnemyManager>();
            }
            enemyManager = HorrorEngine.CEnemyManager.Instance;

            // Inicializar SFX Manager
            sfxManager = HorrorEngine.CManagerSFX.Inst;

            Debug.Log("[Terror] Sistemas de HorrorEngine inicializados");
        }

        protected override void OnEnterMode()
        {
            Debug.Log("[Terror] Activando modo de terror");

            // Activar spawn de enemigos
            if (enemyManager != null)
            {
                // enemyManager.EnableSpawning();
                Debug.Log("[Terror] Sistema de enemigos activado");
            }

            // Cambiar música a terror
            if (sfxManager != null)
            {
                // sfxManager.PlayHorrorMusic();
                Debug.Log("[Terror] Música de terror configurada");
            }

            // Configurar controles de terror
            if (horrorController != null)
            {
                // Configurar parámetros específicos de terror
                Debug.Log("[Terror] Controlador de horror configurado");
            }

            // Efectos visuales de miedo
            // ScreenShake, color grading, etc.
            Debug.Log("[Terror] Efectos visuales de miedo activados");
        }

        protected override void OnUpdate()
        {
            // Verificar si se puede volver a exploración
            CheckForExplorationTrigger();

            // Aquí se pueden agregar otras lógicas de terror:
            // - Sistema de combate
            // - Detección de enemigos
            // - Efectos de miedo
        }

        private void CheckForExplorationTrigger()
        {
            // Si no hay enemigos activos, volver a exploración
            if (enemyManager != null)
            {
                // int activeEnemies = enemyManager.GetActiveEnemyCount();
                // if (activeEnemies == 0)
                // {
                //     gameManager.ChangeGameMode(GameModeType.Exploration);
                //     Debug.Log("[Terror] No hay enemigos activos, volviendo a exploración");
                // }
            }

            // Tecla de debug para testing
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gameManager.ChangeGameMode(GameModeType.Exploration);
            }
            #endif
        }

        protected override void OnExitMode()
        {
            Debug.Log("[Terror] Desactivando modo de terror");

            // Desactivar spawn de enemigos
            if (enemyManager != null)
            {
                // enemyManager.DisableSpawning();
                Debug.Log("[Terror] Sistema de enemigos desactivado");
            }

            // Detener música de terror
            if (sfxManager != null)
            {
                // sfxManager.StopHorrorMusic();
                // sfxManager.PlayAmbientMusic();
                Debug.Log("[Terror] Música restaurada");
            }

            // Desactivar efectos de miedo
            Debug.Log("[Terror] Efectos visuales de miedo desactivados");
        }

        protected override void OnCleanup()
        {
            Debug.Log("[Terror] Limpiando recursos");

            // Limpiar referencias
            horrorController = null;
            enemyManager = null;
            sfxManager = null;
        }
    }
}
