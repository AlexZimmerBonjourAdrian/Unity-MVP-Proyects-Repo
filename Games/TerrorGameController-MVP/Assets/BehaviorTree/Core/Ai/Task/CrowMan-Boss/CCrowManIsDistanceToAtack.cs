using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace Core.AI
{
    public class CCrowManIsDistanceToAtack : CBossConditional
    {
        [SerializeField]
        public float range;
        [SerializeField]
        private Transform target;
        // Start is called before the first frame update
        public override void OnStart()
        {
            target = playerTransform;
        }
        public override TaskStatus OnUpdate()
        {
            float distance = Vector3.Distance(target.position, transform.position);
            return distance >= DataBoss.getMaxDistanceFollow() ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
