using UnityEngine;
using ECS.Core;
using ECS.Components.State;
using ECS.Components.Dialogue;

namespace Managers
{
    public enum EndingType
    {
        None,
        Selfish,
        Heroic
    }

    public class EndingManager : MonoBehaviour
    {
        private static EndingManager instance;
        public static EndingManager Instance { get { return instance; } }

        private World world;
        private EndingType currentEnding;
        private bool endingTriggered;

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
            currentEnding = EndingType.None;
            endingTriggered = false;
        }

        private void Start()
        {
        }

        private void Update()
        {
        }

        public void CheckEndingConditions()
        {
        }

        public void TriggerEnding(EndingType ending)
        {
        }

        public void ShowEnding(EndingType ending)
        {
        }

        public EndingType GetCurrentEnding()
        {
            return currentEnding;
        }

        public bool HasEndingTriggered()
        {
            return endingTriggered;
        }

        public void SetWorld(World world)
        {
            this.world = world;
        }

        private bool CheckSelfishEnding()
        {
            return false;
        }

        private bool CheckHeroicEnding()
        {
            return false;
        }
    }
}

