using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core.AI
{
   
    public class MafiosoIsCovered : CEnemyAction
    {
        [SerializeField]
        private Transform origin;
        
        public override void OnStart()
        {
            origin = transform;
           
        }
        // Start is called before the first frame update

        public override TaskStatus OnUpdate()
        {
          
            if(DataEnemy.GetIsCover() == false)
                    return TaskStatus.Failure;
            else
            {
                DataEnemy.getAnimator().SetTrigger("Cover");
                return TaskStatus.Success;
            }
           
            
        }
    }

   
}
