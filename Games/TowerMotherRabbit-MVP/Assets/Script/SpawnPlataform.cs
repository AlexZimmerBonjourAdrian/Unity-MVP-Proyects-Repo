using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlataform : MonoBehaviour
{
    public List<GameObject> SpanwPointS_List;

     public GameObject platformPrefab; // Asigna el prefab de la plataforma en el Inspector
    public float verticalOffset = -2f;

     [Range(0f, 1f)] // Esto crea un slider en el inspector para controlar la probabilidad
    public float spawnProbability = 0.7f;
    void Start()
    {
          //SpanwPointS_List = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "SpawnPoints")
            {
                SpanwPointS_List.Add(child.gameObject);
            }
        }
        // SpawnerPlataform(); 

    }


  private void SpawnerPlataform()
    {
         foreach (GameObject spawnPoint in SpanwPointS_List)
        {
            // Genera un número aleatorio entre 0 y 1
            //float randomValue = Random.value;

            // Solo spawnea la plataforma si el valor aleatorio es menor a la probabilidad
            // if (randomValue < spawnProbability)
            // {
                Vector3 spawnPosition = spawnPoint.transform.position + Vector3.down * verticalOffset;
                Instantiate(platformPrefab, spawnPosition, spawnPoint.transform.rotation);
         //   }
        }
    }
}
