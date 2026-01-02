using ECS.Core;

namespace ECS.Components.Combat
{
    public enum CombatPatternType
    {
        Mobility,
        SolidPosition,
        Mixed
    }

    public class CombatPatternComponent : IComponent
    {
        public CombatPatternType PatternType { get; set; }
        public float Speed { get; set; }
        public float Difficulty { get; set; }

        public CombatPatternComponent(CombatPatternType patternType, float speed, float difficulty)
        {
            PatternType = patternType;
            Speed = speed;
            Difficulty = difficulty;
        }
    }
}

