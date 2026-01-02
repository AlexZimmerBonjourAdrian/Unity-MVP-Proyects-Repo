using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Core.AI;
using UnityEditor;
using Zuzu;

public class COPlayerInRange : CEnemyCondition
{
	

	[SerializeField]
	public float range;

    [SerializeField]
    public float MaxRange;

    public override void OnStart()
    {
        if (_Player == null)
        {
            _Player = GameObject.FindGameObjectWithTag("Player");
            //throw new ArgumentNullException("El valor no puede ser null");
        }
    }
    public override TaskStatus OnUpdate()
	{
        float distance = Vector3.Distance(_Player.transform.position, transform.position);
        return (distance <= range && distance >= MaxRange) ? TaskStatus.Success : TaskStatus.Failure;
    }
}