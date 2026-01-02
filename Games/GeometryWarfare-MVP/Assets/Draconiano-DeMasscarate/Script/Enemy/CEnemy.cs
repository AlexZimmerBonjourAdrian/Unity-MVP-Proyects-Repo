using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Zuzu
{

    public class CEnemy : MonoBehaviour, IDamage
    {
        //private GameObject _Player;
        //private  float _Speed = 1;
        //private Vector2 _Move = Vector2.zero;
        public float _life = 100;
        public bool _IsDead = false;
        public bool _IsHit = false;
        public float _HitTime = .2f;

        public SpriteRenderer _spriteRenderer;

        public Color _Red = Color.red;
        public Color _Yellow = Color.yellow;

        public GameObject _gameObject;

        public CDetectionBullet _collisionBullet;
        public void OnDamage(float damage)
        {
            SetLife(GetLife() - damage);
            CheckLife();
            SetHit(true);
        }

        public void SetLife(float Life)
        {
            _life = Life;

        }
        public float GetLife()
        {
            return _life;

        }

        public void SetHit(bool IsHit)
        {
            _IsHit = IsHit;

        }
        public bool GetHit()
        {
            return _IsHit;

        }

        public void SetDead(bool IsDead)
        {
            _IsDead = IsDead;

        }
        public bool GetDead()
        {
            return _IsDead;

        }

        // Start is called before the first frame update
        void Start()
        {
        
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collisionBullet = GetComponentInChildren<CDetectionBullet>();
                //if (_Player == null)
                //{ 
                //    _Player = GameObject.FindGameObjectWithTag("Player");
              
                //     //throw new ArgumentNullException("El valor no puede ser null");
                //}
        
            
        //catch (ArgumentNullException e)
        //  { 
        //        Debug.LogError("Error: el player no existe: " + e.Message);
        // }
    }

    // Update is called once per frame

        public void ChangeHit()
        {

            SetHit(false);
            _spriteRenderer.color = _Red;
        }
    public  void ReciveDamage()
     {
        if(_IsHit == true)
         {
                _spriteRenderer.color = _Yellow;
                Invoke("ChangeHit", _HitTime);
                
         }
        else
         {
                
                Debug.Log("Esta bien");
         }
        
     }

    public void CheckLife()
    {
        if(_life<= 0)
        {
                Debug.Log("El Enemigo Esta muerto");
                _IsDead = true;
                Destroy(gameObject);
        }
        else
        {
                Debug.Log("El Enemigo Esta vivo");
        }
    
    }

     
    }
}