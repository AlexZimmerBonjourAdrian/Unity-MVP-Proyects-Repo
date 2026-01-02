using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CDecoratorNode : CNode
{
    [HideInInspector] public CNode child;

}
