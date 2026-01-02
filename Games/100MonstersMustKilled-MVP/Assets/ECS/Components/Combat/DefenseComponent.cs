using ECS.Core;

namespace ECS.Components.Combat
{
    public class DefenseComponent : IComponent
    {
        public float BaseDefense { get; set; }
        public float CurrentDefense { get; set; }
        public bool IsDefending { get; set; }

        public DefenseComponent(float baseDefense)
        {
            BaseDefense = baseDefense;
            CurrentDefense = baseDefense;
            IsDefending = false;
        }
    }
}

