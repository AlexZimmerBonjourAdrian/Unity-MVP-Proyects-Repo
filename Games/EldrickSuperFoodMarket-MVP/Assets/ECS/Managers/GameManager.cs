using UnityEngine;
using ECS.Core;

namespace Managers
{
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        GameOver,
        Victory,
        Dialogue,
        Combat
    }

    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance { get { return instance; } }

        private GameState currentState;
        private World world;
        private ECS.ECSManager ecsManager;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
        }

        private void Initialize()
        {
            currentState = GameState.Menu;
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void ChangeState(GameState newState)
        {
            currentState = newState;
        }

        public GameState GetCurrentState()
        {
            return currentState;
        }

        public void StartGame()
        {
        }

        public void PauseGame()
        {
        }

        public void ResumeGame()
        {
        }

        public void GameOver()
        {
        }

        public void Victory()
        {
        }

        public void RestartGame()
        {
        }

        private void OnDestroy()
        {
        }
    }
}

