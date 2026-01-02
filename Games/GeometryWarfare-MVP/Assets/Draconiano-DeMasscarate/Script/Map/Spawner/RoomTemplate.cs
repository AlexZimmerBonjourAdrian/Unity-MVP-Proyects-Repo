using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class RoomTemplate : MonoBehaviour
{

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    private GameObject[] SpawnPointer;

    [SerializeField] 
    public GameObject layer;

    public GameObject RoomInicio;
    public GameObject RoomFinal;
    public List<GameObject> rooms;
    public bool complete = false;
    public GameObject boss;
    public GameObject closedRoom;
    private void Start()
    {
        layer = GameObject.FindGameObjectWithTag("Grid");
    }

    //public void checkFinish()
    //{
      
    //        //return complete = true;
    //        StartAdnEnd();
        
    //}
    
    public void StartAdnEnd()
    {
        RoomInicio = rooms[0];
        RoomFinal = rooms[rooms.Count - 1];
        SpawnPointer = GameObject.FindGameObjectsWithTag("SpawnPointer");

        if(SpawnPointer.Length <=0)
        {
            complete = true;
            SpawnBoss();
        }
        Debug.Log(SpawnPointer.Length);
    }

    public void SpawnBoss()
    {
        Instantiate(boss, RoomFinal.transform.position, RoomFinal.transform.rotation);
    }
    public GameObject GetLayer()

    {  return layer; }
}
