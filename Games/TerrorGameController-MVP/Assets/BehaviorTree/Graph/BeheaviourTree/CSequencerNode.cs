using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSequencerNode : CCompositeNode
{
    int current;
    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
     
    }

    protected override State OnUpdate()
    {
        var child = children[current];
        //switch(child.GetHashCode-)
        switch(child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Faiulure:
                return State.Faiulure;
            case State.Success:
                current++;
                break;
        }
        return current == children.Count ? State.Success : State.Running;
    }
}
