using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace RetroFPS
{
    public class CPlayer3DController : MonoBehaviour
    {
        // Movement variables
        public float moveSpeed = 5f;
        public float jumpHeight = 2f;
        public float gravity = -9.81f;

        // Look variables
        public float mouseSensitivity = 2f;
        public float clampAngle = 80f;

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

        // Componentes modulares (opcionales)
        private Component headBobComponent;
        private HeadBob headBob;
        private Component zoomComponent;
        private Component crouchComponent;
        private Component fovController;
        private Component crosshairComponent;
        private Component cameraInversionComponent;

        private bool isWalking = false;
        private bool isSprinting = false;
        private CManagerWeapon weaponManager;
        private FPSConfigManager fpsConfigManager;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _verticalRotation = transform.localEulerAngles.y;
            mainCamera = Camera.main;

            // Obtener componentes modulares (opcionales - solo obtener si existen)
            headBobComponent = GetComponentByName("HeadBobComponent");
            zoomComponent = GetComponentByName("ZoomComponent");
            crouchComponent = GetComponentByName("CrouchComponent");
            fovController = GetComponentByName("CameraFOVController");
            crosshairComponent = GetComponentByName("CrosshairComponent");
            cameraInversionComponent = GetComponentByName("CameraInversionComponent");

            headBob = CameraTransform.GetComponent<HeadBob>();
            if (headBob == null)
            {
                headBob = CameraTransform.gameObject.AddComponent<HeadBob>();
            }

            weaponManager = FindObjectOfType<CManagerWeapon>();
            fpsConfigManager = FindObjectOfType<FPSConfigManager>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Inicializar componentes modulares
            if (crosshairComponent != null)
            {
                InvokeMethod(crosshairComponent, "Show");
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
                KeyCode jumpKey = GetJumpKey();
                if (Input.GetKeyDown(jumpKey) || (jumpKey == KeyCode.Space && Input.GetButtonDown("Jump")))
                {
                    _velocity.y = Mathf.Sqrt(jumpHeight * -2f * -gravity);   
                }   
            }

            // Aplicar gravedad siempre
            _velocity.y -= gravity * Time.deltaTime;

            // Movimiento solo si no está en modo puzzle
            bool puzzleMode = false;
            if (CGameManager.Inst != null)
            {
                puzzleMode = CGameManager.Inst.GetPuzzleMode();
            }

            if (!puzzleMode)
            {
                // Aplicar reducción de velocidad si está agachado
                float crouchSpeedMultiplier = 1f;
                if (crouchComponent != null && InvokeMethod<bool>(crouchComponent, "IsCrouched"))
                {
                    crouchSpeedMultiplier = InvokeMethod<float>(crouchComponent, "GetSpeedReduction");
                }

                // Detectar sprint
                KeyCode sprintKey = GetSprintKey();
                float sprintMultiplier = GetSprintMultiplier();
                bool sprintInput = Input.GetKey(sprintKey);
                float speedMultiplier = sprintInput ? sprintMultiplier : 1f;
                isSprinting = sprintInput && (_velocity.x != 0 || _velocity.z != 0) && _controller.isGrounded;

                _velocity.x = _moveDirection.x * moveSpeed * crouchSpeedMultiplier * speedMultiplier;
                _velocity.z = _moveDirection.z * moveSpeed * crouchSpeedMultiplier * speedMultiplier;

                // Detectar si está caminando para headbob
                isWalking = (_velocity.x != 0 || _velocity.z != 0) && _controller.isGrounded;

                // Actualizar componentes modulares
                if (headBobComponent != null)
                {
                    InvokeMethod(headBobComponent, "SetWalking", isWalking);
                    if (crouchComponent != null)
                    {
                        InvokeMethod(headBobComponent, "SetCrouched", InvokeMethod<bool>(crouchComponent, "IsCrouched"));
                    }
                }

                if (headBob != null)
                {
                    headBob.SetWalking(isWalking);
                    headBob.SetSprinting(isSprinting);
                    if (crouchComponent != null)
                    {
                        headBob.SetCrouched(InvokeMethod<bool>(crouchComponent, "IsCrouched"));
                    }
                    if (zoomComponent != null)
                    {
                        headBob.SetAiming(InvokeMethod<bool>(zoomComponent, "IsZoomed"));
                    }
                }

                UpdateWeaponAnimations();

                if (zoomComponent != null)
                {
                    // El zoom se maneja internamente, solo necesitamos verificar si está activo
                }

                if (fovController != null)
                {
                    if (zoomComponent != null)
                    {
                        InvokeMethod(fovController, "SetZoomed", InvokeMethod<bool>(zoomComponent, "IsZoomed"));
                    }
                }
        
                // Mirar alrededor solo si no está en modo puzzle
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

                _verticalRotation += mouseX;
                
                // Aplicar inversión de cámara si está configurada
                if (cameraInversionComponent != null)
                {
                    float inversion = InvokeMethod<float>(cameraInversionComponent, "ApplyInversion", mouseY);
                    _horizontalRotation += inversion;
                }
                else
                {
                    _horizontalRotation -= mouseY; // Comportamiento original (invertido)
                }

                _horizontalRotation = Mathf.Clamp(_horizontalRotation, -clampAngle, clampAngle);

                // Rotar el objeto padre en Y
                transform.parent.localEulerAngles = new Vector3(0f, _verticalRotation, 0f);
            }
            else 
            {
                _velocity.x = 0f; // Detener movimiento horizontal en modo puzzle
                _velocity.z = 0f; // Detener movimiento vertical en modo puzzle
                isWalking = false;

                // Actualizar componentes modulares
                if (headBobComponent != null)
                {
                    InvokeMethod(headBobComponent, "SetWalking", false);
                }

                if (headBob != null)
                {
                    headBob.SetWalking(false);
                    headBob.SetSprinting(false);
                }

                UpdateWeaponAnimations();
            }

            _controller.Move(_velocity * Time.deltaTime);
            CameraTransform.transform.localEulerAngles = new Vector3(_horizontalRotation, 0f, 0f); 

            // Interacción (mantener lógica original)
            KeyCode interactKey = GetInteractKey();
            if (Input.GetKeyDown(interactKey))
            {
                RaycastHit hit;
                // Usar la dirección de la cámara para el Raycast
                if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactionDistance))
                {
                    // Buscar el componente Iinteract en el objeto golpeado
                    Iinteract interactable = hit.collider.GetComponent<Iinteract>();
                    if (interactable != null)
                    {
                        interactable.Oninteract(); // Ejecutar Oninteract()
                    }
                }
            }
        }

        public Transform getDirectionTransform()
        {
            return direction_Transform;
        }

        public void ApplyConfig(FPSConfig config)
        {
            if (config == null) return;

            moveSpeed = config.moveSpeed;
            jumpHeight = config.jumpHeight;
            gravity = config.gravity;
            mouseSensitivity = config.mouseSensitivity;
            clampAngle = config.clampAngle;

            if (mainCamera != null)
            {
                mainCamera.fieldOfView = config.baseFOV;
            }
        }

        private void UpdateWeaponAnimations()
        {
            if (weaponManager == null) return;

            GameObject currentWeapon = weaponManager.GetCurrentWeapon();
            if (currentWeapon == null) return;

            WeaponAnimationController animController = currentWeapon.GetComponent<WeaponAnimationController>();
            if (animController == null) return;

            animController.SetWalking(isWalking);
            animController.SetSprinting(isSprinting);

            if (zoomComponent != null)
            {
                animController.SetAiming(InvokeMethod<bool>(zoomComponent, "IsZoomed"));
            }
        }

        private KeyCode GetSprintKey()
        {
            if (fpsConfigManager != null && fpsConfigManager.GetConfiguration() != null)
            {
                return fpsConfigManager.GetConfiguration().sprintKey;
            }
            return KeyCode.LeftShift;
        }

        private float GetSprintMultiplier()
        {
            if (fpsConfigManager != null && fpsConfigManager.GetConfiguration() != null)
            {
                return fpsConfigManager.GetConfiguration().sprintSpeedMultiplier;
            }
            return 1.5f;
        }

        private KeyCode GetInteractKey()
        {
            if (fpsConfigManager != null && fpsConfigManager.GetConfiguration() != null)
            {
                return fpsConfigManager.GetConfiguration().interactKey;
            }
            return KeyCode.E;
        }

        private KeyCode GetJumpKey()
        {
            if (fpsConfigManager != null && fpsConfigManager.GetConfiguration() != null)
            {
                return fpsConfigManager.GetConfiguration().jumpKey;
            }
            return KeyCode.Space;
        }

        private Component GetComponentByName(string componentName)
        {
            Component[] components = GetComponents<Component>();
            foreach (Component comp in components)
            {
                if (comp != null && comp.GetType().Name == componentName)
                {
                    return comp;
                }
            }
            return null;
        }

        private void InvokeMethod(Component component, string methodName, params object[] parameters)
        {
            if (component == null) return;
            MethodInfo method = component.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(component, parameters);
            }
        }

        private T InvokeMethod<T>(Component component, string methodName, params object[] parameters)
        {
            if (component == null) return default(T);
            MethodInfo method = component.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                object result = method.Invoke(component, parameters);
                if (result is T)
                {
                    return (T)result;
                }
            }
            return default(T);
        }
    }
}
