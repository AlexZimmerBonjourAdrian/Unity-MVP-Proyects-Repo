using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
namespace Core.AI
{
    public class CCrowManPlayerRoud : CEnemyConditional
    {
        [SerializeField]
        private Transform target;
        // Start is called before the first frame update
        public override void OnStart()
        {
            target = playerTransform;
        }
        public override TaskStatus OnUpdate()
        {
            return target != null ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
