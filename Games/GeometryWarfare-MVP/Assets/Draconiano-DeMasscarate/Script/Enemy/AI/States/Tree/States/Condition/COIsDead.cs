using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class COIsDead : CEnemyCondition
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
      
		return TaskStatus.Success;
	}
}