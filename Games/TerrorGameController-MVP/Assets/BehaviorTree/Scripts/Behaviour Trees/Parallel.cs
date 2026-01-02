using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Node
{
    protected List<Node> nodes = new List<Node>();

  

    
    public Parallel(List<Node> nodes)
    {
        this.nodes = nodes;
    }
    public override NodeState Evaluate()
    {
        bool isOneNodeFailed = false;
        foreach (var node in nodes)
        {
           
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    break;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.SUCCESS;
                    break;
                case NodeState.FAILURE:
                    isOneNodeFailed = true;
                    return _nodeState;
                default:
                    break;
            }

        }
        _nodeState = isOneNodeFailed ? NodeState.FAILURE : NodeState.SUCCESS;
        return _nodeState;

    }

}
