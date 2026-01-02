using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace Core.AI
{
    public class CMafiosoExistPatrolling : CEnemyConditional
    {


       
        public override TaskStatus OnUpdate()
        {
            return DataEnemy.getListPatrollingPoints() != null ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
