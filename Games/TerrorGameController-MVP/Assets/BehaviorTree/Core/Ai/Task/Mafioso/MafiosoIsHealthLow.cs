using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

using UnityEngine;

namespace Core.AI
{
    public class MafiosoIsHealthLow : CEnemyConditional
    {

        public SharedInt HealthTreesHold;

        public override TaskStatus OnUpdate()
        {
            return DataEnemy.GetHealth() <= HealthTreesHold.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
        //public SharedInt
    }
}
