using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MafiosoFearNode : Node
{
    private NavMeshAgent agent;
    private CMafiosoAI ai;
    private Vector3 currentVelocity;
    private Animator anim;
    public MafiosoFearNode (NavMeshAgent agent, CMafiosoAI ai,Animator Anim)
    {
        this.anim = Anim;
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {

        agent.isStopped = true;
        //TODO: Animation Fear;
        return NodeState.RUNNING;
    }
}
