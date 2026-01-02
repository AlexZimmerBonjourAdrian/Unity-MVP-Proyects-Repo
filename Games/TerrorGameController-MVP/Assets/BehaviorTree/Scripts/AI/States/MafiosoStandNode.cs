using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MafiosoStandNode : Node
{
    private NavMeshAgent agent;
    private CMafiosoAI ai;
    private int State;
    public MafiosoStandNode(NavMeshAgent agent, CMafiosoAI ai, Animator anim,int State)
    {
        this.ai = ai;
        this.agent = agent;
        this.State = State;
    }
    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        //TODO:Play Animation
        return NodeState.RUNNING;

    }

    private void PlayAnimation()
    {
        //Todo:Play Animation Not Repeat
    }
}
