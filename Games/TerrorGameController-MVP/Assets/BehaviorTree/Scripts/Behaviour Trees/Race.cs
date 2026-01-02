using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Race : Node
{
    // Start is called before the first frame update
    protected List<Node> nodes = new List<Node>();
    public Race(List<Node> nodes)
    {
        this.nodes = nodes;
    }
    public override NodeState Evaluate()
    {
        bool isOneNodeSuccesFull= false;
        foreach (var node in nodes)
        {

            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    break;
                case NodeState.SUCCESS:
                    
                    isOneNodeSuccesFull = true;
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                default:
                    break;
            }

        }
        _nodeState = isOneNodeSuccesFull ? NodeState.SUCCESS : NodeState.FAILURE;
        return _nodeState;

    }
}
