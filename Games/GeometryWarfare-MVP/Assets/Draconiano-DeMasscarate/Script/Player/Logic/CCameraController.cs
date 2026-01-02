using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCameraController : MonoBehaviour
{
    public Transform target;
    // private GameObject _Player;
    public float lerpSpeed;
    private Vector3 offset;
    private Vector3 targetPos;
    //private Vector2 _Move = Vector2.zero;
    

    // Start is called before the first frame update
    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;

            //throw new ArgumentNullException("El valor no puede ser null");
        }
        //offset = transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        //+offset
        targetPos = target.position;
        transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed * Time.deltaTime);
        // transform.position = _Player.transform.position;

    }
}
