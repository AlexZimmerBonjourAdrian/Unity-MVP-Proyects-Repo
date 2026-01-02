using ECS.Core;

namespace ECS.Components.Money
{
    public class MoneyComponent : IComponent
    {
        public float CurrentMoney { get; set; }
        public float TotalSavings { get; set; }
        public float DailyIncome { get; set; }
        public float DailyExpenses { get; set; }
        public float SavingsGoal { get; set; }

        public MoneyComponent()
        {
            CurrentMoney = 0f;
            TotalSavings = 0f;
            DailyIncome = 0f;
            DailyExpenses = 325f; // $325 según MVP
            SavingsGoal = 5000f; // $5,000 según MVP
        }
    }
}

