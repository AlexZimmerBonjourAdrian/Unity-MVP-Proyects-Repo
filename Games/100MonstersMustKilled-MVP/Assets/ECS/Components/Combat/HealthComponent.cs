using ECS.Core;

namespace ECS.Components.Combat
{
    public class HealthComponent : IComponent
    {
        public float CurrentHealth { get; set; }
        public float MaxHealth { get; set; }
        public bool IsDead { get; set; }

        public HealthComponent(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            IsDead = false;
        }
    }
}

