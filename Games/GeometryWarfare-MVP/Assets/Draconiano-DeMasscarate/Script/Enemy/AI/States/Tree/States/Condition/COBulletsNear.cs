using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zuzu;
//using UnityEditor.Timeline.Actions;
using System;
public class COBulletsNear : Conditional
{

    BehaviorTree _BehaviorTree;
    SharedVariable _Variable;
    public GameObject _Enemy;

    public override void OnStart()
    {
        _BehaviorTree = GetComponent<BehaviorTree>();
        _Variable = _BehaviorTree.GetVariable("ThisGameObject");
        _Enemy = (GameObject)_Variable.GetValue();

    }

    
    public override TaskStatus OnUpdate()
	{
        //if (g) 
        //{
            return TaskStatus.Success;
        //}

		
	}
}