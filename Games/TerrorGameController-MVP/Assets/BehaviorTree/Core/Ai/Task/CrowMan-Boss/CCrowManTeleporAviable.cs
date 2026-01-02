using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Core.AI
{
        public class CCrowManTeleporAviable : CBossConditional
        {
        // Start is called before the first frame update
        public override TaskStatus OnUpdate()
        {
            return DataBoss.GetTeletransportPoints() != null ? TaskStatus.Success : TaskStatus.Failure;
        }
        
    }
}
