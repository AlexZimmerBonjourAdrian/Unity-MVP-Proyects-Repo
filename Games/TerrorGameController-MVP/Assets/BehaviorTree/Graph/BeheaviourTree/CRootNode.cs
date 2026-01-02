using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRootNode : CNode
{
    // Start is called before the first frame update
    public CNode child;
    protected override void OnStart()
    {
     
    }

    protected override void OnStop()
    {
   
    }

    protected override State OnUpdate()
    {
        return child.Update();
    }
}
