using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AI
{ 
    public class CEnemyConditional : Conditional
    {
        protected Rigidbody body;
       // protected CRagdollController ragdoll;
        protected Transform playerTransform;
        protected CDataEnemy DataEnemy;
        public override void OnAwake()
        {
            body = GetComponent<Rigidbody>();
            playerTransform = GameObject.Find("PlayerObj").transform;
           // ragdoll = GetComponent<CRagdollController>();
            DataEnemy = GetComponent<CDataEnemy>();
        }
    }
}
