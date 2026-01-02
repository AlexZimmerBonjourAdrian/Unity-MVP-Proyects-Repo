using System.Collections.Generic;
using ECS.Core;

namespace ECS.Components.Dialogue
{
    public class FlagComponent : IComponent
    {
        public Dictionary<string, bool> Flags { get; set; }

        public FlagComponent()
        {
            Flags = new Dictionary<string, bool>();
        }
    }
}

