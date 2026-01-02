using UnityEngine;
using ECS.Core;
using ECS.Components.Anger;
using ECS.Components.Money;

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
        private Entity playerEntity;

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
            
            // Obtener ECSManager de la escena
            ecsManager = FindObjectOfType<ECS.ECSManager>();
            if (ecsManager != null)
            {
                world = ecsManager.GetWorld();
                CreatePlayer();
            }
        }

        private void CreatePlayer()
        {
            if (world == null) return;

            // Crear entidad del jugador
            playerEntity = world.CreateEntity();

            // Añadir AngerComponent
            var angerComponent = new AngerComponent
            {
                CurrentAnger = 0f,
                MaxAnger = 100f,
                HasExploded = false
            };
            world.AddComponent<AngerComponent>(playerEntity, angerComponent);

            // Añadir MoneyComponent
            var moneyComponent = new MoneyComponent
            {
                CurrentMoney = 0f,
                TotalSavings = 0f,
                DailyIncome = 0f,
                DailyExpenses = 325f,
                SavingsGoal = 5000f
            };
            world.AddComponent<MoneyComponent>(playerEntity, moneyComponent);

            // Inicializar sistemas con referencia al jugador
            InitializeSystems();
        }

        private void InitializeSystems()
        {
            if (ecsManager == null) return;

            // Obtener sistemas y asignarles la entidad del jugador
            var angerSystem = ecsManager.GetAngerSystem();
            if (angerSystem != null)
            {
                angerSystem.SetPlayerEntity(playerEntity);
            }

            var economySystem = ecsManager.GetEconomySystem();
            if (economySystem != null)
            {
                economySystem.SetPlayerEntity(playerEntity);
            }
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
            ChangeState(GameState.Playing);
        }

        public void PauseGame()
        {
            if (currentState == GameState.Playing)
            {
                ChangeState(GameState.Paused);
            }
        }

        public void ResumeGame()
        {
            if (currentState == GameState.Paused)
            {
                ChangeState(GameState.Playing);
            }
        }

        public void GameOver()
        {
            ChangeState(GameState.GameOver);
        }

        public void Victory()
        {
            ChangeState(GameState.Victory);
        }

        public void RestartGame()
        {
            // Resetear componentes del jugador
            if (world != null && world.HasComponent<AngerComponent>(playerEntity))
            {
                var anger = world.GetComponent<AngerComponent>(playerEntity);
                anger.CurrentAnger = 0f;
                anger.HasExploded = false;
            }

            if (world != null && world.HasComponent<MoneyComponent>(playerEntity))
            {
                var money = world.GetComponent<MoneyComponent>(playerEntity);
                money.CurrentMoney = 0f;
                money.TotalSavings = 0f;
                money.DailyIncome = 0f;
            }

            ChangeState(GameState.Menu);
        }

        public Entity GetPlayerEntity()
        {
            return playerEntity;
        }

        public World GetWorld()
        {
            return world;
        }

        private void OnDestroy()
        {
        }
    }
}

