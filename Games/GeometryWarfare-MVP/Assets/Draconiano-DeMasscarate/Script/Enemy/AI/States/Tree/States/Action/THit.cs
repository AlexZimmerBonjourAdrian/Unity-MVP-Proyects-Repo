using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Core.AI;
using UnityEditor;
using Zuzu;

public class THit :	CEnemyAction
{
    BehaviorTree _BehaviorTree;
    SharedVariable _Variable;
    public GameObject Enemy;

    public override void OnStart()
	{
        _BehaviorTree = GetComponent<BehaviorTree>(); 
        _Variable = _BehaviorTree.GetVariable("ThisGameObject");
        _GameObject = (GameObject)_Variable.GetValue();
        
    }

    
    public void ExecuteAnimationHit()
    {

    }
	public override TaskStatus OnUpdate()
	{
        var Hit = _GameObject.GetComponent<CEnemy>().GetHit();

        if(Hit == true)
        {
            return TaskStatus.Running;
        }
        else
        {
            return TaskStatus.Success;
        }


    }
}