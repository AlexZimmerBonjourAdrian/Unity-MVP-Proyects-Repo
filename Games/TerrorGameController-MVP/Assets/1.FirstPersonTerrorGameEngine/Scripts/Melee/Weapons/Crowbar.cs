using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Implementación específica de la palanca (Crowbar) estilo Half-Life.
    /// Arma rápida y ligera, perfecta para el inicio del juego.
    /// </summary>
    public class Crowbar : MonoBehaviour
    {
        [Header("Configuración de Crowbar")]
        [Tooltip("Si es true, usa los valores por defecto de Half-Life")]
        [SerializeField] private bool useHalfLifeDefaults = true;

        [Tooltip("Datos del arma (se puede crear desde el menú de Unity)")]
        [SerializeField] private MeleeWeaponDataSO crowbarData;

        private MeleeWeapon meleeWeapon;

        private void Awake()
        {
            // Si no hay datos asignados y se usan defaults, crear datos básicos
            if (crowbarData == null && useHalfLifeDefaults)
            {
                CreateDefaultCrowbarData();
            }

            // Agregar componente MeleeWeapon
            meleeWeapon = GetComponent<MeleeWeapon>();
            if (meleeWeapon == null)
            {
                meleeWeapon = gameObject.AddComponent<MeleeWeapon>();
            }

            // Inicializar con datos
            if (crowbarData != null)
            {
                meleeWeapon.Initialize(crowbarData);
            }
        }

        /// <summary>
        /// Crea datos por defecto estilo Half-Life
        /// Nota: Esto es solo para referencia, se recomienda crear un ScriptableObject
        /// </summary>
        private void CreateDefaultCrowbarData()
        {
            Debug.LogWarning("Crowbar: Se recomienda crear un MeleeWeaponDataSO desde el menú de Unity. " +
                           "Usando valores por defecto de Half-Life.");

            // Los valores por defecto se configurarían así en un ScriptableObject:
            // weaponName = "Crowbar"
            // damage = 25
            // range = 2.5f
            // attackSpeed = 2.0f (2 ataques por segundo)
            // attackCooldown = 0.5f
            // blockDamageReduction = 0.5f (reduce 50% del daño)
            // weaponType = Light
        }

        /// <summary>
        /// Establece los datos del arma
        /// </summary>
        public void SetWeaponData(MeleeWeaponDataSO data)
        {
            crowbarData = data;
            if (meleeWeapon != null)
            {
                meleeWeapon.Initialize(data);
            }
        }

        /// <summary>
        /// Obtiene el componente MeleeWeapon
        /// </summary>
        public MeleeWeapon GetMeleeWeapon()
        {
            return meleeWeapon;
        }
    }
}
