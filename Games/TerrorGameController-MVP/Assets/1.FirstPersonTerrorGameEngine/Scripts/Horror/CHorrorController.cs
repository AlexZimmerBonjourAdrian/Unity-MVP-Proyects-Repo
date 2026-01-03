using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HorrorEngine
{
    [RequireComponent(typeof(CInteractRayCast))]
    [RequireComponent(typeof(Player))]
    public class CHorrorController : MonoBehaviour
{
    // Movement variables
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    // Look variables
    public float mouseSensitivity = 2f;
    public float clampAngle = 80f;

    // Sprint variables
    public float sprintSpeedMultiplier = 2f;
    public float sprintDuration = 3f;
    public float sprintCooldown = 5f;

    private float sprintTimer = 0f;
    private float sprintCooldownTimer = 0f;
    private bool isSprinting = false;

    // Private variables
    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Vector3 _velocity;
    private float _verticalRotation;
    private float _horizontalRotation;

    [SerializeField]
    private Transform direction_Transform;

    public float interactionDistance = 3f; // Distancia de interacción
    public Color gizmoColor = Color.yellow; // Color del Gizmo
 
    [SerializeField]
    private Transform CameraTransform;

    private Camera mainCamera;
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _verticalRotation = transform.localEulerAngles.y;
         mainCamera = Camera.main;
      
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
        Cursor.visible = false; // Hacer el cursor invisible
    }

    private void Update()
    {
        // Movimiento siempre activo
        float xHorizontal = Input.GetAxis("Horizontal");
        float zVertical = Input.GetAxis("Vertical");

        _controller.transform.position = CameraTransform.transform.position;
        _moveDirection = direction_Transform.forward * zVertical; 
        _moveDirection += direction_Transform.right * xHorizontal; 

        if (_controller.isGrounded)
        {
            _velocity.y = 0f; // Reiniciar la velocidad vertical si está en el suelo
            if (Input.GetButtonDown("Jump"))
            {
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * -gravity);   
            }   
        }

        // Aplicar gravedad siempre
        _velocity.y -= gravity * Time.deltaTime;

        // Sprint logic
        if (Input.GetKey(KeyCode.LeftShift) && sprintCooldownTimer <= 0f && sprintTimer < sprintDuration)
        {
            isSprinting = true;
            sprintTimer += Time.deltaTime;
        }
        else
        {
            isSprinting = false;
            if (sprintTimer > 0f)
            {
                sprintCooldownTimer = sprintCooldown;
            }
            sprintTimer = 0f;
        }

        if (sprintCooldownTimer > 0f)
        {
            sprintCooldownTimer -= Time.deltaTime;
        }

        float currentSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;

        _velocity.x = _moveDirection.x * currentSpeed;
        _velocity.z = _moveDirection.z * currentSpeed;
        
        // Mirar alrededor
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        //Invertimos el eje Y
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _verticalRotation += mouseX;
        _horizontalRotation += mouseY; // Cambio aquí: se suma en lugar de restar
        //transform.localEulerAngles = new Vector3(_horizontalRotation, _verticalRotation, 0f);

        _horizontalRotation = Mathf.Clamp(_horizontalRotation, -clampAngle, clampAngle);

        // Rotar el objeto padre en Y
        transform.parent.localEulerAngles = new Vector3(0f, _verticalRotation, 0f);
    
        _controller.Move(_velocity * Time.deltaTime);
        CameraTransform.transform.localEulerAngles = new Vector3(_horizontalRotation, _verticalRotation, 0f);
    
        // Rotar el objeto padre en Y
        CameraTransform.transform.localEulerAngles = new Vector3(_horizontalRotation, 0f, 0f); 
    }

    public Transform getDirectionTransform()
    {
        return direction_Transform;
    }
    }
}
