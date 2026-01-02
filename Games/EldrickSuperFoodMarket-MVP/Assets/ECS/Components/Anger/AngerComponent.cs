using ECS.Core;

namespace ECS.Components.Anger
{
    public class AngerComponent : IComponent
    {
        public float CurrentAnger { get; set; }
        public float MaxAnger { get; set; }
        public bool HasExploded { get; set; }

        public AngerComponent()
        {
            CurrentAnger = 0f;
            MaxAnger = 100f;
            HasExploded = false;
        }

        public float GetAngerPercentage()
        {
            return (CurrentAnger / MaxAnger) * 100f;
        }
    }
}

