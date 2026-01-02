using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{

    public class CMafiosoIsCovered : CEnemyAction
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
            //RaycastHit hit;
            if (DataEnemy.GetIsCover() == false)
                return TaskStatus.Success;
            else
            {
                //DataEnemy.getAnimator().SetTrigger("Cover");
                return TaskStatus.Failure;
            }

        }
    }
  

}

