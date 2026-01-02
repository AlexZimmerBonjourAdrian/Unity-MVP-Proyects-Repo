using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

namespace Core.AI
{
    public class CMafiosoIsHighHealth : CEnemyConditional
    {

        public SharedInt HealthTreeHold;

        public override TaskStatus OnUpdate()
        {
           return DataEnemy.GetHealth() > HealthTreeHold.Value ? TaskStatus.Success : TaskStatus.Failure;
        }

    }
}
