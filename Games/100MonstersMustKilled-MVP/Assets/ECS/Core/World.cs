using System;
using System.Collections.Generic;

namespace ECS.Core
{
    public class World
    {
        private uint nextEntityId = 1;
        private Dictionary<Entity, Dictionary<Type, IComponent>> entities;
        private List<ISystem> systems;

        public World()
        {
            entities = new Dictionary<Entity, Dictionary<Type, IComponent>>();
            systems = new List<ISystem>();
        }

        public Entity CreateEntity()
        {
            Entity entity = new Entity(nextEntityId++);
            entities[entity] = new Dictionary<Type, IComponent>();
            return entity;
        }

        public void DestroyEntity(Entity entity)
        {
            if (entities.ContainsKey(entity))
            {
                entities.Remove(entity);
            }
        }

        public void AddComponent<T>(Entity entity, T component) where T : class, IComponent
        {
            if (!entities.ContainsKey(entity))
            {
                entities[entity] = new Dictionary<Type, IComponent>();
            }

            entities[entity][typeof(T)] = component;
        }

        public T GetComponent<T>(Entity entity) where T : class, IComponent
        {
            if (entities.ContainsKey(entity) && entities[entity].ContainsKey(typeof(T)))
            {
                return entities[entity][typeof(T)] as T;
            }
            return null;
        }

        public bool HasComponent<T>(Entity entity) where T : class, IComponent
        {
            return entities.ContainsKey(entity) && entities[entity].ContainsKey(typeof(T));
        }

        public void RemoveComponent<T>(Entity entity) where T : class, IComponent
        {
            if (entities.ContainsKey(entity) && entities[entity].ContainsKey(typeof(T)))
            {
                entities[entity].Remove(typeof(T));
            }
        }

        public IEnumerable<Entity> GetEntities()
        {
            return entities.Keys;
        }

        public IEnumerable<Entity> GetEntitiesWithComponent<T>() where T : class, IComponent
        {
            foreach (var entity in entities.Keys)
            {
                if (HasComponent<T>(entity))
                {
                    yield return entity;
                }
            }
        }

        public void AddSystem(ISystem system)
        {
            systems.Add(system);
            system.Initialize(this);
        }

        public void RemoveSystem(ISystem system)
        {
            if (systems.Contains(system))
            {
                system.Shutdown();
                systems.Remove(system);
            }
        }

        public void Update(float deltaTime)
        {
            foreach (var system in systems)
            {
                system.Update(deltaTime);
            }
        }

        public void Shutdown()
        {
            foreach (var system in systems)
            {
                system.Shutdown();
            }
            systems.Clear();
            entities.Clear();
        }
    }
}

