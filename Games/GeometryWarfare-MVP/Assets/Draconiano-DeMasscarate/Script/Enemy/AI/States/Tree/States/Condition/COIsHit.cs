using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System;

public class COIsHit : CEnemyCondition
{
	[SerializeField]
	private bool m_isStrongBoss;

	



	public override TaskStatus OnUpdate()
	{
        return m_isStrongBoss == true ? TaskStatus.Failure : TaskStatus.Success;
    }
}