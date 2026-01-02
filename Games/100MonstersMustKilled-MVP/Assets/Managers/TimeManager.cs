using UnityEngine;
using ECS.Core;
using ECS.Components.Time;
using ECS.Components.State;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        private static TimeManager instance;
        public static TimeManager Instance { get { return instance; } }

        private World world;
        private Entity timeEntity;
        private int maxEncounters;
        private int currentEncounters;
        private bool isPaused;

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
            maxEncounters = 5;
            currentEncounters = 0;
            isPaused = false;
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void RegisterEncounter()
        {
        }

        public void PauseTime()
        {
        }

        public void ResumeTime()
        {
        }

        public int GetEncountersRemaining()
        {
            return maxEncounters - currentEncounters;
        }

        public bool HasTimeExpired()
        {
            return currentEncounters >= maxEncounters;
        }

        public void ResetTime()
        {
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        public void SetTimeEntity(Entity entity)
        {
            timeEntity = entity;
        }
    }
}

