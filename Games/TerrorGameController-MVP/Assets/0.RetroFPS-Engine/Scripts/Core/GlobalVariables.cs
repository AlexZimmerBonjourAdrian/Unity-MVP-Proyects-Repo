using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Sistema de Variables Globales - Singleton que mantiene estado global del juego.
    /// Gestiona variables críticas del juego y notifica cambios a través del Observer Pattern.
    /// </summary>
    public class GlobalVariables : MonoBehaviour
    {
        public static GlobalVariables Instance { get; private set; }

        // ============================================
        // PLAYER STATS
        // ============================================

        [Header("Player Stats")]
        [SerializeField] private int playerHealth = 100;
        [SerializeField] private int playerMaxHealth = 100;
        [SerializeField] private int playerAmmo = 30;
        [SerializeField] private int playerMaxAmmo = 200;
        [SerializeField] private int playerLives = 3;
        [SerializeField] private int playerScore = 0;

        // ============================================
        // GAME STATE
        // ============================================

        [Header("Game State")]
        [SerializeField] private int currentLevel = 1;
        [SerializeField] private float gameTime = 0f;
        [SerializeField] private bool gamePaused = false;
        [SerializeField] private string currentGameState = "MainMenu";

        // ============================================
        // ENEMIES & COMBAT
        // ============================================

        [Header("Enemies & Combat")]
        [SerializeField] private int activeEnemies = 0;
        [SerializeField] private int totalEnemiesKilled = 0;
        [SerializeField] private float enemyDamageMultiplier = 1.0f;
        [SerializeField] private float enemySpeedMultiplier = 1.0f;

        // ============================================
        // KEYS & PROGRESSION
        // ============================================

        [Header("Keys & Progression")]
        [SerializeField] private bool hasRedKey = false;
        [SerializeField] private bool hasBlueKey = false;
        [SerializeField] private bool hasYellowKey = false;
        [SerializeField] private int secretsFound = 0;
        [SerializeField] private int totalSecrets = 0;

        // ============================================
        // AUDIO SETTINGS
        // ============================================

        [Header("Audio Settings")]
        [SerializeField] private float masterVolume = 1.0f;
        [SerializeField] private float sfxVolume = 1.0f;
        [SerializeField] private float musicVolume = 1.0f;
        [SerializeField] private string currentMusicTrack = "";

        // ============================================
        // PERFORMANCE & DEBUG
        // ============================================

        [Header("Performance & Debug")]
        [SerializeField] private bool godMode = false;
        [SerializeField] private bool infiniteAmmo = false;
        [SerializeField] private int currentFPS = 60;
        [SerializeField] private float memoryUsage = 0f;

        // ============================================
        // PRIVATE FIELDS
        // ============================================

        private bool initialized = false;

        #region Singleton Lifecycle

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeObservers();
            LoadFromPlayerPrefs();

            initialized = true;
            LogDebug("GlobalVariables initialized");
        }

        private void Update()
        {
            if (!initialized) return;

            // Actualizar tiempo de juego si no está pausado
            if (!gamePaused)
            {
                gameTime += Time.deltaTime;
            }
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                SaveToPlayerPrefs();
                Instance = null;
            }
        }

        #endregion

        #region Observer Initialization

        /// <summary>
        /// Inicializa los observers con valores actuales
        /// </summary>
        private void InitializeObservers()
        {
            // Inicializar observers con valores actuales
            GameObservers.PlayerHealthChanged.SetValue(playerHealth);
            GameObservers.PlayerAmmoChanged.SetValue(playerAmmo);
            GameObservers.PlayerLivesChanged.SetValue(playerLives);
            GameObservers.PlayerScoreChanged.SetValue(playerScore);
            GameObservers.CurrentLevelChanged.SetValue(currentLevel);
            GameObservers.GameTimeChanged.SetValue(gameTime);
            GameObservers.GamePausedChanged.SetValue(gamePaused);
            GameObservers.ActiveEnemiesChanged.SetValue(activeEnemies);
            GameObservers.TotalEnemiesKilledChanged.SetValue(totalEnemiesKilled);

            // Keys
            GameObservers.RedKeyObtained.SetValue(hasRedKey);
            GameObservers.BlueKeyObtained.SetValue(hasBlueKey);
            GameObservers.YellowKeyObtained.SetValue(hasYellowKey);

            // Audio
            GameObservers.MasterVolumeChanged.SetValue(masterVolume);
            GameObservers.SFXVolumeChanged.SetValue(sfxVolume);
            GameObservers.MusicVolumeChanged.SetValue(musicVolume);
            GameObservers.CurrentMusicChanged.SetValue(currentMusicTrack);

            LogDebug("Observers initialized with current values");
        }

        #endregion

        #region Player Stats Methods

        /// <summary>
        /// Modifica la salud del jugador
        /// </summary>
        public void ModifyHealth(int amount)
        {
            int oldHealth = playerHealth;
            playerHealth = Mathf.Clamp(playerHealth + amount, 0, playerMaxHealth);

            if (oldHealth != playerHealth)
            {
                GameObservers.PlayerHealthChanged.SetValue(playerHealth);
                LogDebug($"Player health changed: {oldHealth} -> {playerHealth}");

                // Verificar si el jugador murió
                if (playerHealth <= 0)
                {
                    OnPlayerDeath();
                }
            }
        }

        /// <summary>
        /// Modifica la munición del jugador
        /// </summary>
        public void ModifyAmmo(int amount, string weaponType = "")
        {
            int oldAmmo = playerAmmo;
            playerAmmo = Mathf.Clamp(playerAmmo + amount, 0, playerMaxAmmo);

            if (oldAmmo != playerAmmo)
            {
                GameObservers.PlayerAmmoChanged.SetValue(playerAmmo);
                LogDebug($"Player ammo changed: {oldAmmo} -> {playerAmmo} ({weaponType})");
            }
        }

        /// <summary>
        /// Modifica las vidas del jugador
        /// </summary>
        public void ModifyLives(int amount)
        {
            int oldLives = playerLives;
            playerLives = Mathf.Max(0, playerLives + amount);

            if (oldLives != playerLives)
            {
                GameObservers.PlayerLivesChanged.SetValue(playerLives);
                LogDebug($"Player lives changed: {oldLives} -> {playerLives}");

                if (playerLives <= 0)
                {
                    OnGameOver();
                }
            }
        }

        /// <summary>
        /// Agrega puntos al score
        /// </summary>
        public void AddScore(int points)
        {
            int oldScore = playerScore;
            playerScore += points;

            GameObservers.PlayerScoreChanged.SetValue(playerScore);
            LogDebug($"Score changed: {oldScore} -> {playerScore}");
        }

        #endregion

        #region Game State Methods

        /// <summary>
        /// Cambia el nivel actual
        /// </summary>
        public void SetLevel(int level)
        {
            int oldLevel = currentLevel;
            currentLevel = Mathf.Max(1, level);

            if (oldLevel != currentLevel)
            {
                GameObservers.CurrentLevelChanged.SetValue(currentLevel);
                LogDebug($"Level changed: {oldLevel} -> {currentLevel}");
            }
        }

        /// <summary>
        /// Pausa o reanuda el juego
        /// </summary>
        public void SetPaused(bool paused)
        {
            if (gamePaused != paused)
            {
                gamePaused = paused;
                GameObservers.GamePausedChanged.SetValue(gamePaused);
                Time.timeScale = paused ? 0f : 1f;
                LogDebug($"Game {(paused ? "paused" : "resumed")}");
            }
        }

        /// <summary>
        /// Cambia el estado del juego
        /// </summary>
        public void SetGameState(string state)
        {
            string oldState = currentGameState;
            currentGameState = state;

            GameObservers.GameStateChanged.SetValue(currentGameState);
            LogDebug($"Game state changed: {oldState} -> {currentGameState}");
        }

        #endregion

        #region Enemy Methods

        /// <summary>
        /// Modifica el contador de enemigos activos
        /// </summary>
        public void ModifyActiveEnemies(int amount)
        {
            int oldCount = activeEnemies;
            activeEnemies = Mathf.Max(0, activeEnemies + amount);

            if (oldCount != activeEnemies)
            {
                GameObservers.ActiveEnemiesChanged.SetValue(activeEnemies);
                LogDebug($"Active enemies changed: {oldCount} -> {activeEnemies}");
            }
        }

        /// <summary>
        /// Incrementa el contador total de enemigos asesinados
        /// </summary>
        public void EnemyKilled(int scoreValue = 0)
        {
            totalEnemiesKilled++;
            GameObservers.TotalEnemiesKilledChanged.SetValue(totalEnemiesKilled);

            if (scoreValue > 0)
            {
                AddScore(scoreValue);
            }

            LogDebug($"Enemy killed. Total: {totalEnemiesKilled}");
        }

        #endregion

        #region Key Methods

        /// <summary>
        /// Otorga una llave al jugador
        /// </summary>
        public void GiveKey(string keyType)
        {
            switch (keyType.ToLower())
            {
                case "red":
                    if (!hasRedKey)
                    {
                        hasRedKey = true;
                        GameObservers.RedKeyObtained.SetValue(true);
                        LogDebug("Red key obtained");
                    }
                    break;

                case "blue":
                    if (!hasBlueKey)
                    {
                        hasBlueKey = true;
                        GameObservers.BlueKeyObtained.SetValue(true);
                        LogDebug("Blue key obtained");
                    }
                    break;

                case "yellow":
                    if (!hasYellowKey)
                    {
                        hasYellowKey = true;
                        GameObservers.YellowKeyObtained.SetValue(true);
                        LogDebug("Yellow key obtained");
                    }
                    break;

                default:
                    LogDebug($"Unknown key type: {keyType}");
                    break;
            }
        }

        /// <summary>
        /// Verifica si el jugador tiene una llave específica
        /// </summary>
        public bool HasKey(string keyType)
        {
            switch (keyType.ToLower())
            {
                case "red": return hasRedKey;
                case "blue": return hasBlueKey;
                case "yellow": return hasYellowKey;
                default: return false;
            }
        }

        /// <summary>
        /// Registra que se encontró un secreto
        /// </summary>
        public void SecretFound(int bonusScore = 1000)
        {
            secretsFound++;
            AddScore(bonusScore);
            LogDebug($"Secret found! Total: {secretsFound}/{totalSecrets}");
        }

        #endregion

        #region Audio Methods

        /// <summary>
        /// Cambia el volumen master
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            GameObservers.MasterVolumeChanged.SetValue(masterVolume);
            // TODO: Aplicar al AudioMixer
            LogDebug($"Master volume set to: {masterVolume}");
        }

        /// <summary>
        /// Cambia el volumen de SFX
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            GameObservers.SFXVolumeChanged.SetValue(sfxVolume);
            LogDebug($"SFX volume set to: {sfxVolume}");
        }

        /// <summary>
        /// Cambia el volumen de música
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            GameObservers.MusicVolumeChanged.SetValue(musicVolume);
            LogDebug($"Music volume set to: {musicVolume}");
        }

        /// <summary>
        /// Cambia la pista de música actual
        /// </summary>
        public void SetCurrentMusic(string trackName)
        {
            string oldTrack = currentMusicTrack;
            currentMusicTrack = trackName;

            GameObservers.CurrentMusicChanged.SetValue(currentMusicTrack);
            LogDebug($"Music changed: {oldTrack} -> {currentMusicTrack}");
        }

        #endregion

        #region Debug & Cheats

        /// <summary>
        /// Activa/desactiva el modo dios
        /// </summary>
        public void ToggleGodMode()
        {
            godMode = !godMode;
            LogDebug($"God mode {(godMode ? "enabled" : "disabled")}");
        }

        /// <summary>
        /// Activa/desactiva munición infinita
        /// </summary>
        public void ToggleInfiniteAmmo()
        {
            infiniteAmmo = !infiniteAmmo;
            LogDebug($"Infinite ammo {(infiniteAmmo ? "enabled" : "disabled")}");
        }

        /// <summary>
        /// Actualiza las estadísticas de performance
        /// </summary>
        public void UpdatePerformanceStats(int fps, float memory)
        {
            currentFPS = fps;
            memoryUsage = memory;

            GameObservers.CurrentFPSChanged.SetValue(currentFPS);
            GameObservers.MemoryUsageChanged.SetValue(memoryUsage);
        }

        #endregion

        #region Persistence

        /// <summary>
        /// Guarda las variables en PlayerPrefs
        /// </summary>
        public void SaveToPlayerPrefs()
        {
            PlayerPrefs.SetInt("PlayerHealth", playerHealth);
            PlayerPrefs.SetInt("PlayerMaxHealth", playerMaxHealth);
            PlayerPrefs.SetInt("PlayerAmmo", playerAmmo);
            PlayerPrefs.SetInt("PlayerLives", playerLives);
            PlayerPrefs.SetInt("PlayerScore", playerScore);
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            PlayerPrefs.SetFloat("GameTime", gameTime);

            PlayerPrefs.SetInt("TotalEnemiesKilled", totalEnemiesKilled);
            PlayerPrefs.SetInt("SecretsFound", secretsFound);

            PlayerPrefs.SetInt("HasRedKey", hasRedKey ? 1 : 0);
            PlayerPrefs.SetInt("HasBlueKey", hasBlueKey ? 1 : 0);
            PlayerPrefs.SetInt("HasYellowKey", hasYellowKey ? 1 : 0);

            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.SetString("CurrentMusicTrack", currentMusicTrack);

            PlayerPrefs.SetInt("GodMode", godMode ? 1 : 0);
            PlayerPrefs.SetInt("InfiniteAmmo", infiniteAmmo ? 1 : 0);

            PlayerPrefs.Save();
            LogDebug("Game data saved to PlayerPrefs");
        }

        /// <summary>
        /// Carga las variables desde PlayerPrefs
        /// </summary>
        public void LoadFromPlayerPrefs()
        {
            playerHealth = PlayerPrefs.GetInt("PlayerHealth", playerHealth);
            playerMaxHealth = PlayerPrefs.GetInt("PlayerMaxHealth", playerMaxHealth);
            playerAmmo = PlayerPrefs.GetInt("PlayerAmmo", playerAmmo);
            playerLives = PlayerPrefs.GetInt("PlayerLives", playerLives);
            playerScore = PlayerPrefs.GetInt("PlayerScore", playerScore);
            currentLevel = PlayerPrefs.GetInt("CurrentLevel", currentLevel);
            gameTime = PlayerPrefs.GetFloat("GameTime", gameTime);

            totalEnemiesKilled = PlayerPrefs.GetInt("TotalEnemiesKilled", totalEnemiesKilled);
            secretsFound = PlayerPrefs.GetInt("SecretsFound", secretsFound);

            hasRedKey = PlayerPrefs.GetInt("HasRedKey", 0) == 1;
            hasBlueKey = PlayerPrefs.GetInt("HasBlueKey", 0) == 1;
            hasYellowKey = PlayerPrefs.GetInt("HasYellowKey", 0) == 1;

            masterVolume = PlayerPrefs.GetFloat("MasterVolume", masterVolume);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", sfxVolume);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume);
            currentMusicTrack = PlayerPrefs.GetString("CurrentMusicTrack", currentMusicTrack);

            godMode = PlayerPrefs.GetInt("GodMode", 0) == 1;
            infiniteAmmo = PlayerPrefs.GetInt("InfiniteAmmo", 0) == 1;

            // Actualizar observers con valores cargados
            InitializeObservers();

            LogDebug("Game data loaded from PlayerPrefs");
        }

        /// <summary>
        /// Reinicia todas las variables a valores por defecto
        /// </summary>
        public void ResetToDefaults()
        {
            playerHealth = 100;
            playerMaxHealth = 100;
            playerAmmo = 30;
            playerLives = 3;
            playerScore = 0;

            currentLevel = 1;
            gameTime = 0f;
            gamePaused = false;
            currentGameState = "MainMenu";

            activeEnemies = 0;
            totalEnemiesKilled = 0;
            enemyDamageMultiplier = 1.0f;
            enemySpeedMultiplier = 1.0f;

            hasRedKey = false;
            hasBlueKey = false;
            hasYellowKey = false;
            secretsFound = 0;

            masterVolume = 1.0f;
            sfxVolume = 1.0f;
            musicVolume = 1.0f;
            currentMusicTrack = "";

            godMode = false;
            infiniteAmmo = false;

            InitializeObservers();
            LogDebug("All variables reset to defaults");
        }

        #endregion

        #region Event Handlers

        private void OnPlayerDeath()
        {
            ModifyLives(-1);
            if (playerLives > 0)
            {
                // Respawn logic
                playerHealth = playerMaxHealth;
                GameObservers.PlayerHealthChanged.SetValue(playerHealth);
                LogDebug("Player respawned");
            }
        }

        private void OnGameOver()
        {
            SetGameState("GameOver");
            LogDebug("Game Over!");
        }

        #endregion

        #region Properties (Read-Only Access)

        // Player Stats
        public int PlayerHealth => playerHealth;
        public int PlayerMaxHealth => playerMaxHealth;
        public int PlayerAmmo => playerAmmo;
        public int PlayerMaxAmmo => playerMaxAmmo;
        public int PlayerLives => playerLives;
        public int PlayerScore => playerScore;

        // Game State
        public int CurrentLevel => currentLevel;
        public float GameTime => gameTime;
        public bool GamePaused => gamePaused;
        public string CurrentGameState => currentGameState;

        // Enemies
        public int ActiveEnemies => activeEnemies;
        public int TotalEnemiesKilled => totalEnemiesKilled;
        public float EnemyDamageMultiplier => enemyDamageMultiplier;
        public float EnemySpeedMultiplier => enemySpeedMultiplier;

        // Keys
        public bool HasRedKey => hasRedKey;
        public bool HasBlueKey => hasBlueKey;
        public bool HasYellowKey => hasYellowKey;
        public int SecretsFound => secretsFound;

        // Audio
        public float MasterVolume => masterVolume;
        public float SFXVolume => sfxVolume;
        public float MusicVolume => musicVolume;
        public string CurrentMusicTrack => currentMusicTrack;

        // Debug
        public bool GodMode => godMode;
        public bool InfiniteAmmo => infiniteAmmo;
        public int CurrentFPS => currentFPS;
        public float MemoryUsage => memoryUsage;

        #endregion

        #region Debug Methods

        /// <summary>
        /// Obtiene información de debug de todas las variables
        /// </summary>
        public string GetDebugInfo()
        {
            return "=== GLOBAL VARIABLES DEBUG ===\n" +
                   "\nPLAYER STATS:\n" +
                   $"- Health: {playerHealth}/{playerMaxHealth}\n" +
                   $"- Ammo: {playerAmmo}/{playerMaxAmmo}\n" +
                   $"- Lives: {playerLives}\n" +
                   $"- Score: {playerScore}\n" +
                   "\nGAME STATE:\n" +
                   $"- Level: {currentLevel}\n" +
                   $"- Time: {gameTime:F2}s\n" +
                   $"- Paused: {gamePaused}\n" +
                   $"- State: {currentGameState}\n" +
                   "\nENEMIES:\n" +
                   $"- Active: {activeEnemies}\n" +
                   $"- Killed: {totalEnemiesKilled}\n" +
                   $"- Damage Multiplier: {enemyDamageMultiplier}\n" +
                   "\nKEYS:\n" +
                   $"- Red: {hasRedKey}\n" +
                   $"- Blue: {hasBlueKey}\n" +
                   $"- Yellow: {hasYellowKey}\n" +
                   $"- Secrets: {secretsFound}\n" +
                   "\nAUDIO:\n" +
                   $"- Master: {masterVolume}\n" +
                   $"- SFX: {sfxVolume}\n" +
                   $"- Music: {musicVolume}\n" +
                   $"- Track: {currentMusicTrack}\n" +
                   "\nDEBUG:\n" +
                   $"- God Mode: {godMode}\n" +
                   $"- Infinite Ammo: {infiniteAmmo}\n" +
                   $"- FPS: {currentFPS}\n" +
                   $"- Memory: {memoryUsage:F2}MB";
        }

        #endregion

        private void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[GlobalVariables] {message}");
#endif
        }
    }
}
