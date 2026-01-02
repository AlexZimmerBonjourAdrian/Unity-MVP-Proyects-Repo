using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRepeatNode : CDecoratorNode
{
    // Start is called before the first frame update
    protected override void OnStart()
    {
       
    }

    protected override void OnStop()
    {
      
    }

    protected override State OnUpdate()
    {
        child.Update();
        return State.Running;
    }
}
