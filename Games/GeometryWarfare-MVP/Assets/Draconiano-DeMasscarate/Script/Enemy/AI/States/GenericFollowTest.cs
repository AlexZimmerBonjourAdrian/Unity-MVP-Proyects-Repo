using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericFollowTest : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject _Player;
    private float _Speed = 1;
    private Vector2 _Move = Vector2.zero;
    void Start()
    {
        if (_Player == null)
        {
            _Player = GameObject.FindGameObjectWithTag("Player");
            //throw new ArgumentNullException("El valor no puede ser null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_Player != null)
        {
            FollowPlayer();
            
        }

    }

    public void FollowPlayer()
    {
        transform.position = Vector2.Lerp(transform.position, _Player.transform.position, _Speed * Time.deltaTime);

    }
}
