using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Componente modular para sistema de agacharse en controladores de primera persona.
    /// Adaptado para CharacterController (modifica height y center en lugar de scale).
    /// Versión independiente para HorrorEngine.
    /// </summary>
    public class HorrorCrouchComponent : MonoBehaviour
    {
        [Header("Crouch Settings")]
        [SerializeField] private CharacterController controller;
        [SerializeField] private bool enableCrouch = true;
        [SerializeField] private bool holdToCrouch = true;
        [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
        [SerializeField] private float crouchHeight = 0.75f;
        [SerializeField] private float speedReduction = 0.5f;

        private bool isCrouched = false;
        private float originalHeight;
        private Vector3 originalCenter;

        private void Awake()
        {
            if (controller == null)
            {
                controller = GetComponent<CharacterController>();
            }

            if (controller == null)
            {
                Debug.LogError("HorrorCrouchComponent: No se encontró CharacterController. Este componente requiere CharacterController.");
                enabled = false;
                return;
            }

            // Guardar valores originales
            originalHeight = controller.height;
            originalCenter = controller.center;
        }

        private void Update()
        {
            if (!enableCrouch || controller == null) return;

            // Comportamiento para toggle crouch
            if (Input.GetKeyDown(crouchKey) && !holdToCrouch)
            {
                if (isCrouched)
                {
                    StandUp();
                }
                else
                {
                    Crouch();
                }
            }

            // Comportamiento para hold to crouch
            if (holdToCrouch)
            {
                if (Input.GetKeyDown(crouchKey))
                {
                    Crouch();
                }
                else if (Input.GetKeyUp(crouchKey))
                {
                    StandUp();
                }
            }
        }

        /// <summary>
        /// Agacha al jugador
        /// </summary>
        public void Crouch()
        {
            if (isCrouched || controller == null) return;

            // Reducir altura del CharacterController
            controller.height = originalHeight * crouchHeight;
            controller.center = new Vector3(
                originalCenter.x,
                (originalHeight * crouchHeight) / 2f,
                originalCenter.z
            );

            isCrouched = true;
        }

        /// <summary>
        /// Levanta al jugador
        /// </summary>
        public void StandUp()
        {
            if (!isCrouched || controller == null) return;

            // Restaurar altura original del CharacterController
            controller.height = originalHeight;
            controller.center = originalCenter;

            isCrouched = false;
        }

        /// <summary>
        /// Verifica si el jugador está agachado
        /// </summary>
        public bool IsCrouched()
        {
            return isCrouched;
        }

        /// <summary>
        /// Obtiene el multiplicador de reducción de velocidad
        /// </summary>
        public float GetSpeedReduction()
        {
            return isCrouched ? speedReduction : 1f;
        }

        /// <summary>
        /// Fuerza el estado de agacharse (útil para mecánicas externas)
        /// </summary>
        public void ForceCrouch(bool crouch)
        {
            if (crouch && !isCrouched)
            {
                Crouch();
            }
            else if (!crouch && isCrouched)
            {
                StandUp();
            }
        }
    }
}
