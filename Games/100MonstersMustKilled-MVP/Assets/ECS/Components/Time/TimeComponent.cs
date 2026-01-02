using ECS.Core;

namespace ECS.Components.Time
{
    public class TimeComponent : IComponent
    {
        public int CurrentDay { get; set; }
        public int EncountersRemaining { get; set; }
        public int MaxEncounters { get; set; }
        public bool IsPaused { get; set; }

        public TimeComponent(int maxEncounters)
        {
            CurrentDay = 1;
            MaxEncounters = maxEncounters;
            EncountersRemaining = maxEncounters;
            IsPaused = false;
        }
    }
}

