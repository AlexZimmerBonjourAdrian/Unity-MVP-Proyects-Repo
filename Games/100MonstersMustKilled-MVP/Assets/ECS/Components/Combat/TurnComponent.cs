using ECS.Core;

namespace ECS.Components.Combat
{
    public enum TurnType
    {
        Player,
        Enemy
    }

    public class TurnComponent : IComponent
    {
        public TurnType CurrentTurn { get; set; }
        public bool HasActed { get; set; }

        public TurnComponent(TurnType initialTurn)
        {
            CurrentTurn = initialTurn;
            HasActed = false;
        }
    }
}

