using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Core.AI;
using UnityEditor;
using Zuzu;
public class COCan : CEnemyCondition
{
    [SerializeField]
    private bool IsCan;

    public override TaskStatus OnUpdate()
	{
        return IsCan == true ? TaskStatus.Success : TaskStatus.Failure;
       
	}
}