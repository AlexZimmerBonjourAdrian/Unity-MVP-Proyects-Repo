using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Zuzu;
//using UnityEditor.Timeline.Actions;
using System;
public class COHIghtLife : CEnemyCondition
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

    private void CheckLife()
    {
        _LIfeEnemy = _Enemy.GetComponent<CEnemy>().GetLife();
    }
    public override TaskStatus OnUpdate()
	{
        CheckLife();
        if (_LIfeEnemy >= _MaxLife)
            return TaskStatus.Success;
        else
            return TaskStatus.Failure;

	}


}