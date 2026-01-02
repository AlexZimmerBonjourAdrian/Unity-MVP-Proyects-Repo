using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MafiosoDeathNode : Node
{

    private NavMeshAgent agent;
    private CMafiosoAI ai;
    private Transform target;

    private Vector3 currentVelocity;
    private float smoothDamp;
    private Transform SpawnBullet;
    private float BulletSpeed;
    //private int Damage = 20;

    [SerializeField]
    private GameObject bulletTrailPreb;
    //[SerializeField]
    //private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    [SerializeField]
    private TrailRenderer BulletTrail;
    //[SerializeField]
    //private float ShootDelay = 0.5f;
    [SerializeField]
    protected LayerMask Mask;

    private GameObject MuzzleToon;
    private int healthEnemy;

    //CRagdollController ragdollController;

    // Start is called before the first frame update
    // public MafiosoDeathNode(NavMeshAgent agent, CMafiosoAI ai, int healthEnemy,CRagdollController ragdollController)
    // {
    //     this.ragdollController = ragdollController;
    //     this.agent = agent;
    //     this.ai = ai;
    //     this.healthEnemy = healthEnemy;
    // }


    public override NodeState Evaluate()
    {
        //ragdollController.SetEnabled(true);
        return NodeState.SUCCESS;
    }
}
