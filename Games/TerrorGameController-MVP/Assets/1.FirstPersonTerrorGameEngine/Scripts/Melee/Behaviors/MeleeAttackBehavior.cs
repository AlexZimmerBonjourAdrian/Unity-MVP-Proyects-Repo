using UnityEngine;
using RetroFPS;

namespace HorrorEngine
{
    /// <summary>
    /// Información sobre un ataque melee exitoso
    /// </summary>
    public struct MeleeHitInfo
    {
        public bool hit;
        public Collider hitCollider;
        public Vector3 hitPoint;
        public Vector3 hitNormal;
        public IDamage damageable;
        public float distance;
    }

    /// <summary>
    /// Comportamiento de ataque para armas cuerpo a cuerpo.
    /// Maneja la detección de hits y aplicación de daño.
    /// </summary>
    public class MeleeAttackBehavior : MonoBehaviour
    {
        [Header("Configuración")]
        [Tooltip("Si es true, usa SphereCast en lugar de Raycast para área más amplia")]
        [SerializeField] private bool useSphereCast = false;

        [Tooltip("Debug: mostrar raycast en Scene view")]
        [SerializeField] private bool showDebugRay = false;

        private Camera playerCamera;

        private void Awake()
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                playerCamera = FindObjectOfType<Camera>();
            }
        }

        /// <summary>
        /// Realiza un ataque melee
        /// </summary>
        /// <param name="weapon">Arma que está atacando</param>
        /// <returns>Información sobre el hit</returns>
        public MeleeHitInfo PerformAttack(MeleeWeapon weapon)
        {
            MeleeHitInfo hitInfo = new MeleeHitInfo
            {
                hit = false
            };

            if (weapon == null || weapon.WeaponData == null)
            {
                return hitInfo;
            }

            if (playerCamera == null)
            {
                Debug.LogWarning("MeleeAttackBehavior: No se encontró cámara del jugador");
                return hitInfo;
            }

            Vector3 origin = playerCamera.transform.position;
            Vector3 direction = playerCamera.transform.forward;
            float range = weapon.Range;
            float radius = weapon.GetAttackRadius();
            LayerMask targetLayers = weapon.GetTargetLayers();

            // Realizar detección de hit
            if (useSphereCast && radius > 0.01f)
            {
                hitInfo = PerformSphereCast(origin, direction, range, radius, targetLayers);
            }
            else
            {
                hitInfo = PerformRaycast(origin, direction, range, targetLayers);
            }

            // Aplicar daño si se detectó un hit
            if (hitInfo.hit && hitInfo.damageable != null)
            {
                int damage = weapon.Damage;
                hitInfo.damageable.TakeDamage(damage, hitInfo.hitPoint, hitInfo.hitNormal);

                // Reproducir sonido de impacto
                if (weapon.WeaponData.hitSound != null)
                {
                    weapon.PlaySound(weapon.WeaponData.hitSound);
                }

                // Instanciar efecto de impacto si está configurado
                if (weapon.WeaponData.hitEffectPrefab != null)
                {
                    Instantiate(weapon.WeaponData.hitEffectPrefab, hitInfo.hitPoint, Quaternion.LookRotation(hitInfo.hitNormal));
                }

                // Debug
                if (showDebugRay)
                {
                    Debug.Log($"Melee hit: {hitInfo.hitCollider.name} for {damage} damage");
                }
            }
            else if (hitInfo.hit)
            {
                // Hit pero no es damageable (pared, objeto, etc.)
                if (weapon.WeaponData.hitSound != null)
                {
                    weapon.PlaySound(weapon.WeaponData.hitSound);
                }
            }

            // Visualizar raycast en editor
            if (showDebugRay)
            {
                Color rayColor = hitInfo.hit ? Color.red : Color.green;
                Debug.DrawRay(origin, direction * range, rayColor, 0.5f);
            }

            return hitInfo;
        }

        /// <summary>
        /// Realiza un Raycast para detectar hits
        /// </summary>
        private MeleeHitInfo PerformRaycast(Vector3 origin, Vector3 direction, float range, LayerMask targetLayers)
        {
            MeleeHitInfo hitInfo = new MeleeHitInfo
            {
                hit = false
            };

            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, range, targetLayers))
            {
                hitInfo.hit = true;
                hitInfo.hitCollider = hit.collider;
                hitInfo.hitPoint = hit.point;
                hitInfo.hitNormal = hit.normal;
                hitInfo.distance = hit.distance;
                hitInfo.damageable = hit.collider.GetComponent<IDamage>();
            }

            return hitInfo;
        }

        /// <summary>
        /// Realiza un SphereCast para detectar hits en un área más amplia
        /// </summary>
        private MeleeHitInfo PerformSphereCast(Vector3 origin, Vector3 direction, float range, float radius, LayerMask targetLayers)
        {
            MeleeHitInfo hitInfo = new MeleeHitInfo
            {
                hit = false
            };

            RaycastHit hit;
            if (Physics.SphereCast(origin, radius, direction, out hit, range, targetLayers))
            {
                hitInfo.hit = true;
                hitInfo.hitCollider = hit.collider;
                hitInfo.hitPoint = hit.point;
                hitInfo.hitNormal = hit.normal;
                hitInfo.distance = hit.distance;
                hitInfo.damageable = hit.collider.GetComponent<IDamage>();
            }

            return hitInfo;
        }

        /// <summary>
        /// Establece la cámara del jugador (útil si se cambia de cámara)
        /// </summary>
        public void SetCamera(Camera camera)
        {
            playerCamera = camera;
        }
    }
}
