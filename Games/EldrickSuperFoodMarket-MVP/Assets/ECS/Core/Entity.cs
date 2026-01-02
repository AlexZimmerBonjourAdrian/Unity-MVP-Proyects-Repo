using System;

namespace ECS.Core
{
    public struct Entity
    {
        public uint Id { get; private set; }

        public Entity(uint id)
        {
            Id = id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            return a.Id == b.Id;
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return a.Id != b.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Entity entity)
            {
                return Id == entity.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

