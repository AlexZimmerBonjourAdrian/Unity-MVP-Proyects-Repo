using UnityEngine;
using System;

namespace RetroFPS
{
    /// <summary>
    /// Observers globales del juego - Sistema centralizado de observers para variables importantes.
    /// Permite que múltiples sistemas se suscriban a cambios en valores críticos del juego.
    /// </summary>
    public static class GameObservers
    {
        #region Player Stats Observers

        /// <summary>
        /// Observer para cambios en la salud del jugador
        /// </summary>
        public static readonly GameObserver<int> PlayerHealthChanged = new GameObserver<int>(100);

        /// <summary>
        /// Observer para cambios en la munición del jugador
        /// </summary>
        public static readonly GameObserver<int> PlayerAmmoChanged = new GameObserver<int>(30);

        /// <summary>
        /// Observer para cambios en las vidas del jugador
        /// </summary>
        public static readonly GameObserver<int> PlayerLivesChanged = new GameObserver<int>(3);

        /// <summary>
        /// Observer para cambios en el score del jugador
        /// </summary>
        public static readonly GameObserver<int> PlayerScoreChanged = new GameObserver<int>(0);

        #endregion

        #region Game State Observers

        /// <summary>
        /// Observer para cambios en el estado del juego (Menu, Playing, Paused, etc.)
        /// </summary>
        public static readonly GameObserver<string> GameStateChanged = new GameObserver<string>("MainMenu");

        /// <summary>
        /// Observer para cambios en el nivel actual
        /// </summary>
        public static readonly GameObserver<int> CurrentLevelChanged = new GameObserver<int>(1);

        /// <summary>
        /// Observer para cambios en el tiempo de juego
        /// </summary>
        public static readonly GameObserver<float> GameTimeChanged = new GameObserver<float>(0f);

        /// <summary>
        /// Observer para pausas del juego
        /// </summary>
        public static readonly GameObserver<bool> GamePausedChanged = new GameObserver<bool>(false);

        #endregion

        #region Enemy Observers

        /// <summary>
        /// Observer para cambios en el contador de enemigos activos
        /// </summary>
        public static readonly GameObserver<int> ActiveEnemiesChanged = new GameObserver<int>(0);

        /// <summary>
        /// Observer para cambios en el contador total de enemigos asesinados
        /// </summary>
        public static readonly GameObserver<int> TotalEnemiesKilledChanged = new GameObserver<int>(0);

        /// <summary>
        /// Observer para alertas de detección de enemigos
        /// </summary>
        public static readonly GameObserver<Vector3> EnemyAlertTriggered = new GameObserver<Vector3>(Vector3.zero);

        #endregion

        #region Inventory Observers

        /// <summary>
        /// Observer para cambios en el inventario (número de items)
        /// </summary>
        public static readonly GameObserver<int> InventoryItemsChanged = new GameObserver<int>(0);

        /// <summary>
        /// Observer para cuando se equipa un item
        /// </summary>
        public static readonly GameObserver<string> ItemEquipped = new GameObserver<string>("");

        /// <summary>
        /// Observer para cuando se usa un item
        /// </summary>
        public static readonly GameObserver<string> ItemUsed = new GameObserver<string>("");

        #endregion

        #region Keys and Doors Observers

        /// <summary>
        /// Observer para cuando se obtiene una llave roja
        /// </summary>
        public static readonly GameObserver<bool> RedKeyObtained = new GameObserver<bool>(false);

        /// <summary>
        /// Observer para cuando se obtiene una llave azul
        /// </summary>
        public static readonly GameObserver<bool> BlueKeyObtained = new GameObserver<bool>(false);

        /// <summary>
        /// Observer para cuando se obtiene una llave amarilla
        /// </summary>
        public static readonly GameObserver<bool> YellowKeyObtained = new GameObserver<bool>(false);

        /// <summary>
        /// Observer para cuando se abre una puerta
        /// </summary>
        public static readonly GameObserver<string> DoorOpened = new GameObserver<string>("");

        #endregion

        #region Weapon Observers

        /// <summary>
        /// Observer para cambios en el arma actual
        /// </summary>
        public static readonly GameObserver<string> CurrentWeaponChanged = new GameObserver<string>("Pistol");

        /// <summary>
        /// Observer para cuando se dispara un arma
        /// </summary>
        public static readonly GameObserver<string> WeaponFired = new GameObserver<string>("");

        /// <summary>
        /// Observer para cuando se recarga un arma
        /// </summary>
        public static readonly GameObserver<string> WeaponReloaded = new GameObserver<string>("");

        #endregion

        #region UI Observers

        /// <summary>
        /// Observer para mostrar/ocultar HUD
        /// </summary>
        public static readonly GameObserver<bool> HUDVisibilityChanged = new GameObserver<bool>(true);

        /// <summary>
        /// Observer para mensajes de pantalla
        /// </summary>
        public static readonly GameObserver<string> ScreenMessageChanged = new GameObserver<string>("");

        /// <summary>
        /// Observer para cambios en el menú activo
        /// </summary>
        public static readonly GameObserver<string> ActiveMenuChanged = new GameObserver<string>("");

        #endregion

        #region Dialogue Observers

        /// <summary>
        /// Observer para cuando inicia un diálogo
        /// </summary>
        public static readonly GameObserver<bool> DialogueActiveChanged = new GameObserver<bool>(false);

        /// <summary>
        /// Observer para el texto actual del diálogo
        /// </summary>
        public static readonly GameObserver<string> DialogueTextChanged = new GameObserver<string>("");

        /// <summary>
        /// Observer para el nombre del hablante actual
        /// </summary>
        public static readonly GameObserver<string> DialogueSpeakerChanged = new GameObserver<string>("");

        #endregion

        #region Audio Observers

        /// <summary>
        /// Observer para cambios en el volumen master
        /// </summary>
        public static readonly GameObserver<float> MasterVolumeChanged = new GameObserver<float>(1.0f);

        /// <summary>
        /// Observer para cambios en el volumen SFX
        /// </summary>
        public static readonly GameObserver<float> SFXVolumeChanged = new GameObserver<float>(1.0f);

        /// <summary>
        /// Observer para cambios en el volumen de música
        /// </summary>
        public static readonly GameObserver<float> MusicVolumeChanged = new GameObserver<float>(1.0f);

        /// <summary>
        /// Observer para cambios en la música actual
        /// </summary>
        public static readonly GameObserver<string> CurrentMusicChanged = new GameObserver<string>("");

        #endregion

        #region Performance Observers

        /// <summary>
        /// Observer para FPS actuales
        /// </summary>
        public static readonly GameObserver<int> CurrentFPSChanged = new GameObserver<int>(60);

        /// <summary>
        /// Observer para uso de memoria
        /// </summary>
        public static readonly GameObserver<float> MemoryUsageChanged = new GameObserver<float>(0f);

        /// <summary>
        /// Observer para número de objetos activos
        /// </summary>
        public static readonly GameObserver<int> ActiveObjectsChanged = new GameObserver<int>(0);

        #endregion

        #region Métodos de Utilidad

        /// <summary>
        /// Limpia todos los observers (útil para cambio de escenas)
        /// </summary>
        public static void ClearAll()
        {
            Debug.Log("[GameObservers] Clearing all observers...");

            // Player Stats
            PlayerHealthChanged.Clear();
            PlayerAmmoChanged.Clear();
            PlayerLivesChanged.Clear();
            PlayerScoreChanged.Clear();

            // Game State
            GameStateChanged.Clear();
            CurrentLevelChanged.Clear();
            GameTimeChanged.Clear();
            GamePausedChanged.Clear();

            // Enemies
            ActiveEnemiesChanged.Clear();
            TotalEnemiesKilledChanged.Clear();
            EnemyAlertTriggered.Clear();

            // Inventory
            InventoryItemsChanged.Clear();
            ItemEquipped.Clear();
            ItemUsed.Clear();

            // Keys and Doors
            RedKeyObtained.Clear();
            BlueKeyObtained.Clear();
            YellowKeyObtained.Clear();
            DoorOpened.Clear();

            // Weapons
            CurrentWeaponChanged.Clear();
            WeaponFired.Clear();
            WeaponReloaded.Clear();

            // UI
            HUDVisibilityChanged.Clear();
            ScreenMessageChanged.Clear();
            ActiveMenuChanged.Clear();

            // Dialogue
            DialogueActiveChanged.Clear();
            DialogueTextChanged.Clear();
            DialogueSpeakerChanged.Clear();

            // Audio
            MasterVolumeChanged.Clear();
            SFXVolumeChanged.Clear();
            MusicVolumeChanged.Clear();
            CurrentMusicChanged.Clear();

            // Performance
            CurrentFPSChanged.Clear();
            MemoryUsageChanged.Clear();
            ActiveObjectsChanged.Clear();

            Debug.Log("[GameObservers] All observers cleared");
        }

        /// <summary>
        /// Obtiene información de debug de todos los observers
        /// </summary>
        public static string GetDebugInfo()
        {
            string info = "=== GameObservers Debug Info ===\n\n";

            info += "Player Stats:\n";
            info += $"- Health: {PlayerHealthChanged.GetDebugInfo()}\n";
            info += $"- Ammo: {PlayerAmmoChanged.GetDebugInfo()}\n";
            info += $"- Lives: {PlayerLivesChanged.GetDebugInfo()}\n";
            info += $"- Score: {PlayerScoreChanged.GetDebugInfo()}\n\n";

            info += "Game State:\n";
            info += $"- State: {GameStateChanged.GetDebugInfo()}\n";
            info += $"- Level: {CurrentLevelChanged.GetDebugInfo()}\n";
            info += $"- Paused: {GamePausedChanged.GetDebugInfo()}\n\n";

            info += "Enemies:\n";
            info += $"- Active: {ActiveEnemiesChanged.GetDebugInfo()}\n";
            info += $"- Killed: {TotalEnemiesKilledChanged.GetDebugInfo()}\n\n";

            info += "Inventory:\n";
            info += $"- Items: {InventoryItemsChanged.GetDebugInfo()}\n\n";

            info += "Keys:\n";
            info += $"- Red: {RedKeyObtained.GetDebugInfo()}\n";
            info += $"- Blue: {BlueKeyObtained.GetDebugInfo()}\n";
            info += $"- Yellow: {YellowKeyObtained.GetDebugInfo()}\n\n";

            info += "Audio:\n";
            info += $"- Master Vol: {MasterVolumeChanged.GetDebugInfo()}\n";
            info += $"- SFX Vol: {SFXVolumeChanged.GetDebugInfo()}\n";
            info += $"- Music Vol: {MusicVolumeChanged.GetDebugInfo()}\n\n";

            return info;
        }

        /// <summary>
        /// Suscribe un callback a múltiples observers comunes
        /// </summary>
        public static void SubscribeToCommonEvents(Action onPlayerStateChanged)
        {
            PlayerHealthChanged.Attach(_ => onPlayerStateChanged());
            PlayerAmmoChanged.Attach(_ => onPlayerStateChanged());
            PlayerLivesChanged.Attach(_ => onPlayerStateChanged());
        }

        /// <summary>
        /// Desuscribe un callback de múltiples observers comunes
        /// </summary>
        public static void UnsubscribeFromCommonEvents(Action onPlayerStateChanged)
        {
            PlayerHealthChanged.Detach(_ => onPlayerStateChanged());
            PlayerAmmoChanged.Detach(_ => onPlayerStateChanged());
            PlayerLivesChanged.Detach(_ => onPlayerStateChanged());
        }

        #endregion
    }
}
