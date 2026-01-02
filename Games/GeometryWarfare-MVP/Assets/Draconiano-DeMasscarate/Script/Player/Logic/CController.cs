using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace Draconiano_PC
{
    public class CController : MonoBehaviour
    {
        [SerializeField] protected float _Speed = 5;
        // Start is called before the first frame update
        [SerializeField] protected Rigidbody2D _Rigidbody2D;
        //  private float _Acceleration = 0;
        [SerializeField] protected Vector2 _Move = Vector2.zero;
        //private float _DashSpeed = 0
        //private bool _IsInmortal = False
        //private time6 _Delay = false


        void Start()
        {
            _Rigidbody2D = GetComponent<Rigidbody2D>();
            _Speed = 20;
        }
        private void Dash()
        {
            return;
        }

        private float TimeDelay()
        {
            return 0;
        }

        public void Update()
        {
            Move();
        }
        public virtual void Move()
        {

            _Move.x = Input.GetAxis("Horizontal");

            _Move.y = Input.GetAxis("Vertical");

            _Move = new Vector2(_Move.x, _Move.y);

            _Move.Normalize();
        }

        public void FixedUpdate()
        {
            _Rigidbody2D.MovePosition(_Rigidbody2D.position + _Move * _Speed * Time.deltaTime);

        }


    }


}

//namespace Draconiano_Android
//{
//    public class CController : MonoBehaviour, IInteract
//    {
//        [SerializeField] protected float _Speed;
//        // Start is called before the first frame update
//        [SerializeField] protected Rigidbody2D _Rigidbody2D;
//        //  private float _Acceleration = 0;

//        [SerializeField] protected Vector2 _Move = Vector2.zero;
//        //private float _DashSpeed = 0
//        //private bool _IsInmortal = False
//        //private time6 _Delay = false
//        private InputAction _MoveAction;
//        private InputAction _InteractAction;
//        private InputAction _ActivePowersAction;
//        private InputAction _DashAction;
//        public object MoveVector { get; private set; }
//        public Vector3 movementDirection;
//        private CInputSystemMultiplayer _defaultPlayerAction;
//        protected Vector2 moveVector;
//        protected Vector3 movement;

//        private void Awake()
//        {
//            _defaultPlayerAction = new CInputSystemMultiplayer();
//        }
//        void Start()
//        {
//            _Rigidbody2D = GetComponent<Rigidbody2D>();
//            //_Speed = 20;
//        }
//        private void Dash()
//        {
//            return;
//        }

//        private float TimeDelay()
//        {
//            return 0;
//        }

//        public void Update()
//        {
//            Move();
//        }
//        public virtual void Move()
//        {
//            _Move = _MoveAction.ReadValue<Vector2>();
//            movement = new Vector3(moveVector.x, 0.0f, moveVector.y);
//            var movementDirection = new Vector3(moveVector.x, 0.0f, moveVector.y);
//            movementDirection.Normalize();

//            //_Move.x = Input.GetAxis("Horizontal");

//            //_Move.y = Input.GetAxis("Vertical");

//            //_Move = new Vector2(_Move.x, _Move.y);

//            _Move.Normalize();
//        }

//        public void FixedUpdate()
//        {
//            _Rigidbody2D.MovePosition(_Rigidbody2D.position + _Move * (_Speed * Time.deltaTime));

//        }

//        private void OnEnable()
//        {
//            _MoveAction = _defaultPlayerAction.Player.Move;
//            _MoveAction.Enable();

//            //_DashAction = _defaultPlayerAction.Player.Dash;
//            //_DashAction.Enable();

//            _ActivePowersAction = _defaultPlayerAction.Player.AcivatePowers;
//            _ActivePowersAction.Enable();

//            _InteractAction = _defaultPlayerAction.Player.Interact;
//            _InteractAction.Enable();

//            //_defaultPlayerAction.Player.Dash.performed += OnDash;
//            _defaultPlayerAction.Player.Interact.performed += OnInteract;
//            _defaultPlayerAction.Player.AcivatePowers.performed += OnActivePowers;
//            //_defaultPlayerAction.Player.ChangeForm.performed += OnChangeForm;
//        }

//        private void OnDisable()
//        {
//            _MoveAction.Enable();
//            _DashAction.Enable();
//            _ActivePowersAction.Enable();
//            _InteractAction.Enable();
//        }

//        //public virtual void OnDash(InputAction.CallbackContext context)
//        //{
//        //    throw new NotImplementedException();
//        //}
//        public void OnActivePowers(InputAction.CallbackContext context)
//        {
//            throw new NotImplementedException();
//        }
//        public void OnInteract(InputAction.CallbackContext context)
//        {
//            throw new NotImplementedException();
//        }

//        public void OnInteract()
//        {

//        }
//    }


//}
