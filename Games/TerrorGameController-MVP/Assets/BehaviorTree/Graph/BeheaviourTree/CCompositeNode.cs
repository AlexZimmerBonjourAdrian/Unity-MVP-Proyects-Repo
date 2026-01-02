using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CCompositeNode : CNode
{
    [HideInInspector] public CNode child;
    public List<CNode> children = new List<CNode>();
}
