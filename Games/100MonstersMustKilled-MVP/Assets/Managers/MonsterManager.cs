using UnityEngine;
using System.Collections.Generic;
using ECS.Core;
using ECS.Components.Combat;
using ECS.Components.Dialogue;
using ECS.Components.Behavior;
using ECS.Components.State;

namespace Managers
{
    public class MonsterManager : MonoBehaviour
    {
        private static MonsterManager instance;
        public static MonsterManager Instance { get { return instance; } }

        private World world;
        private List<Entity> activeMonsters;
        private List<Entity> killedMonsters;
        private int totalMonstersKilled;
        private int maxMonsters;

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
            activeMonsters = new List<Entity>();
            killedMonsters = new List<Entity>();
            totalMonstersKilled = 0;
            maxMonsters = 5;
        }

        private void Start()
        {
        }

        public Entity SpawnMonster(MonsterData data)
        {
            return default(Entity);
        }

        public void KillMonster(Entity monster)
        {
        }

        public void RemoveMonster(Entity monster)
        {
        }

        public List<Entity> GetActiveMonsters()
        {
            return activeMonsters;
        }

        public int GetTotalMonstersKilled()
        {
            return totalMonstersKilled;
        }

        public bool HasReachedMaxMonsters()
        {
            return totalMonstersKilled >= maxMonsters;
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        public void Reset()
        {
        }
    }

    [System.Serializable]
    public class MonsterData
    {
        public string name;
        public float maxHealth;
        public float damage;
        public float defense;
        public string dialogueText;
        public List<string> dialogueOptions;
    }
}

