using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Core.AI;
using UnityEditor;
using Zuzu;
public class TDie : CEnemyAction
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

    public void AnimationDead()
    {
        Debug.Log("Funcion de animacion");
    }
	public override TaskStatus OnUpdate()
	{
       // var Dead = _Enemy.GetComponent<CEnemy>().GetHit();
      if(_Enemy.GetComponent<CEnemy>().GetDead() == true)
        {
            AnimationDead();
            return TaskStatus.Success;
        }
       else
        {
            return TaskStatus.Failure;
        }
    }
}