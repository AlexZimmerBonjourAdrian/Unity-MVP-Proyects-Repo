using ECS.Core;
using ECS.Components.Time;
using System.Collections.Generic;

namespace ECS.Systems
{
    public class TimeSystem : ISystem
    {
        private World world;

        public void Initialize(World world)
        {
            this.world = world;
        }

        public void Update(float deltaTime)
        {
        }

        public void Shutdown()
        {
        }
    }
}

