using BehaviorDesigner.Runtime.Tasks.Unity.UnityLayerMask;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build;
using UnityEngine;
using Zuzu;

public class CNormalBullet : CGenericBullet
{
    private Rigidbody2D _Rig;
    private float _Damage = 30;
    private void SpeedAddImpulse()
    {
        _Rig = GetComponent<Rigidbody2D>();
      
    }

    private void Update()
    {
        SpeedAddImpulse();
    }

    public override void AddVel(Vector3 vel)
    {
        _Rig.AddForce(vel, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        int layer = LayerMask.NameToLayer("Collisionable");
        if (other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("Player"))
        {
            //if (other.gameObject != null)
            //{
               other.gameObject.GetComponent<IDamage>().OnDamage(_Damage);
               
                 Debug.LogWarning("Collision Enemy");
                Destroy(gameObject,0f);    
            //}
        }
         else if(other.gameObject.layer == layer )
        {
            Debug.LogWarning("No es un enemigo");
            Destroy(gameObject, 0f);
        }
        Destroy(gameObject, 5f);



    }

   
}
