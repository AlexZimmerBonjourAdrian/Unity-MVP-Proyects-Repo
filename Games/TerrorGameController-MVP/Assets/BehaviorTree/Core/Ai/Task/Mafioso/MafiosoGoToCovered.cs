using BehaviorDesigner.Runtime.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core.AI
{
    public class MafiosoGoToCovered : CEnemyAction
    {
        //[SerializeField]
        //protected NavMeshAgent navMeshAgent;
        [SerializeField]
        public MafiosoIsCovereAvaliable ai;
        public override TaskStatus OnUpdate()
        {
            Transform coverSpot =  ai.GetBestCoverSpot();
            if (coverSpot == null)
                return TaskStatus.Failure;
            //ai.SetColor(Color.blue);
            float distance = Vector3.Distance(coverSpot.position, DataEnemy.getNavMeshAgent().transform.position);
            if (DataEnemy.getIsDead() == false)
            {
                if (distance > 1f)
            {
                DataEnemy.getAnimator().SetTrigger("Run");
                DataEnemy.getNavMeshAgent().isStopped = false;
                DataEnemy.getNavMeshAgent().SetDestination(coverSpot.position);
                return TaskStatus.Running;
            }
            else
            {
                DataEnemy.getNavMeshAgent().isStopped = true;
                DataEnemy.SetIsCover(true); 
                return TaskStatus.Success;
            }
            }
            else
            {
                return TaskStatus.Failure;
            }
        }

      
    }
}
