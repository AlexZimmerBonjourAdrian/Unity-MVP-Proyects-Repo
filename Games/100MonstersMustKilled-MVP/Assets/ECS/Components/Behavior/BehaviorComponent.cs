using ECS.Core;

namespace ECS.Components.Behavior
{
    public enum BehaviorType
    {
        Aggressive,
        Defensive,
        Passive,
        Surrender
    }

    public class BehaviorComponent : IComponent
    {
        public BehaviorType CurrentBehavior { get; set; }
        public float AggressionLevel { get; set; }
        public bool CanSurrender { get; set; }

        public BehaviorComponent(BehaviorType initialBehavior, float aggressionLevel)
        {
            CurrentBehavior = initialBehavior;
            AggressionLevel = aggressionLevel;
            CanSurrender = false;
        }
    }
}

