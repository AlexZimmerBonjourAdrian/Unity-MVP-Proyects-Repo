using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
namespace Zuzu
{
        public class CBulletManager : MonoBehaviour
    {
        public List<CGenericBullet> _ListBullets = new List<CGenericBullet>();
        [SerializeField] private GameObject _bulletAsset;
        //public GameObject GenericBullet;

        //Singleton
        public static CBulletManager Inst
        {
            get
            {
                if (_inst == null)
                {
                    GameObject obj = new GameObject("BulletManager");
                    return obj.AddComponent<CBulletManager>();
                }
                return _inst;
            }
        }

        private static CBulletManager _inst;
        public void Start()
        {
            _inst = this;
        }
        private void Update()
        {
            //for(int i = _ListBullets.Count - 1; i>= 0; i--)
            //{
            //    if (_ListBullets[i] == null)
            //    {
            //        _ListBullets.RemoveAt(i);
            //    }
            //}
        }
        public void Spawn(Vector2 post,Vector2 Vel)
        {
            GameObject obj = (GameObject)Instantiate(_bulletAsset, post, Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().AddForce(Vel, ForceMode2D.Impulse);

            // Vector3 localScale = obj.transform.localScale;
            //localScale.x * =Rot
            CGenericBullet newBullet = obj.GetComponent<CGenericBullet>();
            //newBullet.AddVel(Vel);
          //  _bulletList.Add(newBullet);
            Destroy(obj, 3f);
        }
    }
}
