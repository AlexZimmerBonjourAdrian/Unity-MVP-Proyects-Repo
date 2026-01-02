using UnityEngine;
using ECS.Core;
using ECS.Components.Combat;
using ECS.Components.State;

namespace Managers
{
    public class CombatManager : MonoBehaviour
    {
        private static CombatManager instance;
        public static CombatManager Instance { get { return instance; } }

        private World world;
        private Entity playerEntity;
        private Entity currentEnemyEntity;
        private bool isInCombat;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
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
            isInCombat = false;
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void StartCombat(Entity player, Entity enemy)
        {
        }

        public void EndCombat()
        {
        }

        public void PlayerAttack()
        {
        }

        public void PlayerDefend()
        {
        }

        public void EnemyTurn()
        {
        }

        public void ResolveCombat()
        {
        }

        public bool IsInCombat()
        {
            return isInCombat;
        }

        public Entity GetCurrentEnemy()
        {
            return currentEnemyEntity;
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        public void SetPlayerEntity(Entity player)
        {
            playerEntity = player;
        }
    }
}

