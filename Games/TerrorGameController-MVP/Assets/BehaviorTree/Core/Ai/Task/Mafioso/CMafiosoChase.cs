using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core.AI
{
    public class CMafiosoChase : CEnemyAction
    {
        [SerializeField]
        private NavMeshAgent navMeshAgent;
     
        public override TaskStatus OnUpdate()
        {
          float distance = Vector3.Distance(DataEnemy.getTrget().position,DataEnemy.getNavMeshAgent().transform.position);
          if (distance > 0.2f)
            {
                DataEnemy.getAnimator().SetTrigger("Run");
                DataEnemy.getNavMeshAgent().isStopped = false;
                DataEnemy.getNavMeshAgent().SetDestination(DataEnemy.getTrget().position);
                return TaskStatus.Running;
            }
            else
            {
               
                DataEnemy.getNavMeshAgent().isStopped = true;
                return TaskStatus.Success;
            }
        }

    }
}
