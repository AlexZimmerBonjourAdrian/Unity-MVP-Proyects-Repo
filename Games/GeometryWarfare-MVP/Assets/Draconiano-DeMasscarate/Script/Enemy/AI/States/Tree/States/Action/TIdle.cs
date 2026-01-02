using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Core.AI;
using UnityEditor;
//using static BehaviorDesigner.Runtime.BehaviorManager;

public class TIdle : CEnemyAction
{
    BehaviorTree _BehaviorTree;
    SharedVariable _Variable;

    //[SerializeField]
    //bool _IsAlert;
    public override void OnStart()
	{
        _BehaviorTree = GetComponent<BehaviorTree>();
        _Variable = _BehaviorTree.GetVariable("IsAlert");
        _IsAlert = (bool)_Variable.GetValue();
    }

    //Sketch Animation Idle Function
    //public void AnimationIdle()
    //{

    //}
    public void SetIsAlert(bool IsAlert)
    {  _IsAlert = IsAlert; }

    public bool GetIsAlert()
    {
        return _IsAlert;
    }
	public override TaskStatus OnUpdate()
	{

        return _IsAlert == false ? TaskStatus.Running : TaskStatus.Failure;
    }
}