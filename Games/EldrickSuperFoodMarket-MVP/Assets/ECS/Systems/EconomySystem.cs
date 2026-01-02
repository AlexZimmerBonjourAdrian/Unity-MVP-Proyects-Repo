using ECS.Core;
using ECS.Components.Money;

namespace ECS.Systems
{
    public class EconomySystem : ISystem
    {
        private World world;
        private Entity playerEntity;

        public void Initialize(World world)
        {
            this.world = world;
        }

        public void Update(float deltaTime)
        {
            // Verificar si el jugador alcanzó la meta
            if (playerEntity.Id != 0 && HasReachedGoal())
            {
                // Notificar GameManager para Victoria
                if (Managers.GameManager.Instance != null)
                {
                    Managers.GameManager.Instance.Victory();
                }
            }
        }

        public void Shutdown()
        {
        }

        public void SetPlayerEntity(Entity entity)
        {
            playerEntity = entity;
        }

        public void AddMoney(float amount)
        {
            if (world == null || playerEntity.Id == 0) return;

            var money = world.GetComponent<MoneyComponent>(playerEntity);
            if (money == null) return;

            money.CurrentMoney += amount;
            money.DailyIncome += amount;
        }

        public void SubtractMoney(float amount)
        {
            if (world == null || playerEntity.Id == 0) return;

            var money = world.GetComponent<MoneyComponent>(playerEntity);
            if (money == null) return;

            money.CurrentMoney -= amount;
            
            // Asegurar que no sea negativo
            if (money.CurrentMoney < 0f)
            {
                money.CurrentMoney = 0f;
            }
        }

        public void ProcessDailyExpenses()
        {
            if (world == null || playerEntity.Id == 0) return;

            var money = world.GetComponent<MoneyComponent>(playerEntity);
            if (money == null) return;

            // Restar gastos diarios
            money.CurrentMoney -= money.DailyExpenses;
            
            // Si queda dinero después de gastos, añadirlo a ahorros
            if (money.CurrentMoney > 0f)
            {
                money.TotalSavings += money.CurrentMoney;
                money.CurrentMoney = 0f;
            }
            
            // Resetear ingresos del día
            money.DailyIncome = 0f;
        }

        public void AddToSavings(float amount)
        {
            if (world == null || playerEntity.Id == 0) return;

            var money = world.GetComponent<MoneyComponent>(playerEntity);
            if (money == null) return;

            money.TotalSavings += amount;
        }

        public bool HasReachedGoal()
        {
            if (world == null || playerEntity.Id == 0) return false;

            var money = world.GetComponent<MoneyComponent>(playerEntity);
            if (money == null) return false;

            return money.TotalSavings >= money.SavingsGoal;
        }

        public float GetCurrentMoney()
        {
            if (world == null || playerEntity.Id == 0) return 0f;

            var money = world.GetComponent<MoneyComponent>(playerEntity);
            if (money == null) return 0f;

            return money.CurrentMoney;
        }

        public float GetTotalSavings()
        {
            if (world == null || playerEntity.Id == 0) return 0f;

            var money = world.GetComponent<MoneyComponent>(playerEntity);
            if (money == null) return 0f;

            return money.TotalSavings;
        }
    }
}

