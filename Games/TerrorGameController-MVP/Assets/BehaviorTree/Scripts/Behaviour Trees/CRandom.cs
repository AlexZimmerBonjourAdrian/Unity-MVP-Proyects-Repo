using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRandom : Node
{
    // Start is called before the first frame update
    protected List<Node> nodes = new List<Node>();
    public override NodeState Evaluate()
    {
        int i;
        i = Random.Range(0, nodes.Count - 1);

        switch (nodes[i].Evaluate())
        {
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;
                return _nodeState;
            case NodeState.SUCCESS:
                return _nodeState;
            case NodeState.FAILURE:
                _nodeState = NodeState.FAILURE;
                break;
            default:
                break;
        }

        _nodeState = NodeState.SUCCESS;
        return _nodeState;
    }
}
