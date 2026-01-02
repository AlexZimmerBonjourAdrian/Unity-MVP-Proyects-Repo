using UnityEngine;
using HorrorEngine.Events;
using HorrorEngine.Manager;
using RetroFPS.Music;
using System.Collections.Generic;
using HorrorEngine.LevelManager;
using HorrorEngine.Player;
namespace HorrorEngine
{
    public class CGameManager : MonoBehaviour
    {
        // --- Singleton Pattern ---
        public static CGameManager Inst { get; private set; }
        public Vector3 PlayerPosition => player.transform.position;

        private CLevelManager levelManager;
        private CManagerMusic musicManager;
        private CManagerSFX audioManager;

        private GameObject player;

        // --- State Pattern ---
        private IGameState currentState;
        private Dictionary<GameState, IGameState> gameStates;

        // --- Command Pattern ---
        private ICommand unlockDoorCommand;

        private void Awake()
        {
            if (Inst != null && Inst != this)
            {
                Destroy(gameObject);
                return;
            }
            Inst = this;

            // Initialize subsystems (Facade Pattern)
            levelManager = CLevelManager.Inst;
            musicManager = CManagerMusic.Inst;
            audioManager = CManagerSFX.Inst;

            // Initialize game states (State Pattern)
            // gameStates = new Dictionary<GameState, IGameState>
            // {
            //     { GameState.Playing, new PlayingState(this) },
            //     { GameState.Paused, new PausedState(this) },
            //     { GameState.ReadingNote, new ReadingNoteState(this) }
            // };
//            currentState = gameStates[GameState.Playing];

            // Initialize commands (Command Pattern)
            unlockDoorCommand = new UnlockDoorCommand(this);
             CGameEventManager.RegisterStaticEvents();
           
        }

        private void OnEnable()
        {
            CGameEvents.OnUnlockDoor.Subscribe(() => unlockDoorCommand.Execute());
        }

        private void OnDisable()
        {
            CGameEvents.OnUnlockDoor.Unsubscribe(() => unlockDoorCommand.Execute());
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            if (player == null)
            {
                Debug.LogError("CGameManager: Player object not found!");
            }
            PlayBackgroundMusic();
        }

        #region Music Management

        public void PlayBackgroundMusic()
        {
            if (musicManager == null)
            {
                Debug.LogError("CGameManager: CManagerMusic instance not found! Cannot play music.");
                return;
            }
            // musicManager.PlayMusicBackground(backgroundMusicIndex);
        }

        public void StopBackgroundMusic()
        {
            if (musicManager == null)
            {
                Debug.LogError("CGameManager: CManagerMusic instance not found! Cannot stop music.");
                return;
            }
            musicManager.StopMusic();
        }

        #endregion

        #region Game State Management

        public void SetGameState(GameState newState)
        {
            currentState.Exit();
            currentState = gameStates[newState];
            currentState.Enter();
        }

        #endregion
    }
}
