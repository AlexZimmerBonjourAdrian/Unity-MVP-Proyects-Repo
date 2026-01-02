using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.AI;
using Zuzu;

public class TInstantDie : CEnemyAction
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
    public void AnimationInstantDead()
    {
        Debug.Log("Funcion de animacion");
    }
    public override TaskStatus OnUpdate()
	{
        if (_Enemy.GetComponent<CEnemy>().GetDead() == true)
        {
            AnimationInstantDead();
            return TaskStatus.Success;
        }
        else
        {
            return TaskStatus.Failure;
        }
    }
}