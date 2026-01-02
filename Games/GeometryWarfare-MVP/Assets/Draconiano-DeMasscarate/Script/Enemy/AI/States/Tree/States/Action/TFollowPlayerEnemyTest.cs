using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Core.AI;

public class TFollowPlayerEnemyTest : CEnemyAction
{

	public override void OnStart()
	{
        if (_Player == null)
        {
            _Player = GameObject.FindGameObjectWithTag("Player");

            //throw new ArgumentNullException("El valor no puede ser null");
        }
    }

   

    public void FollowPlayer()
    {
        transform.position = Vector2.Lerp(transform.position, _Player.transform.position, _Speed * Time.deltaTime);

    }
    public override TaskStatus OnUpdate()
	{
        if (_Player != null)
        {
            FollowPlayer();
            return TaskStatus.Success;

        }
        else
        {
            return TaskStatus.Failure;
        }
        
	}
}