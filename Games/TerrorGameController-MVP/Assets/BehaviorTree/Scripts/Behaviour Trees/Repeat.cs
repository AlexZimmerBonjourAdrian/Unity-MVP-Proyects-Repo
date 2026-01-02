using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repeat : Node
{
    [SerializeField]int _Repeat = 0;

    protected Node Prevnode;

    public Repeat(Node Prevnode)
    {
        this.Prevnode = Prevnode;
    }
    public override NodeState Evaluate()
    {
      
       for(int i = _Repeat; i <= 0; i--)
        { 

            switch (Prevnode.Evaluate())
            {
                case NodeState.RUNNING:
                    break;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
                case NodeState.FAILURE:
                    break;
                default:
                    break;
            }

        }

       // _nodeState = isOneNodeSuccesFull ? NodeState.SUCCESS : NodeState.FAILURE;
        _nodeState = NodeState.SUCCESS;
        return _nodeState;

    }
}
