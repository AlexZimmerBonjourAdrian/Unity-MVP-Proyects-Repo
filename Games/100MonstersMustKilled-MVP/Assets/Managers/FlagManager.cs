using UnityEngine;
using System.Collections.Generic;
using ECS.Core;
using ECS.Components.Dialogue;

namespace Managers
{
    public class FlagManager : MonoBehaviour
    {
        private static FlagManager instance;
        public static FlagManager Instance { get { return instance; } }

        private World world;
        private Dictionary<string, bool> globalFlags;
        private Entity flagEntity;

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
            globalFlags = new Dictionary<string, bool>();
            InitializeDefaultFlags();
        }

        private void InitializeDefaultFlags()
        {
            globalFlags["has_killed_monsters"] = false;
            globalFlags["has_spoken_monsters"] = false;
        }

        private void Start()
        {
        }

        public void SetFlag(string flagName, bool value)
        {
            if (globalFlags.ContainsKey(flagName))
            {
                globalFlags[flagName] = value;
            }
            else
            {
                globalFlags.Add(flagName, value);
            }

            if (world != null && flagEntity.Id != 0)
            {
                var flagComponent = world.GetComponent<FlagComponent>(flagEntity);
                if (flagComponent != null)
                {
                    flagComponent.Flags[flagName] = value;
                }
            }
        }

        public bool GetFlag(string flagName)
        {
            if (world != null && flagEntity.Id != 0)
            {
                var flagComponent = world.GetComponent<FlagComponent>(flagEntity);
                if (flagComponent != null && flagComponent.Flags.ContainsKey(flagName))
                {
                    return flagComponent.Flags[flagName];
                }
            }
            return globalFlags.ContainsKey(flagName) && globalFlags[flagName];
        }

        public bool HasFlag(string flagName)
        {
            if (world != null && flagEntity.Id != 0)
            {
                var flagComponent = world.GetComponent<FlagComponent>(flagEntity);
                if (flagComponent != null && flagComponent.Flags.ContainsKey(flagName))
                {
                    return true;
                }
            }
            return globalFlags.ContainsKey(flagName);
        }

        public void ClearFlag(string flagName)
        {
            if (globalFlags.ContainsKey(flagName))
            {
                globalFlags.Remove(flagName);
            }

            if (world != null && flagEntity.Id != 0)
            {
                var flagComponent = world.GetComponent<FlagComponent>(flagEntity);
                if (flagComponent != null && flagComponent.Flags.ContainsKey(flagName))
                {
                    flagComponent.Flags.Remove(flagName);
                }
            }
        }

        public void ClearAllFlags()
        {
            globalFlags.Clear();
            InitializeDefaultFlags();

            if (world != null && flagEntity.Id != 0)
            {
                var flagComponent = world.GetComponent<FlagComponent>(flagEntity);
                if (flagComponent != null)
                {
                    flagComponent.Flags.Clear();
                }
            }
        }

        public Dictionary<string, bool> GetAllFlags()
        {
            return globalFlags;
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        public void SetFlagEntity(Entity entity)
        {
            flagEntity = entity;
        }
    }
}

