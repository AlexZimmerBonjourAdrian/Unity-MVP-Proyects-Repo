using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.AI
{
    public class CBossConditional : Conditional
    {
        protected Rigidbody body;
        //protected CRagdollController ragdoll;
        protected Transform playerTransform;
        protected CDataBoss DataBoss;
       public override void OnAwake()
        {
            body = GetComponent<Rigidbody>();
            playerTransform = GameObject.Find("PlayerObj").transform;
            //ragdoll = GetComponent<CRagdollController>();
            DataBoss = GetComponent<CDataBoss>();
        }
    }
}
