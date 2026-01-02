using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    private RoomTemplate templates;
    // Start is called before the first frame update
    private void Start()
    {
        templates = GameObject.FindGameObjectWithTag("room").GetComponent<RoomTemplate>();
        templates.rooms.Add(this.gameObject);
    }
}

