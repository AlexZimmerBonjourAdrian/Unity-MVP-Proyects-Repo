using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
namespace Core.AI
{
    public class CCromManISDead : CBossConditional
    {
        // Start is called before the first frame update

        [SerializeField]
        private bool isDead;
        public override void OnStart()
        {
            base.OnStart();
        }

        public override TaskStatus OnUpdate()
        {
            return isDead == DataBoss.getIsDead() ? TaskStatus.Success : TaskStatus.Failure;
            //return base.OnUpdate();
        }
    }
}
