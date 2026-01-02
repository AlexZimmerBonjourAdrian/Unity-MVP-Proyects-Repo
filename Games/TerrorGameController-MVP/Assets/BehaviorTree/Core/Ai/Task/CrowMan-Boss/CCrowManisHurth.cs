using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Core.AI
{
    public class CCrowManisHurth : CBossConditional
    {
        [SerializeField]
        public bool IsInvulnerable_Comprobate;

        public override void OnStart()
        {
            //DataBoss.getAnimator().SetTrigger("Hit");
        }
        public override TaskStatus OnUpdate()
       {  
                return IsInvulnerable_Comprobate == DataBoss.GetIsInvulnerable() ? TaskStatus.Success : TaskStatus.Failure;
       }
    }
}
