using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace Core.AI
{
    public class CBossAction : Action
    {
        protected Rigidbody body;
        protected float smoothDamp;
        //[SerializeField]
        //protected CRagdollController ragdoll;
        protected Vector3 currentVelocity;
        protected Transform playerTransform;
        protected float timeSinceLastShot = 0f;
        //[SerializeField]
        //protected Transform SpawnBullet;

        //[SerializeField]
        //protected GameObject bulletTrailPreb;
        [SerializeField]
        protected Transform targetBoss;

        protected CDataBoss DataBoss;


        public override void OnAwake()
        {
            body = GetComponent<Rigidbody>();
            playerTransform = GameObject.Find("PlayerObj").transform;
            //ragdoll = GetComponent<CRagdollController>();
            DataBoss = GetComponent<CDataBoss>();
            targetBoss = DataBoss.getTrget();

        }
    }
}
