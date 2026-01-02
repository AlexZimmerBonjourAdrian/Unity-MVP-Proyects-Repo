using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Core.AI;
using UnityEditor;

public class TEvading : CEnemyAction
{
	//Todo Evadir ataques el jugador con dash 

	private Vector2 Evasion;

	public override void OnStart()
	{
		
	}

	//public GameObject GetBullet()
	//{ return null; }

	//public void DetectionBullet()
	//{

	//}
	public override TaskStatus OnUpdate()
	{
		return TaskStatus.Success;
	}
}