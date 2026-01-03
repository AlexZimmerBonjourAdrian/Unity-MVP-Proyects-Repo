// ========================================================================
// EJEMPLO DE USO COMPLETO - Retro FPS Engine
// ========================================================================

using UnityEngine;
using RetroFPS;
using System.Threading.Tasks;

namespace RetroFPS
{
    /// <summary>
    /// Ejemplo completo de cómo usar todos los sistemas del motor RetroFPS
    /// </summary>
    public class GameManagerExample : MonoBehaviour
    {
        private async void Start()
        {
            // 1. INICIALIZACIÓN DE SISTEMAS
            InitializeSystems();

            // 2. CARGAR ASSETS CON ADDRESSABLES
            await LoadGameAssets();

            // 3. MOSTRAR INTRO DIALOGUE
            ShowIntroDialogue();

            // 4. SPAWNEAR ENEMIGOS
            await SpawnInitialEnemies();

            // 5. CONFIGURAR AUDIO
            SetupAudio();
        }

        private void InitializeSystems()
        {
            // Verificar que todos los managers existan
            if (CAssetManager.Instance == null)
                Debug.LogError("AssetManager no encontrado en escena");

            if (CDialogueManager.Instance == null)
                Debug.LogError("DialogueManager no encontrado en escena");

            Debug.Log("✅ Sistemas inicializados correctamente");
        }

        private async Task LoadGameAssets()
        {
            Debug.Log("📦 Cargando assets del juego...");

            // Cargar assets críticos (síncronos para inicio)
            GameObject playerPrefab = CAssetManager.Instance.LoadAsset<GameObject>("Player");
            GameObject hudPrefab = CAssetManager.Instance.LoadAsset<GameObject>("HUD");

            // Instanciar elementos críticos
            if (playerPrefab != null)
                Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

            if (hudPrefab != null)
                Instantiate(hudPrefab, Vector3.zero, Quaternion.identity);

            // Cargar assets opcionales (asíncronos)
            await CAssetManager.Instance.LoadAssetAsync<GameObject>("Enemy_Basic");
            await CAssetManager.Instance.LoadAssetAsync<GameObject>("Weapon_Pistol");

            Debug.Log("✅ Assets cargados correctamente");
        }

        private void ShowIntroDialogue()
        {
            var introDialogue = new DialogueData
            {
                speakerName = "Narrador",
                message = "Bienvenido a Retro FPS. La base está bajo ataque. " +
                         "Encuentra armas y elimina a todos los enemigos.",
                choices = new string[] { "¡Entendido!", "Mostrar tutorial" }
            };

            CDialogueManager.Instance.ShowDialogueWithOptions(introDialogue, OnIntroChoice);
        }

        private void OnIntroChoice(int choice)
        {
            if (choice == 1) // Mostrar tutorial
            {
                var tutorialDialogue = new DialogueData
                {
                    speakerName = "Tutorial",
                    message = "Controles básicos:\n" +
                             "• WASD: Moverse\n" +
                             "• Mouse: Apuntar\n" +
                             "• Click Izquierdo: Disparar\n" +
                             "• Espacio: Saltar"
                };

                CDialogueManager.Instance.ShowDialogue(tutorialDialogue);
            }

            // Continuar con el juego
            StartGame();
        }

        private async Task SpawnInitialEnemies()
        {
            Debug.Log("👾 Spawneando enemigos iniciales...");

            // Spawn básico de enemigos
            for (int i = 0; i < 3; i++)
            {
                Vector3 spawnPos = new Vector3(i * 5f, 0f, 10f);
                await CAssetManager.Instance.InstantiateAssetAsync("Enemy_Basic", spawnPos);
                await Task.Delay(500); // Pequeño delay entre spawns
            }

            Debug.Log("✅ Enemigos spawnados");
        }

        private void SetupAudio()
        {
            // Configurar música de fondo
            CManagerMusic.Inst.PlayMusicBackground(0);

            // Configurar efectos de sonido
            CManagerSFX.Inst.PlaySound(0); // Sonido de inicio
        }

        private void StartGame()
        {
            Debug.Log("🎮 ¡Juego iniciado!");

            // Aquí iría la lógica para empezar el gameplay
            // - Habilitar controles del jugador
            // - Iniciar timer de nivel
            // - Activar spawners de enemigos
            // - etc.
        }

        // ========================================================================
        // EJEMPLOS ADICIONALES DE USO
        // ========================================================================

        /// <summary>
        /// Ejemplo: Spawn de enemigo con configuración
        /// </summary>
        public async Task SpawnConfiguredEnemy(string enemyType, Vector3 position, int difficulty = 1)
        {
            GameObject enemy = await CAssetManager.Instance.InstantiateAssetAsync(enemyType, position);

            if (enemy != null)
            {
                // Configurar enemigo según dificultad
                // Ejemplo: Obtener componente de enemigo y configurarlo
                // var enemyController = enemy.GetComponent<EnemyController>();
                // if (enemyController != null)
                // {
                //     // Configurar propiedades según la dificultad
                //     // enemyController.health *= difficulty;
                //     // enemyController.damage *= difficulty;
                // }

                Debug.Log($"Enemigo {enemyType} spawnado en {position} con dificultad {difficulty}");
            }
        }

        /// <summary>
        /// Ejemplo: Sistema de checkpoints con diálogo
        /// </summary>
        public void OnCheckpointReached(int checkpointId)
        {
            string[] checkpointMessages = {
                "Checkpoint 1 alcanzado. ¡Buen trabajo!",
                "Checkpoint 2. La dificultad aumenta...",
                "Checkpoint 3. ¡El jefe se acerca!"
            };

            var checkpointDialogue = new DialogueData
            {
                speakerName = "Sistema",
                message = checkpointMessages[Mathf.Min(checkpointId, checkpointMessages.Length - 1)]
            };

            CDialogueManager.Instance.ShowDialogueWithCallback(checkpointDialogue, () =>
            {
                // Guardar progreso
                SaveGame(checkpointId);

                // Aumentar dificultad
                IncreaseDifficulty(checkpointId);
            });
        }

        /// <summary>
        /// Ejemplo: Sistema de recogida de items
        /// </summary>
        public async void OnItemPickup(string itemType)
        {
            // Reproducir sonido de pickup
            CManagerSFX.Inst.PlaySound(1); // Sound effect ID

            // Mostrar mensaje
            var pickupDialogue = new DialogueData
            {
                speakerName = "Sistema",
                message = $"¡Has recogido {itemType}!"
            };

            CDialogueManager.Instance.ShowDialogue(pickupDialogue);

            // Aplicar efecto del item
            await ApplyItemEffect(itemType);
        }

        /// <summary>
        /// Ejemplo: Cambio de nivel
        /// </summary>
        public async void LoadNextLevel(string levelName)
        {
            // Mostrar loading screen
            var loadingDialogue = new DialogueData
            {
                speakerName = "Sistema",
                message = $"Cargando nivel: {levelName}..."
            };

            CDialogueManager.Instance.ShowDialogue(loadingDialogue);

            // Limpiar assets actuales
            CAssetManager.Instance.ClearCache();

            // Cargar nuevo nivel
            await CAssetManager.Instance.LoadSceneAsync(levelName);

            // Ocultar loading
            CDialogueManager.Instance.HideDialogue();
        }

        // ========================================================================
        // MÉTODOS DE SOPORTE (IMPLEMENTACIÓN DE EJEMPLO)
        // ========================================================================

        private void SaveGame(int checkpointId)
        {
            // Implementar guardado
            PlayerPrefs.SetInt("Checkpoint", checkpointId);
            PlayerPrefs.Save();
            Debug.Log($"Juego guardado en checkpoint {checkpointId}");
        }

        private void IncreaseDifficulty(int checkpointId)
        {
            // Implementar aumento de dificultad
            float difficultyMultiplier = 1f + (checkpointId * 0.2f);
            Debug.Log($"Dificultad aumentada: {difficultyMultiplier}x");
        }

        private async Task ApplyItemEffect(string itemType)
        {
            switch (itemType)
            {
                case "HealthPack":
                    // Curar jugador
                    await Task.Delay(100); // Simular efecto
                    Debug.Log("Salud restaurada");
                    break;

                case "AmmoBox":
                    // Dar munición
                    Debug.Log("Munición recargada");
                    break;

                case "WeaponUpgrade":
                    // Mejorar arma
                    Debug.Log("Arma mejorada");
                    break;
            }
        }
    }

    /// <summary>
    /// Ejemplo de NPC interactivo
    /// </summary>
    public class NPCExample : MonoBehaviour, Iinteract
    {
        [SerializeField] private DialogueData npcDialogue;

        public void Oninteract()
        {
            if (npcDialogue != null)
            {
                CDialogueManager.Instance.ShowDialogueWithOptions(npcDialogue, OnNPCTalkChoice);
            }
        }

        private void OnNPCTalkChoice(int choice)
        {
            switch (choice)
            {
                case 0: // Ayuda
                    Debug.Log("NPC: Gracias por ayudar");
                    // Dar recompensa
                    break;

                case 1: // Ignorar
                    Debug.Log("NPC: Está bien, entiendo");
                    break;
            }
        }
    }

    /// <summary>
    /// Ejemplo de spawner de enemigos
    /// </summary>
    public class EnemySpawnerExample : MonoBehaviour
    {
        [SerializeField] private string[] enemyTypes = { "Enemy_Basic", "Enemy_Fast" };
        [SerializeField] private float spawnInterval = 5f;

        private void Start()
        {
            InvokeRepeating("SpawnRandomEnemy", 2f, spawnInterval);
        }

        private async void SpawnRandomEnemy()
        {
            string randomEnemy = enemyTypes[Random.Range(0, enemyTypes.Length)];
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * 5f;
            spawnPos.y = 0; // Mantener en suelo

            await CAssetManager.Instance.InstantiateAssetAsync(randomEnemy, spawnPos);
        }
    }
}

// ========================================================================
// CONFIGURACIÓN DE ESCENA RECOMENDADA
// ========================================================================
/*
Para usar este ejemplo, crea una escena con:

1. GameManagerExample (con este script)
2. CAssetManager (vacío, singleton)
3. CDialogueManager (con UI asignada)
4. CManagerSFX (para audio)
5. CManagerMusic (para música)

6. En Addressables Groups:
   - Player.prefab
   - HUD.prefab
   - Enemy_Basic.prefab
   - Weapon_Pistol.prefab

7. UI Canvas con:
   - DialoguePanel (Panel)
   - SpeakerNameText (TextMeshPro)
   - MessageText (TextMeshPro)
   - OptionsContainer (Panel)
   - OptionButton prefab (Button + TextMeshPro)

¡Listo para empezar a crear tu FPS retro!
*/
