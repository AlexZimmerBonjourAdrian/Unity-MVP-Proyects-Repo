using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Core.AI
{
    public class CCrowManDeath : CBossAction
    {
      public override void OnStart()
        {
            DataBoss.getAnimator().SetTrigger("Death");
            DataBoss.ListTeleport = new List<GameObject>();
           // DataBoss.getNavMeshAgent().enabled = false;
            DataBoss.transform.position = DataBoss.getDeathPosition().transform.position;
            DataBoss.transform.LookAt(DataBoss.getTrget(), Vector3.forward);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Running;
        }

    }
}
