using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;

namespace Core.AI
{
    public class CCrowManIsRange : CBossConditional
    {
        // Start is called before the first frame update
       
        
            [SerializeField]
            public float range;
            [SerializeField]
            private Transform target;

            // Start is called before the first frame update
            public override void OnStart()
            {
                target = DataBoss.getTrget();
            }
            public override TaskStatus OnUpdate()
            {
                float distance = Vector3.Distance(target.position, transform.position);
                return distance < range ? TaskStatus.Success : TaskStatus.Failure;
            }
        }
}
