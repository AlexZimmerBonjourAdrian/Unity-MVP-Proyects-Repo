using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zuzu
{
    public class CAim : MonoBehaviour
    {
        public Vector2 Mouse;

        public float _speed = 50;

        void Update()
        {
            AimUpdate();
            transform.position = Mouse;

        }
        public void AimUpdate()
        {
            Vector3 _ScreenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Mouse = new Vector2(_ScreenToWorldPoint.x, _ScreenToWorldPoint.y);


        }
    }
}
  
