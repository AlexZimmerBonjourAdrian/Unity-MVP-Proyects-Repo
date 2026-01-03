using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Eventos específicos del juego para Retro FPS Engine.
    /// Todos heredan de IEvent para compatibilidad con EventBus.
    /// </summary>

    #region Player Events

    /// <summary>
    /// Evento cuando cambia la salud del jugador
    /// </summary>
    public class PlayerHealthChangedEvent : IEvent
    {
        public int NewHealth { get; set; }
        public int MaxHealth { get; set; }
        public int HealthDifference { get; set; }

        public PlayerHealthChangedEvent(int newHealth, int maxHealth, int difference)
        {
            NewHealth = newHealth;
            MaxHealth = maxHealth;
            HealthDifference = difference;
        }
    }

    /// <summary>
    /// Evento cuando cambia la munición del jugador
    /// </summary>
    public class PlayerAmmoChangedEvent : IEvent
    {
        public int NewAmmo { get; set; }
        public int MaxAmmo { get; set; }
        public string WeaponType { get; set; }

        public PlayerAmmoChangedEvent(int newAmmo, int maxAmmo, string weaponType = "")
        {
            NewAmmo = newAmmo;
            MaxAmmo = maxAmmo;
            WeaponType = weaponType;
        }
    }

    /// <summary>
    /// Evento cuando el jugador muere
    /// </summary>
    public class PlayerDiedEvent : IEvent
    {
        public Vector3 DeathPosition { get; set; }
        public string CauseOfDeath { get; set; }

        public PlayerDiedEvent(Vector3 position, string cause = "")
        {
            DeathPosition = position;
            CauseOfDeath = cause;
        }
    }

    /// <summary>
    /// Evento cuando el jugador recolecta un item
    /// </summary>
    public class PlayerItemCollectedEvent : IEvent
    {
        public string ItemName { get; set; }
        public string ItemType { get; set; }
        public Vector3 CollectionPosition { get; set; }

        public PlayerItemCollectedEvent(string itemName, string itemType, Vector3 position)
        {
            ItemName = itemName;
            ItemType = itemType;
            CollectionPosition = position;
        }
    }

    #endregion

    #region Enemy Events

    /// <summary>
    /// Evento cuando un enemigo es destruido
    /// </summary>
    public class EnemyKilledEvent : IEvent
    {
        public GameObject Enemy { get; set; }
        public string EnemyType { get; set; }
        public Vector3 DeathPosition { get; set; }
        public int ScoreValue { get; set; }

        public EnemyKilledEvent(GameObject enemy, string enemyType, Vector3 position, int score = 0)
        {
            Enemy = enemy;
            EnemyType = enemyType;
            DeathPosition = position;
            ScoreValue = score;
        }
    }

    /// <summary>
    /// Evento cuando un enemigo spawnea
    /// </summary>
    public class EnemySpawnedEvent : IEvent
    {
        public GameObject Enemy { get; set; }
        public string EnemyType { get; set; }
        public Vector3 SpawnPosition { get; set; }

        public EnemySpawnedEvent(GameObject enemy, string enemyType, Vector3 position)
        {
            Enemy = enemy;
            EnemyType = enemyType;
            SpawnPosition = position;
        }
    }

    /// <summary>
    /// Evento cuando un enemigo detecta al jugador
    /// </summary>
    public class EnemyDetectedPlayerEvent : IEvent
    {
        public GameObject Enemy { get; set; }
        public GameObject Player { get; set; }
        public Vector3 DetectionPosition { get; set; }

        public EnemyDetectedPlayerEvent(GameObject enemy, GameObject player, Vector3 position)
        {
            Enemy = enemy;
            Player = player;
            DetectionPosition = position;
        }
    }

    #endregion

    #region Weapon Events

    /// <summary>
    /// Evento cuando se dispara un arma
    /// </summary>
    public class WeaponFiredEvent : IEvent
    {
        public string WeaponName { get; set; }
        public Vector3 FirePosition { get; set; }
        public Vector3 FireDirection { get; set; }
        public int AmmoRemaining { get; set; }

        public WeaponFiredEvent(string weaponName, Vector3 position, Vector3 direction, int ammoRemaining)
        {
            WeaponName = weaponName;
            FirePosition = position;
            FireDirection = direction;
            AmmoRemaining = ammoRemaining;
        }
    }

    /// <summary>
    /// Evento cuando se recarga un arma
    /// </summary>
    public class WeaponReloadedEvent : IEvent
    {
        public string WeaponName { get; set; }
        public int AmmoAdded { get; set; }
        public int NewAmmoCount { get; set; }

        public WeaponReloadedEvent(string weaponName, int ammoAdded, int newAmmoCount)
        {
            WeaponName = weaponName;
            AmmoAdded = ammoAdded;
            NewAmmoCount = newAmmoCount;
        }
    }

    /// <summary>
    /// Evento cuando se cambia de arma
    /// </summary>
    public class WeaponSwitchedEvent : IEvent
    {
        public string PreviousWeapon { get; set; }
        public string NewWeapon { get; set; }

        public WeaponSwitchedEvent(string previous, string newWeapon)
        {
            PreviousWeapon = previous;
            NewWeapon = newWeapon;
        }
    }

    #endregion

    #region Level Events

    /// <summary>
    /// Evento cuando se carga un nivel
    /// </summary>
    public class LevelLoadedEvent : IEvent
    {
        public string LevelName { get; set; }
        public int LevelIndex { get; set; }

        public LevelLoadedEvent(string levelName, int levelIndex)
        {
            LevelName = levelName;
            LevelIndex = levelIndex;
        }
    }

    /// <summary>
    /// Evento cuando se completa un nivel
    /// </summary>
    public class LevelCompletedEvent : IEvent
    {
        public string LevelName { get; set; }
        public int LevelIndex { get; set; }
        public float CompletionTime { get; set; }
        public int Score { get; set; }

        public LevelCompletedEvent(string levelName, int levelIndex, float time, int score)
        {
            LevelName = levelName;
            LevelIndex = levelIndex;
            CompletionTime = time;
            Score = score;
        }
    }

    /// <summary>
    /// Evento cuando se falla un nivel
    /// </summary>
    public class LevelFailedEvent : IEvent
    {
        public string LevelName { get; set; }
        public string FailureReason { get; set; }

        public LevelFailedEvent(string levelName, string reason)
        {
            LevelName = levelName;
            FailureReason = reason;
        }
    }

    #endregion

    #region UI Events

    /// <summary>
    /// Evento cuando se pausa el juego
    /// </summary>
    public class GamePausedEvent : IEvent
    {
        public bool IsPaused { get; set; }

        public GamePausedEvent(bool paused)
        {
            IsPaused = paused;
        }
    }

    /// <summary>
    /// Evento cuando se abre el menú principal
    /// </summary>
    public class MainMenuOpenedEvent : IEvent
    {
        public bool FromGameplay { get; set; }

        public MainMenuOpenedEvent(bool fromGameplay = false)
        {
            FromGameplay = fromGameplay;
        }
    }

    #endregion

    #region Dialogue Events

    /// <summary>
    /// Evento cuando se inicia un diálogo
    /// </summary>
    public class DialogueStartedEvent : IEvent
    {
        public string SpeakerName { get; set; }
        public string DialogueText { get; set; }

        public DialogueStartedEvent(string speaker, string text)
        {
            SpeakerName = speaker;
            DialogueText = text;
        }
    }

    /// <summary>
    /// Evento cuando se completa un diálogo
    /// </summary>
    public class DialogueCompletedEvent : IEvent
    {
        public string SpeakerName { get; set; }
        public bool HadChoices { get; set; }

        public DialogueCompletedEvent(string speaker, bool hadChoices = false)
        {
            SpeakerName = speaker;
            HadChoices = hadChoices;
        }
    }

    #endregion

    #region Score Events

    /// <summary>
    /// Evento cuando cambia el puntaje
    /// </summary>
    public class ScoreChangedEvent : IEvent
    {
        public int NewScore { get; set; }
        public int ScoreDifference { get; set; }
        public string Reason { get; set; }

        public ScoreChangedEvent(int newScore, int difference, string reason = "")
        {
            NewScore = newScore;
            ScoreDifference = difference;
            Reason = reason;
        }
    }

    /// <summary>
    /// Evento cuando se obtiene un secreto
    /// </summary>
    public class SecretFoundEvent : IEvent
    {
        public string SecretName { get; set; }
        public Vector3 SecretPosition { get; set; }
        public int BonusScore { get; set; }

        public SecretFoundEvent(string secretName, Vector3 position, int bonusScore = 0)
        {
            SecretName = secretName;
            SecretPosition = position;
            BonusScore = bonusScore;
        }
    }

    #endregion

    #region Interaction Events

    /// <summary>
    /// Evento cuando el jugador interactúa con un objeto
    /// </summary>
    public class PlayerInteractedEvent : IEvent
    {
        public GameObject InteractedObject { get; set; }
        public string InteractionType { get; set; }
        public Vector3 InteractionPosition { get; set; }

        public PlayerInteractedEvent(GameObject obj, string interactionType, Vector3 position)
        {
            InteractedObject = obj;
            InteractionType = interactionType;
            InteractionPosition = position;
        }
    }

    /// <summary>
    /// Evento cuando se abre una puerta
    /// </summary>
    public class DoorOpenedEvent : IEvent
    {
        public GameObject Door { get; set; }
        public bool RequiresKey { get; set; }
        public string KeyType { get; set; }

        public DoorOpenedEvent(GameObject door, bool requiresKey = false, string keyType = "")
        {
            Door = door;
            RequiresKey = requiresKey;
            KeyType = keyType;
        }
    }

    /// <summary>
    /// Evento cuando se activa un switch
    /// </summary>
    public class SwitchActivatedEvent : IEvent
    {
        public GameObject Switch { get; set; }
        public string SwitchType { get; set; }
        public bool IsActivated { get; set; }

        public SwitchActivatedEvent(GameObject switchObj, string switchType, bool activated)
        {
            Switch = switchObj;
            SwitchType = switchType;
            IsActivated = activated;
        }
    }

    #endregion
}
