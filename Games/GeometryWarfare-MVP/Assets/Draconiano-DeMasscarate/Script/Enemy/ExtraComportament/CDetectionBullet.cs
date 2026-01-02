using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class CDetectionBullet : MonoBehaviour
{

    public Vector2D _Move;
    
    public CircleCollider2D _CircleCollider2D;

    //public Vector2D ;
    public void Start()
    {
        _CircleCollider2D=GetComponent<CircleCollider2D>(); 
    }
    public float speed = 5f; // Velocidad de movimiento del enemigo
    GameObject bullet = null;
    void Update()
    {
     
        if (bullet != null)
        {
            Vector2 bulletDirection = bullet.transform.position - transform.parent.position;
            Vector2 evadeDirection = -bulletDirection.normalized;
            transform.position += (Vector3)evadeDirection * speed * Time.deltaTime;
        }
    }

    public CircleCollider2D getCircleCollider2D()
    { 
        return _CircleCollider2D; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            bullet = collision.gameObject;
            Debug.Log("Detecta la bala en la posición: " + bullet.transform.position);
        }
    }
}

