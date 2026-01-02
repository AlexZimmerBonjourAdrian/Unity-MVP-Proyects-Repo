using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core.AI
{
   public class CMafiosoIdle : CEnemyAction
    {
        public Animator animator;
        
        public override void OnStart()
        {
            animator = GetComponent<Animator>();
            if(DataEnemy.GetPlayingOneAnimation() == false)
            { 
                if((int)DataEnemy.getStateIdle() == 0 )
                {
                    animator.SetTrigger("Reset");
                    DataEnemy.SetPlayingOneAnimation(true);
                }
               else if((int)DataEnemy.getStateIdle() == 1)
                {
                    animator.SetTrigger("Sit");
                    DataEnemy.SetPlayingOneAnimation(true);
                }
                else
                {
                    animator.SetTrigger("Idle");
                    DataEnemy.SetPlayingOneAnimation(true);
                }
            }
        }

        public override TaskStatus OnUpdate()
        {
            return DataEnemy.getAlert() == false ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
