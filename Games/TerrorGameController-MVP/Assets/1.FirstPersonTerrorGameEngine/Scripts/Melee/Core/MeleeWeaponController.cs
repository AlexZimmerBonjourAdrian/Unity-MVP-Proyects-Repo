using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Controlador principal del sistema de armas cuerpo a cuerpo.
    /// Gestiona el input, estados y coordinación entre componentes.
    /// </summary>
    [RequireComponent(typeof(MeleeAttackBehavior))]
    [RequireComponent(typeof(MeleeBlockBehavior))]
    public class MeleeWeaponController : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private CHorrorController horrorController;

        [Header("Input")]
        [Tooltip("Si es true, el input se maneja automáticamente en Update()")]
        [SerializeField] private bool handleInputAutomatically = true;

        [Tooltip("Tecla para atacar (Mouse0 por defecto)")]
        [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;

        [Tooltip("Tecla para bloquear (Mouse1 por defecto)")]
        [SerializeField] private KeyCode blockKey = KeyCode.Mouse1;

        [Header("Estado")]
        [SerializeField] private MeleeWeapon currentWeapon;

        private MeleeAttackBehavior attackBehavior;
        private MeleeBlockBehavior blockBehavior;
        private bool isBlockingInput = false;

        private void Awake()
        {
            // Obtener referencias
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
                if (playerCamera == null)
                {
                    playerCamera = FindObjectOfType<Camera>();
                }
            }

            if (horrorController == null)
            {
                horrorController = GetComponent<CHorrorController>();
                if (horrorController == null)
                {
                    horrorController = FindObjectOfType<CHorrorController>();
                }
            }

            // Obtener componentes de comportamiento
            attackBehavior = GetComponent<MeleeAttackBehavior>();
            if (attackBehavior == null)
            {
                attackBehavior = gameObject.AddComponent<MeleeAttackBehavior>();
            }

            blockBehavior = GetComponent<MeleeBlockBehavior>();
            if (blockBehavior == null)
            {
                blockBehavior = gameObject.AddComponent<MeleeBlockBehavior>();
            }

            // Configurar cámaras en los behaviors
            if (attackBehavior != null && playerCamera != null)
            {
                attackBehavior.SetCamera(playerCamera);
            }

            if (blockBehavior != null && playerCamera != null)
            {
                blockBehavior.SetCamera(playerCamera);
            }
        }

        private void Update()
        {
            if (currentWeapon == null) return;

            // Manejar input automáticamente si está habilitado
            if (handleInputAutomatically)
            {
                HandleInput();
            }

            // Actualizar velocidad de movimiento según bloqueo
            UpdateMovementSpeed();
        }

        /// <summary>
        /// Maneja el input del jugador
        /// </summary>
        private void HandleInput()
        {
            // Ataque
            if (Input.GetKeyDown(attackKey))
            {
                Attack();
            }

            // Bloqueo (hold)
            bool blockInput = Input.GetKey(blockKey);
            if (blockInput && !isBlockingInput)
            {
                StartBlock();
            }
            else if (!blockInput && isBlockingInput)
            {
                StopBlock();
            }
            isBlockingInput = blockInput;
        }

        /// <summary>
        /// Equipa un arma melee
        /// </summary>
        public void EquipWeapon(MeleeWeaponDataSO weaponData)
        {
            if (weaponData == null)
            {
                Debug.LogWarning("MeleeWeaponController: Intento de equipar arma con datos null");
                return;
            }

            // Si ya hay un arma equipada, desequiparla
            if (currentWeapon != null)
            {
                UnequipWeapon();
            }

            // Crear nuevo arma
            GameObject weaponObject = new GameObject(weaponData.weaponName);
            weaponObject.transform.SetParent(transform);
            weaponObject.transform.localPosition = Vector3.zero;
            weaponObject.transform.localRotation = Quaternion.identity;

            currentWeapon = weaponObject.AddComponent<MeleeWeapon>();
            currentWeapon.Initialize(weaponData);

            // Disparar evento
            CGameEvents.OnMeleeWeaponEquipped.Publish(weaponData.weaponName);

            Debug.Log($"Arma melee equipada: {weaponData.weaponName}");
        }

        /// <summary>
        /// Desequipa el arma actual
        /// </summary>
        public void UnequipWeapon()
        {
            if (currentWeapon != null)
            {
                StopBlock(); // Asegurar que se detenga el bloqueo

                string weaponName = currentWeapon.WeaponName;
                Destroy(currentWeapon.gameObject);
                currentWeapon = null;

                Debug.Log($"Arma melee desequipada: {weaponName}");
            }
        }

        /// <summary>
        /// Realiza un ataque
        /// </summary>
        public void Attack()
        {
            if (currentWeapon == null || attackBehavior == null)
            {
                return;
            }

            if (!currentWeapon.CanAttack())
            {
                return;
            }

            // Iniciar ataque en el arma
            currentWeapon.StartAttack();

            // Realizar el ataque
            MeleeHitInfo hitInfo = attackBehavior.PerformAttack(currentWeapon);

            // Finalizar ataque
            currentWeapon.EndAttack();

            // Disparar eventos
            CGameEvents.OnMeleeAttack.Publish(currentWeapon.WeaponName);

            if (hitInfo.hit && hitInfo.damageable != null)
            {
                var hitData = new MeleeHitEventData(currentWeapon.WeaponName, hitInfo.hitCollider.name, currentWeapon.Damage);
                CGameEvents.OnMeleeHit.Publish(hitData);
            }
        }

        /// <summary>
        /// Inicia el bloqueo
        /// </summary>
        public void StartBlock()
        {
            if (currentWeapon == null || blockBehavior == null)
            {
                return;
            }

            if (!currentWeapon.CanBlock())
            {
                return;
            }

            if (blockBehavior.StartBlock(currentWeapon))
            {
                currentWeapon.StartBlock();
                CGameEvents.OnMeleeBlockStart.Publish(currentWeapon.WeaponName);
            }
        }

        /// <summary>
        /// Detiene el bloqueo
        /// </summary>
        public void StopBlock()
        {
            if (currentWeapon == null || blockBehavior == null)
            {
                return;
            }

            if (currentWeapon.IsBlocking)
            {
                currentWeapon.StopBlock();
                blockBehavior.StopBlock();
                CGameEvents.OnMeleeBlockEnd.Publish(currentWeapon.WeaponName);
            }
        }

        /// <summary>
        /// Actualiza la velocidad de movimiento según el estado de bloqueo
        /// </summary>
        private void UpdateMovementSpeed()
        {
            if (horrorController == null || blockBehavior == null)
            {
                return;
            }

            // El bloqueo reduce la velocidad de movimiento
            // Esto se puede implementar modificando moveSpeed en CHorrorController
            // Por ahora, solo notificamos que se está bloqueando
            // La implementación completa requeriría modificar CHorrorController
        }

        /// <summary>
        /// Obtiene el arma actual
        /// </summary>
        public MeleeWeapon GetCurrentWeapon()
        {
            return currentWeapon;
        }

        /// <summary>
        /// Verifica si hay un arma equipada
        /// </summary>
        public bool HasWeapon()
        {
            return currentWeapon != null;
        }

        /// <summary>
        /// Obtiene el multiplicador de velocidad de movimiento (para integración con CHorrorController)
        /// </summary>
        public float GetMovementSpeedMultiplier()
        {
            if (blockBehavior != null)
            {
                return blockBehavior.GetMovementSpeedMultiplier();
            }
            return 1f;
        }
    }
}
