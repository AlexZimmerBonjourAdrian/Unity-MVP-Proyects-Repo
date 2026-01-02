using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Core.AI
{
        public class CMafiosoFollow : CEnemyAction
        {
        [SerializeField]
        private NavMeshAgent navMeshAgent;
        [SerializeField]
        private float distance_Max;
        private Transform target;

        public override void OnStart()
        {
            navMeshAgent = DataEnemy.getNavMeshAgent();
            target = playerTransform;
        }
        public override TaskStatus OnUpdate()
        {
            float distance = Vector3.Distance(target.position, navMeshAgent.transform.position);
            if (distance > distance_Max)
            {
                //navMeshAgent.isStopped = false;
               // navMeshAgent.SetDestination(target.position);              
                return TaskStatus.Running;
            }
            else
            {
                navMeshAgent.isStopped = true;
                return TaskStatus.Success;
            }
        }
    }
}
