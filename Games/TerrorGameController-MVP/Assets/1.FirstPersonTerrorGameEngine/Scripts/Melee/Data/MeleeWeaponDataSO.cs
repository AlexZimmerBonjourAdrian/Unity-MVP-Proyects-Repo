using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Tipo de arma cuerpo a cuerpo
    /// </summary>
    public enum MeleeWeaponType
    {
        Light,      // Arma ligera, rápida, daño moderado
        Heavy,      // Arma pesada, lenta, daño alto
        Blocking    // Arma optimizada para bloqueo
    }

    /// <summary>
    /// ScriptableObject para configurar armas cuerpo a cuerpo.
    /// Permite a los diseñadores crear y configurar armas melee desde el editor de Unity.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMeleeWeapon", menuName = "HorrorEngine/Melee/Melee Weapon Data", order = 1)]
    public class MeleeWeaponDataSO : ScriptableObject
    {
        [Header("Información Básica")]
        [Tooltip("Nombre del arma")]
        public string weaponName = "Melee Weapon";

        [TextArea(2, 4)]
        [Tooltip("Descripción del arma")]
        public string description = "";

        [Tooltip("Icono del arma (opcional)")]
        public Sprite icon;

        [Header("Estadísticas de Ataque")]
        [Tooltip("Daño base del ataque")]
        [Range(1, 100)]
        public int damage = 25;

        [Tooltip("Rango de ataque en metros")]
        [Range(0.5f, 5f)]
        public float range = 2.5f;

        [Tooltip("Velocidad de ataque (ataques por segundo)")]
        [Range(0.5f, 5f)]
        public float attackSpeed = 2.0f;

        [Tooltip("Tiempo de cooldown entre ataques en segundos")]
        [Range(0.1f, 2f)]
        public float attackCooldown = 0.5f;

        [Header("Estadísticas de Bloqueo")]
        [Tooltip("Porcentaje de reducción de daño al bloquear (0 = sin reducción, 1 = bloqueo completo)")]
        [Range(0f, 1f)]
        public float blockDamageReduction = 0.5f;

        [Tooltip("Costo de stamina por bloqueo (opcional, 0 = sin costo)")]
        [Range(0f, 100f)]
        public float blockStaminaCost = 10f;

        [Tooltip("Costo de stamina por ataque (opcional, 0 = sin costo)")]
        [Range(0f, 100f)]
        public float attackStaminaCost = 5f;

        [Header("Tipo de Arma")]
        [Tooltip("Tipo de arma melee")]
        public MeleeWeaponType weaponType = MeleeWeaponType.Light;

        [Header("Audio")]
        [Tooltip("Sonido al realizar un swing")]
        public AudioClip swingSound;

        [Tooltip("Sonido al impactar con el arma")]
        public AudioClip hitSound;

        [Tooltip("Sonido al bloquear un ataque")]
        public AudioClip blockSound;

        [Header("Visual")]
        [Tooltip("Modelo del arma (opcional, puede ser instanciado)")]
        public GameObject weaponModel;

        [Tooltip("Prefab de efecto de impacto (opcional)")]
        public GameObject hitEffectPrefab;

        [Header("Configuración Avanzada")]
        [Tooltip("LayerMask para detectar objetivos (dejar vacío para usar todos)")]
        public LayerMask targetLayers = -1;

        [Tooltip("Radio del SphereCast para área de ataque más amplia (0 = usar Raycast normal)")]
        [Range(0f, 1f)]
        public float attackRadius = 0.1f;

        /// <summary>
        /// Valida que la configuración del arma sea correcta
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(weaponName))
            {
                Debug.LogWarning($"MeleeWeaponDataSO '{name}' tiene un weaponName vacío");
                return false;
            }

            if (damage <= 0)
            {
                Debug.LogWarning($"MeleeWeaponDataSO '{name}' tiene damage <= 0");
                return false;
            }

            if (range <= 0)
            {
                Debug.LogWarning($"MeleeWeaponDataSO '{name}' tiene range <= 0");
                return false;
            }

            if (attackSpeed <= 0)
            {
                Debug.LogWarning($"MeleeWeaponDataSO '{name}' tiene attackSpeed <= 0");
                return false;
            }

            return true;
        }
    }
}
