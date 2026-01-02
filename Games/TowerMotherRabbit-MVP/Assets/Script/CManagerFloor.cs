using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CManagerFloor : MonoBehaviour
{
    public static CManagerFloor Instance { get; private set; } // Singleton instance

    [SerializeField] private GameObject[] floorsPrefabs; // Array para almacenar los pisos
    // Velocidad de movimiento de los pisos

    [SerializeField] private List<GameObject> Floors_Lists;

        private int currentFloor = 0; // Índice del piso actual


     private void Awake()
    {
        // Singleton initialization
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private GameObject lastFloorInstantiated; // Índice del piso actual

     void Start()
    {
        // Instanciar el primer piso al inicio del juego
       lastFloorInstantiated = Instantiate(floorsPrefabs[0], transform.position, Quaternion.identity);
    }
    void Update()
    {

       
    }
    

    // Conecta el final del piso actual con el inicio del siguiente piso
    public void InstantiateNextFloor()
    {
        // Encuentra el punto final del último piso instanciado
        Transform endPoint = lastFloorInstantiated.transform.Find("EndPoint");

        // Calcula el índice del siguiente piso
        int nextFloorIndex = (currentFloor + 1) % floorsPrefabs.Length;

        // Instancia el siguiente piso en la posición y rotación del EndPoint
        lastFloorInstantiated = Instantiate(floorsPrefabs[nextFloorIndex], endPoint.position, endPoint.rotation);
        
        Floors_Lists.Add(lastFloorInstantiated);

        // Actualiza el índice del piso actual
        currentFloor = nextFloorIndex;
        if(Floors_Lists.Count > 2)
        {
            Destroy(Floors_Lists[0]);
            Floors_Lists.RemoveAt(0);
        }
    }
}
