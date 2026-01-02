using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using Core.AI;
using UnityEngine.AI;
namespace Core.CrowMan.AI
{
    public class CCrowManFollow : CBossAction
    {
        [SerializeField]
        private NavMeshAgent navMeshAgent;
        [SerializeField]
        private float distance_Max;
        private Transform target;

        public override void OnStart()
        {
            navMeshAgent = DataBoss.getNavMeshAgent();
            target = playerTransform;
            navMeshAgent.enabled = true;
            DataBoss.getNavMeshAgent().isStopped = false;
            //if (DataBoss.getAnimator().GetCurrentAnimatorStateInfo(0).IsName("Running Knife"))
            //{ 

            //}
        }
        public override TaskStatus OnUpdate()
        {
         
                Debug.Log("Run Knife");
                target = playerTransform;
                float distance = Vector3.Distance(playerTransform.position, DataBoss.getNavMeshAgent().transform.position);
                if (distance > 3f)
                {
                    // Debug.Log(distance);
                    DataBoss.getNavMeshAgent().isStopped = false;
                    DataBoss.getNavMeshAgent().SetDestination(playerTransform.position);
                    DataBoss.getNavMeshAgent().speed = 3f;
                    AnimatorStateInfo currentState = DataBoss.Anim.GetCurrentAnimatorStateInfo(0);
                if(!currentState.IsName("RunKnife"))
                {
                    DataBoss.getAnimator().Play("RunKnife");
                }
                    Vector3 direction = playerTransform.position - transform.localPosition;
                    Vector3 currentDirection = Vector3.SmoothDamp(transform.forward, direction, ref currentVelocity, 0.5f);
                    Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, rotation, 0.5f);
                    return TaskStatus.Running;
                }
                else
                {
                    DataBoss.getNavMeshAgent().isStopped = true;
                    return TaskStatus.Success;
                }
            
           

        }
    }

}
