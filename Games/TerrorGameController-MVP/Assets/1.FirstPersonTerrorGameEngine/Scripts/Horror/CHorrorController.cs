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

    // Componentes modulares
    private HorrorHeadBobComponent headBobComponent;
    private HorrorZoomComponent zoomComponent;
    private HorrorCrouchComponent crouchComponent;
    private HorrorCameraFOVController fovController;
    private HorrorCrosshairComponent crosshairComponent;
    private HorrorCameraInversionComponent cameraInversionComponent;

    private bool isWalking = false;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _verticalRotation = transform.localEulerAngles.y;
        mainCamera = Camera.main;

        // Obtener o crear componentes modulares
        headBobComponent = GetComponent<HorrorHeadBobComponent>();
        if (headBobComponent == null)
        {
            headBobComponent = gameObject.AddComponent<HorrorHeadBobComponent>();
        }

        zoomComponent = GetComponent<HorrorZoomComponent>();
        if (zoomComponent == null)
        {
            zoomComponent = gameObject.AddComponent<HorrorZoomComponent>();
        }

        crouchComponent = GetComponent<HorrorCrouchComponent>();
        if (crouchComponent == null)
        {
            crouchComponent = gameObject.AddComponent<HorrorCrouchComponent>();
        }

        fovController = GetComponent<HorrorCameraFOVController>();
        if (fovController == null)
        {
            fovController = gameObject.AddComponent<HorrorCameraFOVController>();
        }

        crosshairComponent = GetComponent<HorrorCrosshairComponent>();
        if (crosshairComponent == null)
        {
            crosshairComponent = gameObject.AddComponent<HorrorCrosshairComponent>();
        }

        cameraInversionComponent = GetComponent<HorrorCameraInversionComponent>();
        if (cameraInversionComponent == null)
        {
            cameraInversionComponent = gameObject.AddComponent<HorrorCameraInversionComponent>();
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
        Cursor.visible = false; // Hacer el cursor invisible

        // Inicializar componentes modulares
        if (crosshairComponent != null)
        {
            crosshairComponent.Show();
        }
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

        // Aplicar reducción de velocidad si está agachado
        float crouchSpeedMultiplier = 1f;
        if (crouchComponent != null && crouchComponent.IsCrouched())
        {
            crouchSpeedMultiplier = crouchComponent.GetSpeedReduction();
        }

        float currentSpeed = isSprinting ? moveSpeed * sprintSpeedMultiplier : moveSpeed;
        currentSpeed *= crouchSpeedMultiplier;

        _velocity.x = _moveDirection.x * currentSpeed;
        _velocity.z = _moveDirection.z * currentSpeed;

        // Detectar si está caminando para headbob
        isWalking = (_velocity.x != 0 || _velocity.z != 0) && _controller.isGrounded;

        // Actualizar componentes modulares
        if (headBobComponent != null)
        {
            headBobComponent.SetWalking(isWalking);
            headBobComponent.SetSprinting(isSprinting);
            if (crouchComponent != null)
            {
                headBobComponent.SetCrouched(crouchComponent.IsCrouched());
            }
        }

        if (zoomComponent != null)
        {
            zoomComponent.SetSprinting(isSprinting);
        }

        if (fovController != null)
        {
            fovController.SetSprinting(isSprinting);
            if (zoomComponent != null)
            {
                fovController.SetZoomed(zoomComponent.IsZoomed());
            }
        }
        
        // Mirar alrededor
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _verticalRotation += mouseX;
        
        // Aplicar inversión de cámara si está configurada
        if (cameraInversionComponent != null)
        {
            _horizontalRotation += cameraInversionComponent.ApplyInversion(mouseY);
        }
        else
        {
            _horizontalRotation += mouseY; // Comportamiento por defecto (ya invertido)
        }

        _horizontalRotation = Mathf.Clamp(_horizontalRotation, -clampAngle, clampAngle);

        // Rotar el objeto padre en Y
        transform.parent.localEulerAngles = new Vector3(0f, _verticalRotation, 0f);
    
        _controller.Move(_velocity * Time.deltaTime);
        CameraTransform.transform.localEulerAngles = new Vector3(_horizontalRotation, 0f, 0f); 
    }

    public Transform getDirectionTransform()
    {
        return direction_Transform;
    }
    }
}
