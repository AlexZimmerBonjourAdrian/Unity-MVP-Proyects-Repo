using UnityEngine;
using ECS.Core;
using ECS.Systems;

namespace ECS
{
    public class ECSManager : MonoBehaviour
    {
        private World world;
        private CombatSystem combatSystem;
        private DialogueSystem dialogueSystem;
        private TimeSystem timeSystem;
        private SaveSystem saveSystem;

        private void Awake()
        {
            world = new World();
            
            combatSystem = new CombatSystem();
            dialogueSystem = new DialogueSystem();
            timeSystem = new TimeSystem();
            saveSystem = new SaveSystem();

            world.AddSystem(combatSystem);
            world.AddSystem(dialogueSystem);
            world.AddSystem(timeSystem);
            world.AddSystem(saveSystem);
        }

        private void Update()
        {
            if (world != null)
            {
                world.Update(Time.deltaTime);
            }
        }

        private void OnDestroy()
        {
            if (world != null)
            {
                world.Shutdown();
            }
        }

        public World GetWorld()
        {
            return world;
        }
    }
}

