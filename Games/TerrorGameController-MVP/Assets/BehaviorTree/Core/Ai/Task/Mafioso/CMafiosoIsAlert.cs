using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.AI
{
    public class CMafiosoIsAlert : CEnemyConditional
    {
        public override void OnStart()
        {
              if (DataEnemy.getAlert() == true)
              {
                DataEnemy.getAnimator().SetTrigger("Restart");
              }
        }
       
        public bool IsAlert;
        public override TaskStatus OnUpdate()
        {
            return DataEnemy.getAlert() == IsAlert ? TaskStatus.Success : TaskStatus.Failure;
        }


    }
}
