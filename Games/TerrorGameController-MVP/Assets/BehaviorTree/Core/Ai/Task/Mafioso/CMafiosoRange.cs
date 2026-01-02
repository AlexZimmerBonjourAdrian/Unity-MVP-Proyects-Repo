using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{
       public class CMafiosoRange : CEnemyConditional
       {
        [SerializeField]
        public float range;
        [SerializeField]
        private Transform target;

        // Start is called before the first frame update
        public override void OnStart()
        {
            target = DataEnemy.getTrget();
        }
        public override TaskStatus OnUpdate()
        {
            
                float distance = Vector3.Distance(target.position, transform.position);
                return distance <= range ? TaskStatus.Success : TaskStatus.Failure;
            
           

        }
    }
}
