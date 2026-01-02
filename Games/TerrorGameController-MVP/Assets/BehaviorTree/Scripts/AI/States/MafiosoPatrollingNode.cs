using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MafiosoPatrollingNode : Node
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator Anim;
    [SerializeField]
    private CMafiosoAI Mafioso;

    [SerializeField]
    private Transform[] transforms;
    public  MafiosoPatrollingNode(NavMeshAgent agent, Animator anim, CMafiosoAI mafioso, Transform[] transforms)
    {

        this.agent = agent;
        this.Anim = anim;
        this.Mafioso = mafioso;
        this.transforms = transforms;
    }

    public override NodeState Evaluate()
    {

        return NodeState.RUNNING;
    }


}
