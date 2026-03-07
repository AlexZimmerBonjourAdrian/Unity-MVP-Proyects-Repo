using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Comportamiento de bloqueo para armas cuerpo a cuerpo.
    /// Maneja la reducción de daño y efectos visuales del bloqueo.
    /// </summary>
    public class MeleeBlockBehavior : MonoBehaviour
    {
        [Header("Configuración de Bloqueo")]
        [Tooltip("Multiplicador de velocidad de movimiento mientras bloquea (0-1)")]
        [Range(0f, 1f)]
        [SerializeField] private float movementSpeedMultiplier = 0.6f;

        [Tooltip("Si es true, el bloqueo consume stamina")]
        [SerializeField] private bool useStamina = false;

        [Tooltip("Stamina requerida para bloquear (si useStamina es true)")]
        [SerializeField] private float requiredStamina = 10f;

        [Header("Efectos Visuales")]
        [Tooltip("Cambiar FOV mientras bloquea")]
        [SerializeField] private bool changeFOVOnBlock = false;

        [Tooltip("FOV mientras bloquea (si changeFOVOnBlock es true)")]
        [SerializeField] private float blockFOV = 50f;

        private float originalFOV = 60f;
        private Camera playerCamera;
        private bool isBlocking = false;
        private MeleeWeapon currentWeapon;

        private void Awake()
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                playerCamera = FindObjectOfType<Camera>();
            }

            if (playerCamera != null)
            {
                originalFOV = playerCamera.fieldOfView;
            }
        }

        /// <summary>
        /// Inicia el bloqueo
        /// </summary>
        /// <param name="weapon">Arma que está bloqueando</param>
        /// <returns>True si el bloqueo se inició correctamente</returns>
        public bool StartBlock(MeleeWeapon weapon)
        {
            if (weapon == null || !weapon.CanBlock())
            {
                return false;
            }

            // Verificar stamina si está habilitado
            if (useStamina && weapon.WeaponData != null)
            {
                float staminaCost = weapon.WeaponData.blockStaminaCost;
                if (staminaCost > 0 && Player.Instance != null)
                {
                    // Verificar si hay suficiente stamina (si se implementa en Player)
                    // Por ahora, asumimos que siempre hay stamina suficiente
                }
            }

            isBlocking = true;
            currentWeapon = weapon;

            // Aplicar efectos visuales
            if (changeFOVOnBlock && playerCamera != null)
            {
                playerCamera.fieldOfView = blockFOV;
            }

            // Reproducir sonido de bloqueo si está configurado
            if (weapon.WeaponData != null && weapon.WeaponData.blockSound != null)
            {
                weapon.PlaySound(weapon.WeaponData.blockSound);
            }

            return true;
        }

        /// <summary>
        /// Detiene el bloqueo
        /// </summary>
        public void StopBlock()
        {
            if (!isBlocking) return;

            isBlocking = false;

            // Restaurar efectos visuales
            if (changeFOVOnBlock && playerCamera != null)
            {
                playerCamera.fieldOfView = originalFOV;
            }

            currentWeapon = null;
        }

        /// <summary>
        /// Procesa el daño recibido mientras se bloquea
        /// </summary>
        /// <param name="incomingDamage">Daño entrante</param>
        /// <returns>Daño final después de aplicar reducción de bloqueo</returns>
        public int ProcessBlockedDamage(int incomingDamage)
        {
            if (!isBlocking || currentWeapon == null || currentWeapon.WeaponData == null)
            {
                return incomingDamage;
            }

            float reduction = currentWeapon.BlockDamageReduction;
            int finalDamage = Mathf.RoundToInt(incomingDamage * (1f - reduction));

            // Reproducir sonido de bloqueo exitoso
            if (currentWeapon.WeaponData.blockSound != null)
            {
                currentWeapon.PlaySound(currentWeapon.WeaponData.blockSound);
            }

            return finalDamage;
        }

        /// <summary>
        /// Obtiene el multiplicador de velocidad de movimiento mientras bloquea
        /// </summary>
        public float GetMovementSpeedMultiplier()
        {
            return isBlocking ? movementSpeedMultiplier : 1f;
        }

        /// <summary>
        /// Verifica si actualmente se está bloqueando
        /// </summary>
        public bool IsBlocking()
        {
            return isBlocking;
        }

        /// <summary>
        /// Verifica si hay suficiente stamina para bloquear
        /// </summary>
        public bool HasStaminaToBlock(MeleeWeapon weapon)
        {
            if (!useStamina || weapon == null || weapon.WeaponData == null)
            {
                return true;
            }

            float staminaCost = weapon.WeaponData.blockStaminaCost;
            if (staminaCost <= 0)
            {
                return true;
            }

            // Por ahora, siempre retornamos true
            // Esto se puede integrar con un sistema de stamina en Player
            return true;
        }

        /// <summary>
        /// Establece la cámara del jugador
        /// </summary>
        public void SetCamera(Camera camera)
        {
            playerCamera = camera;
            if (playerCamera != null)
            {
                originalFOV = playerCamera.fieldOfView;
            }
        }
    }
}
