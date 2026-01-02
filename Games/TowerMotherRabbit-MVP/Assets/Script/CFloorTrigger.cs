using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFloorTrigger : MonoBehaviour
{
  private void OnTriggerEnter(Collider other) 
  {
 
        Debug.Log("Trigger entered");
        // Verifica si el objeto que entró al trigger es el jugador (ajusta la etiqueta si es necesario)
       
        CManagerFloor.Instance.InstantiateNextFloor();
             // Destruye el trigger después de un uso
        
    }
   }


