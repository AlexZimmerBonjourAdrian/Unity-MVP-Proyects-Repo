using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MafiosoHearthNode : Node
{
    private CMafiosoAI Ai;
    private float threshold;
    public MafiosoHearthNode(CMafiosoAI ai, float threshold)
    {
        this.Ai = ai;
        this.threshold = threshold;
    }
    public override NodeState Evaluate()
    {
        return Ai.currentHealth <= threshold ? NodeState.SUCCESS : NodeState.FAILURE;
    }

    // Start is called before the first frame update
   
}
