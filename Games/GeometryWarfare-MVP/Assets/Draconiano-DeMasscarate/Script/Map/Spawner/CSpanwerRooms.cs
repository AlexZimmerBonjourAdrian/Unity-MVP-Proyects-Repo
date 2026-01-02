using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSpanwerRooms : MonoBehaviour
{

  
    private GameObject[] SpawnPointer;
   // private bool spawned = false;

    void Start()
    {

        Invoke("CheckCollision", 0.1f);
        SpawnPointer = GameObject.FindGameObjectsWithTag("SpawnPointer");
       for (int i = SpawnPointer.Length - 1; i >= 0; i--)
        {
           if(!(SpawnPointer[i].transform == transform.parent))
            {
                SpawnPointer.ToList().RemoveAt(i);
            }
        }
       
    }
        public void CheckCollision()
        {
       
           { 
            for (int i = 0; i < SpawnPointer.Length; i++)
            {
                Debug.Log(SpawnPointer.Length);
                if (SpawnPointer[i] != null)
                {
                    var collider = SpawnPointer[i].GetComponent<Collider2D>();
                
            //  float radius = collider.size.x / 2; // Asume que el collider es un círculo
            /* Physics2D.OverlapBoxAll(SpawnPointer[i].transform.position, radius);*/

            //foreach (var col in collider)
            //{
            //    if (col.gameObject != SpawnPointer[i] && col.gameObject.tag == "Rooms")
            //    {
            //        Debug.Log("El objeto " + SpawnPointer[i].name + " está colisionando con " + col.gameObject.name);

            //}
                if(collider.CompareTag("Rooms"))
                {
                    
                        Destroy(collider.gameObject);
                        
                }

                   // }
               }
                
            }
        }
    }
}
