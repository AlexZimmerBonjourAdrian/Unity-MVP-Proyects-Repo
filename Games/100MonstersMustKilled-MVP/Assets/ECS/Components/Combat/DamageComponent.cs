using ECS.Core;

namespace ECS.Components.Combat
{
    public class DamageComponent : IComponent
    {
        public float BaseDamage { get; set; }
        public float CurrentDamage { get; set; }

        public DamageComponent(float baseDamage)
        {
            BaseDamage = baseDamage;
            CurrentDamage = baseDamage;
        }
    }
}

