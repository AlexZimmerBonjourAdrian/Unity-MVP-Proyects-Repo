using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MafiosoChaseNode : Node
{
    // Start is called before the first frame update
    private Transform target;
    private NavMeshAgent agent;
    private CMafiosoAI ai;

    public MafiosoChaseNode(Transform target, NavMeshAgent agent, CMafiosoAI aI)
    {
        this.target = target;
        this.agent = agent;
        this.ai = aI;

    }

    public override NodeState Evaluate()
    {
        //ai.SetColor(Color.yellow);
        float distance = Vector3.Distance(target.position, agent.transform.position);
        if (distance > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            return NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }
}
