using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayer : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float _speed = 5f;

     [SerializeField] private float _speed_bledding = 3f;
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private float _gravity = 9.81f;


     [SerializeField] private float targetBlend = 2f;
    private Animator _animator;


  // public enum PlayerState
  //   {
  //       Grounded = 0,
  //       Jumping = 1,
  //       Falling = 2,
  //       // ... otros estados que necesites
  //   }

    enum animValues
    {
      Jump = 2,
      Running = 0,
      Slide = -1,
      Fall = 5,
      Death = 0,

    };
    [SerializeField] private float _fallMultiplier = 2.5f; // Multiplicador para caída más rápida


    private Vector3 _moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {   
        _controller = GetComponent<CharacterController>();

        _animator = GetComponent<Animator>();
        targetBlend = (float)animValues.Jump;
      
    }

    // Update is called once per frame
    void Update()
    {
         // Calcula el valor objetivo del blend
        float currentBlend = _animator.GetFloat("Blend"); // Obtiene el valor actual del blend

        // Interpola suavemente entre el valor actual y el objetivo
        float newBlend = Mathf.Lerp(currentBlend, targetBlend, _speed_bledding * Time.deltaTime);

        _animator.SetFloat("Blend", newBlend); // Aplica el nuevo valor al blend

        Move();

        if (_controller.isGrounded && Input.GetButtonDown("Jump"))
        {   
            _moveDirection.y = _jumpForce; 
            targetBlend = (float)animValues.Jump;
  
        }
    }

      private void Move()
    {
        // Movimiento horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 moveHorizontal = (Vector3.right * horizontalInput).normalized;
        _controller.Move(moveHorizontal * _speed * Time.deltaTime);
        
        // Salto
       

        // Aplicar gravedad con aceleración gradual
        if (!_controller.isGrounded) 
        {
            _moveDirection.y -= _gravity * (_moveDirection.y < 0 ? _fallMultiplier : 1) * Time.deltaTime; 
              // if(targetBlend >  (float)animValues.Jump)
              //    targetBlend = (float)animValues.Running;
        }

             
        


        _controller.Move(_moveDirection * Time.deltaTime);
    }

    //Detectar colisión con el suelo
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("isGrounded"))
        {
            _moveDirection.y = 0f; 
            targetBlend = (float)animValues.Running;
              // Reiniciar la velocidad vertical al tocar el suelo
        }
    }
}