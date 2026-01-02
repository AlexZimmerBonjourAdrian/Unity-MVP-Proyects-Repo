using ECS.Core;

namespace ECS.Components.State
{
    public class StateComponent : IComponent
    {
        public bool IsAlive { get; set; }
        public bool IsInCombat { get; set; }
        public bool IsInDialogue { get; set; }
        public int MonstersKilled { get; set; }
        public int MonstersSpoken { get; set; }

        public StateComponent()
        {
            IsAlive = true;
            IsInCombat = false;
            IsInDialogue = false;
            MonstersKilled = 0;
            MonstersSpoken = 0;
        }
    }
}

