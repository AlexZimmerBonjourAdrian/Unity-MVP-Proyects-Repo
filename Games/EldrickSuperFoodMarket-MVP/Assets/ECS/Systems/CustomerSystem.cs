using ECS.Core;
using ECS.Components.Customer;
using System.Collections.Generic;

namespace ECS.Systems
{
    public class CustomerSystem : ISystem
    {
        private World world;
        private List<Entity> activeCustomers;

        public void Initialize(World world)
        {
            this.world = world;
            activeCustomers = new List<Entity>();
        }

        public void Update(float deltaTime)
        {
            // TODO: Implementar lógica de gestión de clientes
        }

        public void Shutdown()
        {
            activeCustomers.Clear();
        }

        public Entity CreateCustomer(CustomerType type, PersonalityType personality)
        {
            if (world == null) return new Entity(0);

            // Crear entidad de cliente
            Entity customer = world.CreateEntity();

            // Crear CustomerComponent
            var customerComponent = new CustomerComponent
            {
                Type = type,
                Personality = personality,
                Name = GetCustomerName(type),
                HasBeenServed = false,
                WillGiveTip = personality == PersonalityType.Friendly && UnityEngine.Random.Range(0f, 1f) < 0.5f
            };

            // Añadir componente a la entidad
            world.AddComponent<CustomerComponent>(customer, customerComponent);

            // Añadir a lista de clientes activos
            activeCustomers.Add(customer);

            return customer;
        }

        public void SpawnCustomer()
        {
            if (world == null) return;

            // Generar tipo y personalidad aleatorios
            CustomerType type = (CustomerType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(CustomerType)).Length);
            PersonalityType personality = (PersonalityType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(PersonalityType)).Length);

            // Crear cliente
            CreateCustomer(type, personality);
        }

        public void RemoveCustomer(Entity customer)
        {
            if (world == null) return;

            // Eliminar de lista de clientes activos
            if (activeCustomers.Contains(customer))
            {
                activeCustomers.Remove(customer);
            }

            // Destruir entidad del world
            world.DestroyEntity(customer);
        }

        private string GetCustomerName(CustomerType type)
        {
            switch (type)
            {
                case CustomerType.Lovecraftian:
                    return "Cthulhu";
                case CustomerType.Angel:
                    return "Serafín";
                case CustomerType.Demon:
                    return "Demonio Menor";
                default:
                    return "Cliente";
            }
        }

        public List<Entity> GetActiveCustomers()
        {
            return activeCustomers;
        }
    }
}

