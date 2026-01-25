using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Estado de un arma cuerpo a cuerpo
    /// </summary>
    public enum MeleeWeaponState
    {
        Idle,       // En reposo
        Attacking,  // Atacando
        Blocking,   // Bloqueando
        Cooldown    // En cooldown después de ataque
    }

    /// <summary>
    /// Clase base para armas cuerpo a cuerpo.
    /// Maneja el estado y las propiedades básicas del arma.
    /// </summary>
    public class MeleeWeapon : MonoBehaviour
    {
        [Header("Datos del Arma")]
        [SerializeField] private MeleeWeaponDataSO weaponData;

        [Header("Estado")]
        [SerializeField] private MeleeWeaponState currentState = MeleeWeaponState.Idle;

        [Header("Referencias")]
        [SerializeField] private GameObject weaponModelInstance;

        private float lastAttackTime = 0f;
        private bool isBlocking = false;

        // Propiedades públicas
        public MeleeWeaponDataSO WeaponData => weaponData;
        public MeleeWeaponState CurrentState => currentState;
        public bool IsBlocking => isBlocking;
        public bool IsAttacking => currentState == MeleeWeaponState.Attacking;
        public bool IsInCooldown => currentState == MeleeWeaponState.Cooldown;

        // Propiedades derivadas de WeaponData
        public int Damage => weaponData != null ? weaponData.damage : 0;
        public float Range => weaponData != null ? weaponData.range : 0f;
        public float AttackSpeed => weaponData != null ? weaponData.attackSpeed : 1f;
        public float BlockDamageReduction => weaponData != null ? weaponData.blockDamageReduction : 0f;
        public string WeaponName => weaponData != null ? weaponData.weaponName : "Unknown";

        private void Awake()
        {
            if (weaponData == null)
            {
                Debug.LogWarning($"MeleeWeapon en '{gameObject.name}' no tiene WeaponData asignado");
            }
        }

        private void Start()
        {
            // Instanciar modelo del arma si está configurado
            if (weaponData != null && weaponData.weaponModel != null && weaponModelInstance == null)
            {
                weaponModelInstance = Instantiate(weaponData.weaponModel, transform);
                weaponModelInstance.transform.localPosition = Vector3.zero;
                weaponModelInstance.transform.localRotation = Quaternion.identity;
            }
        }

        /// <summary>
        /// Inicializa el arma con datos
        /// </summary>
        public void Initialize(MeleeWeaponDataSO data)
        {
            weaponData = data;
            currentState = MeleeWeaponState.Idle;
            isBlocking = false;
            lastAttackTime = 0f;

            // Instanciar modelo si es necesario
            if (weaponData != null && weaponData.weaponModel != null && weaponModelInstance == null)
            {
                weaponModelInstance = Instantiate(weaponData.weaponModel, transform);
                weaponModelInstance.transform.localPosition = Vector3.zero;
                weaponModelInstance.transform.localRotation = Quaternion.identity;
            }
        }

        /// <summary>
        /// Verifica si el arma puede atacar
        /// </summary>
        public bool CanAttack()
        {
            if (weaponData == null) return false;
            if (currentState == MeleeWeaponState.Attacking) return false;
            if (currentState == MeleeWeaponState.Cooldown) return false;
            if (isBlocking) return false;

            float timeSinceLastAttack = Time.time - lastAttackTime;
            return timeSinceLastAttack >= weaponData.attackCooldown;
        }

        /// <summary>
        /// Verifica si el arma puede bloquear
        /// </summary>
        public bool CanBlock()
        {
            if (weaponData == null) return false;
            if (currentState == MeleeWeaponState.Attacking) return false;
            return true;
        }

        /// <summary>
        /// Inicia un ataque
        /// </summary>
        public void StartAttack()
        {
            if (!CanAttack()) return;

            currentState = MeleeWeaponState.Attacking;
            lastAttackTime = Time.time;

            // Reproducir sonido de swing
            if (weaponData != null && weaponData.swingSound != null)
            {
                PlaySound(weaponData.swingSound);
            }
        }

        /// <summary>
        /// Finaliza un ataque y entra en cooldown
        /// </summary>
        public void EndAttack()
        {
            if (currentState == MeleeWeaponState.Attacking)
            {
                currentState = MeleeWeaponState.Cooldown;
                Invoke(nameof(ResetToIdle), weaponData != null ? weaponData.attackCooldown : 0.5f);
            }
        }

        /// <summary>
        /// Inicia el bloqueo
        /// </summary>
        public void StartBlock()
        {
            if (!CanBlock()) return;

            isBlocking = true;
            currentState = MeleeWeaponState.Blocking;
        }

        /// <summary>
        /// Detiene el bloqueo
        /// </summary>
        public void StopBlock()
        {
            if (isBlocking)
            {
                isBlocking = false;
                if (currentState == MeleeWeaponState.Blocking)
                {
                    currentState = MeleeWeaponState.Idle;
                }
            }
        }

        /// <summary>
        /// Reproduce un sonido del arma
        /// </summary>
        public void PlaySound(AudioClip clip)
        {
            if (clip != null)
            {
                // Usar el sistema de audio existente si está disponible
                if (CGameEvents.OnPlaySound != null)
                {
                    // Por ahora, solo reproducir directamente
                    AudioSource.PlayClipAtPoint(clip, transform.position);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(clip, transform.position);
                }
            }
        }

        /// <summary>
        /// Resetea el estado a Idle
        /// </summary>
        private void ResetToIdle()
        {
            if (currentState == MeleeWeaponState.Cooldown && !isBlocking)
            {
                currentState = MeleeWeaponState.Idle;
            }
        }

        /// <summary>
        /// Obtiene el LayerMask para detección de objetivos
        /// </summary>
        public LayerMask GetTargetLayers()
        {
            if (weaponData != null && weaponData.targetLayers.value != 0)
            {
                return weaponData.targetLayers;
            }
            return -1; // Todos los layers
        }

        /// <summary>
        /// Obtiene el radio de ataque (para SphereCast)
        /// </summary>
        public float GetAttackRadius()
        {
            return weaponData != null ? weaponData.attackRadius : 0.1f;
        }
    }
}
