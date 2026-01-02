using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MafiosoFollowPlayer : Node
{
    private NavMeshAgent agent;
    private CMafiosoAI ai;
    private Transform target;

    private Vector3 currentVelocity;
    private float smoothDamp;
    private Animator AnimatorEnemy;
    public MafiosoFollowPlayer(NavMeshAgent agent, CMafiosoAI ai, Transform target,Animator animator)
    {

        this.agent = agent;
        this.ai = ai;
        this.target = target;
        this.AnimatorEnemy = animator;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        //ai.SetColor(Color.green);
        //Vector3 direction = target.position - ai.transform.position;
        //Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        //Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        //CManagerBullet.Inst.Spawn(SpawnBullet.position, ai.transform.forward * BulletSpeed, 0);

        
        return NodeState.SUCCESS;

    }
}

