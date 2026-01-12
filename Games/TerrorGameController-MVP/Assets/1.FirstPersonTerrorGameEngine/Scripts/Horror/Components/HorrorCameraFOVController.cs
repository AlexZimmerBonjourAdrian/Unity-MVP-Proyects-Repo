using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Componente modular para control dinámico de FOV en controladores de primera persona.
    /// Maneja cambios de FOV durante sprint, compatible con zoom.
    /// Prioridad: Zoom > Sprint > Normal
    /// Versión independiente para HorrorEngine.
    /// </summary>
    public class HorrorCameraFOVController : MonoBehaviour
    {
        [Header("FOV Settings")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float normalFOV = 60f;
        [SerializeField] private float sprintFOV = 80f;
        [SerializeField] private float sprintFOVStepTime = 10f;

        private bool isSprinting = false;
        private bool isZoomed = false;

        private void Awake()
        {
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
                if (playerCamera == null)
                {
                    playerCamera = GetComponentInChildren<Camera>();
                }
            }

            if (playerCamera != null)
            {
                normalFOV = playerCamera.fieldOfView;
            }
            else
            {
                Debug.LogError("HorrorCameraFOVController: No se encontró Camera. Asigna una cámara en el Inspector.");
            }
        }

        private void Update()
        {
            if (playerCamera == null) return;

            // Solo aplicar FOV de sprint si no está en zoom
            // Prioridad: Zoom > Sprint > Normal
            if (isSprinting && !isZoomed)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, sprintFOV, sprintFOVStepTime * Time.deltaTime);
            }
            else if (!isZoomed)
            {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, normalFOV, sprintFOVStepTime * Time.deltaTime);
            }
        }

        /// <summary>
        /// Establece si el jugador está corriendo
        /// </summary>
        public void SetSprinting(bool sprinting)
        {
            isSprinting = sprinting;
        }

        /// <summary>
        /// Establece si el zoom está activo (para prioridad)
        /// </summary>
        public void SetZoomed(bool zoomed)
        {
            isZoomed = zoomed;
        }

        /// <summary>
        /// Establece el FOV normal
        /// </summary>
        public void SetNormalFOV(float fov)
        {
            normalFOV = fov;
            if (!isSprinting && !isZoomed && playerCamera != null)
            {
                playerCamera.fieldOfView = normalFOV;
            }
        }

        /// <summary>
        /// Obtiene el FOV normal actual
        /// </summary>
        public float GetNormalFOV()
        {
            return normalFOV;
        }

        /// <summary>
        /// Fuerza el FOV a normal inmediatamente (sin lerp)
        /// </summary>
        public void ForceNormalFOV()
        {
            if (playerCamera != null)
            {
                playerCamera.fieldOfView = normalFOV;
            }
        }
    }
}
