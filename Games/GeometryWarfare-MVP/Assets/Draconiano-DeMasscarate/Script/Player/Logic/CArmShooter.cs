using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Jobs;
using Zuzu;
//using static UnityEditor.Experimental.GraphView.GraphView;

//namespace Draconiano_PC
//{
//    public class CArmShooter : MonoBehaviour
//    {

//        private Camera mainCam;
//        public Vector3 mousePos;

//        void Start()
//        {
//            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
//            //  Weapon= GameObject.FindGameObjectWithTag("Weapon").GetComponent<Game>();
//        }
//        void Update()
//        {
//            //mousePos = pointerPosition.action.ReadValue<Vector2>();
//            MousePoseFunction();
//        }

//        public void MousePoseFunction()
//        {
//            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

//            Vector2 rotation = mousePos - transform.position;

//            float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

//            transform.rotation = Quaternion.Euler(0, 0, rotz);
//        }

//    }


//}

namespace Draconiano_Android
{


    public class CArmShooter : MonoBehaviour
    {

        [SerializeField] protected float _Speed = 5;
        // Start is called before the first frame update
        [SerializeField] protected Rigidbody2D _Rigidbody2D;
        //  private float _Acceleration = 0;

        [SerializeField] protected Vector2 _Look = Vector2.zero;
        //private float _DashSpeed = 0
        //private bool _IsInmortal = False
        //private time6 _Delay = false
        private InputAction _LookAction;
        public object LookVector { get; private set; }
        public Vector3 lookDirection;
        private CInputSystemMultiplayer _defaultPlayerAction;
        protected Vector2 lookVector;
        protected Vector3 look;

      


        private GameObject _nearEnemy;
        //private Camera mainCam;
        //  public Vector3 mousePos;

        void Start()
        {
            //  mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            //  Weapon= GameObject.FindGameObjectWithTag("Weapon").GetComponent<Game>();

        }
        void Update()
        {
            //mousePos = pointerPosition.action.ReadValue<Vector2>();
            // MousePoseFunction();
            AutoAimFunction();
        }
        public void MousePoseFunction()
        {
            // mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);


            _Look = _LookAction.ReadValue<Vector2>();
            look = new Vector3(lookVector.x, 0.0f, lookVector.y);
            var lookDirection = new Vector3(look.x, 0.0f, look.y);
            lookDirection.Normalize();
            Vector2 rotation = lookDirection - transform.position;

            float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rotz);
        }


        public void AutoAimFunction()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject closestEnemy = null;
            float closestDistance = 20;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
            Debug.Log(closestEnemy.transform.position);
            if (closestEnemy != null)
            {
                //look = new Vector3(closestEnemy.transform.position.x, 0.0f, closestEnemy.transform.position.y);
                //var lookDirection = new Vector3(look.x, 0.0f, look.y);
                //lookDirection.Normalize();
                //Vector2 rotation = lookDirection - transform.position;
                //float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                //transform.rotation = Quaternion.Euler(0, 0, rotz);

                Vector2 rotation = closestEnemy.transform.position - transform.position;
                float rotz = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, rotz);
            }
        }

        //private void OnEnable()
        //{
        //    _LookAction = _defaultPlayerAction.Player.Look;
        //    _LookAction.Enable();


        //}

        //private void OnDisable()
        //{
        //    _LookAction.Enable();

        //}
    }
}


