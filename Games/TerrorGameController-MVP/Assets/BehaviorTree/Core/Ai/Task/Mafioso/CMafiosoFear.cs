using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using UnityEngine;
namespace Core.AI
{
    public class CMafiosoFear : CEnemyAction
    {
        
        public override TaskStatus OnUpdate()
        {
            if(DataEnemy.getIsDead() == false)
            {
                DataEnemy.getAnimator().SetTrigger("Fear");
                DataEnemy.getNavMeshAgent().isStopped = true;
                return TaskStatus.Running;
            }
            else
            {
                return TaskStatus.Failure;
            }
           
        }
    }
}
