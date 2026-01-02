using UnityEngine;
using ECS.Core;
using ECS.Systems;

namespace ECS
{
    public class ECSManager : MonoBehaviour
    {
        private World world;
        private CustomerSystem customerSystem;
        private OrderSystem orderSystem;
        private AngerSystem angerSystem;
        private EconomySystem economySystem;
        private DialogueSystem dialogueSystem;

        private void Awake()
        {
            world = new World();
            
            customerSystem = new CustomerSystem();
            orderSystem = new OrderSystem();
            angerSystem = new AngerSystem();
            economySystem = new EconomySystem();
            dialogueSystem = new DialogueSystem();

            world.AddSystem(customerSystem);
            world.AddSystem(orderSystem);
            world.AddSystem(angerSystem);
            world.AddSystem(economySystem);
            world.AddSystem(dialogueSystem);
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

        public CustomerSystem GetCustomerSystem()
        {
            return customerSystem;
        }

        public OrderSystem GetOrderSystem()
        {
            return orderSystem;
        }

        public AngerSystem GetAngerSystem()
        {
            return angerSystem;
        }

        public EconomySystem GetEconomySystem()
        {
            return economySystem;
        }

        public DialogueSystem GetDialogueSystem()
        {
            return dialogueSystem;
        }
    }
}

