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
        }

        public bool GetFlag(string flagName)
        {
            return globalFlags.ContainsKey(flagName) && globalFlags[flagName];
        }

        public bool HasFlag(string flagName)
        {
            return globalFlags.ContainsKey(flagName);
        }

        public void ClearFlag(string flagName)
        {
        }

        public void ClearAllFlags()
        {
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

