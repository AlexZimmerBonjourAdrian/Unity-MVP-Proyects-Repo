using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{
        public class CMafiosoIsProbably : CEnemyConditional
        {

        int probably;
        int result = Random.Range(0, 12);

        public override void OnStart()
        {
            
            Debug.Log(probably);
        }
        public override TaskStatus OnUpdate()
        {
            return probably == result ? TaskStatus.Success : TaskStatus.Failure;
        }


    }  
}

