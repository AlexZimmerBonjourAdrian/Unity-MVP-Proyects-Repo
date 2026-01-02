using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace Core.AI
{
    public class CEnemyAction : Action
    {
        protected Rigidbody body;
        protected float smoothDamp;
        // [SerializeField]
        // protected CRagdollController ragdoll;
        protected Vector3 currentVelocity;
        protected Transform playerTransform;




        [SerializeField]
        protected Transform targetEnemy;
        
        [SerializeField]
        protected float SmoothDamp = 3.0f;
        protected float BulletSpeed = 10;
        protected int Damage = 20;
       
        [SerializeField]
        protected GameObject bulletTrailPreb;
        [SerializeField]
        private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
        [SerializeField]
        protected ParticleSystem ShootingSystem;
        //[SerializeField]
        //protected ParticleSystem ImpactParticleSystem;

        //[SerializeField]
        //protected TrailRenderer BulletTrail;
        [SerializeField]
        protected float ShootDelay = 0.5f;
        [SerializeField]
        protected LayerMask Mask;

       
        [SerializeField]
        protected GameObject MuzzleToon;
        protected float timeSinceLastShot = 0f;

        protected GameObject playerHealth;
        [SerializeField] private float startingHealth;
        [SerializeField] private float lowHealthThreshold;
        [SerializeField] private float healthRestoreRate;
        private float _currentHealth;
        protected CDataEnemy DataEnemy;
    
        public float currentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
        }
        public override void OnAwake()
        {
            body = GetComponent<Rigidbody>();
            playerTransform = GameObject.Find("PlayerObj").transform;
           // ragdoll = GetComponent<CRagdollController>();
            playerHealth = GameObject.Find("PlayerObj");
            DataEnemy = GetComponent<CDataEnemy>();
            
            targetEnemy = DataEnemy.getTrget();
            
        }
    }
}
