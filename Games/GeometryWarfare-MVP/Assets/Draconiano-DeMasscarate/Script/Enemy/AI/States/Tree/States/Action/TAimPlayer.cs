using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Core.AI;
using UnityEditor;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2;


public class TAimPlayer : CEnemyAction
{
    BehaviorTree _BehaviorTree;
    SharedVariable _Variable;
   

    public override void OnStart()
	{
        if (_Player == null)
        {
            _Player = GameObject.FindGameObjectWithTag("Player");

            //throw new ArgumentNullException("El valor no puede ser null");
        }
        
        _BehaviorTree = GetComponent<BehaviorTree>();
        _Variable = _BehaviorTree.GetVariable("RootWeapon");
        _ArmWeapon = (GameObject)_Variable.GetValue();





    }
	
	public void AimPlayer()
	{
		//Vector2 dirction = _Player.transform.position - _ArmWeapon.transform.position;
		//Vector2 currentDirection = Vector2.SmoothDamp(_ArmWeapon.transform.position, dirction, ref currentVelocity, 0.0005f);
        Vector2 rotation = _Player.transform.position - _ArmWeapon.transform.position;
        float rotz = Mathf.Atan2(-rotation.y, -rotation.x) * Mathf.Rad2Deg;
        // Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector2.right);
        //transform.rotation = Quaternion.Slerp(_ArmWeapon.transform.rotation, rotation, 0.5f);
        _ArmWeapon.transform.rotation = Quaternion.Euler(0, 0, rotz);
    }

	public override TaskStatus OnUpdate()
    {
      //  float distance = Vector3.Distance(_Player.transform.position, transform.position);
        if (_Player != null)
        {
            AimPlayer();
            return TaskStatus.Success;

        }
        else
        {
            return TaskStatus.Failure;
        }
     
	}
}