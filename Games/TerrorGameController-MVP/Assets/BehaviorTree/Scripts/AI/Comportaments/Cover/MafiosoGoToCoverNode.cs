using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MafiosoGoToCoverNode : Node
{
    private NavMeshAgent agent;
    private CMafiosoAI AI;

    public MafiosoGoToCoverNode(NavMeshAgent agent, CMafiosoAI AI)
    {
        this.agent = agent;
        this.AI = AI;
    }
    public override NodeState Evaluate()
    {
        Transform coverSpot = AI.GetBestCoverSpot();
        if (coverSpot == null)
            return NodeState.FAILURE;
        //ai.SetColor(Color.blue);
        float distance = Vector3.Distance(coverSpot.position, agent.transform.position);
        if (distance > 0.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(coverSpot.position);
            return NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }

    // Start is called before the first frame update
  
}
