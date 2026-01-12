using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RetroFPS
{
    public class CWeapon : MonoBehaviour
    {
        private CWeaponController weaponController; // Referencia al controlador de armas

        [Header("Datos del Arma - Nuevo Sistema")]
        [SerializeField] private WeaponData weaponData; // ScriptableObject con datos del arma

        [Header("Datos del Arma - Legacy (Compatibilidad)")]
        [SerializeField] public string Nombre; // Nombre del arma (legacy)
        [SerializeField] public int Damage; // Daño que inflige el arma (legacy)
        [SerializeField] public int MaxMag; // Capacidad máxima del cargador (legacy)
        [SerializeField] public float FireRate = 10f; // Velocidad de disparo (legacy)
        [SerializeField] public float ReloadTime = 2f; // Tiempo de recarga (legacy)
        [SerializeField] public int CurrentAmmo; // Munición actual en el cargador
        [SerializeField] public int TotalAmmo; // Munición total disponible

        [Header("Estado")]
        public bool IsReloading { get; private set; }

        // Propiedades que leen de weaponData (con fallback a legacy)
        public string WeaponName => weaponData != null ? weaponData.weaponName : Nombre;
        public int WeaponDamage => weaponData != null ? weaponData.damage : Damage;
        public int MaxMagazine => weaponData != null ? weaponData.maxMagazine : MaxMag;
        public float WeaponFireRate => weaponData != null ? weaponData.fireRate : FireRate;
        public float WeaponReloadTime => weaponData != null ? weaponData.reloadTime : ReloadTime;
        public float WeaponRange => weaponData != null ? weaponData.range : 100f;
        public WeaponData Data => weaponData;

        private void Awake()
        {
            // Obtener referencia al controlador de armas
            weaponController = GetComponent<CWeaponController>();
            if (weaponController == null)
            {
                Debug.LogError("CWeaponController no encontrado en el GameObject.");
            }

            // Inicializar desde WeaponData si está disponible
            if (weaponData != null)
            {
                InitializeFromData();
            }
            else if (!string.IsNullOrEmpty(Nombre))
            {
                // Usar datos legacy
                Debug.LogWarning($"[CWeapon] {gameObject.name} está usando datos legacy. Considera migrar a WeaponData ScriptableObject.");
            }
        }

        private void InitializeFromData()
        {
            if (weaponData == null) return;

            // Sincronizar valores legacy para compatibilidad
            Nombre = weaponData.weaponName;
            Damage = weaponData.damage;
            MaxMag = weaponData.maxMagazine;
            FireRate = weaponData.fireRate;
            ReloadTime = weaponData.reloadTime;

            // Inicializar munición
            if (CurrentAmmo == 0)
            {
                CurrentAmmo = weaponData.maxMagazine;
            }
            if (TotalAmmo == 0)
            {
                TotalAmmo = weaponData.maxAmmo;
            }
        }

        // Método para disparar el arma
        public void Fire()
        {
            if (weaponController != null)
            {
                weaponController.HandleFire(this);
            }
        }

        // Método para recargar el arma
        public void Reload()
        {
            if (weaponController != null)
            {
                weaponController.HandleReload(this);
            }
        }

        public void SetReloading(bool reloading)
        {
            IsReloading = reloading;
        }

        // Método para inicializar el arma con valores predeterminados (legacy)
        public void Initialize(string nombre, int damage, int maxMag, float fireRate, float reloadTime, int totalAmmo)
        {
            Nombre = nombre;
            Damage = damage;
            MaxMag = maxMag;
            FireRate = fireRate;
            ReloadTime = reloadTime;
            TotalAmmo = totalAmmo;
            CurrentAmmo = maxMag; // Inicializa el cargador lleno

            Debug.Log($"{Nombre} inicializado con {CurrentAmmo}/{MaxMag} balas y {TotalAmmo} balas totales.");
        }

        // Método para inicializar desde WeaponData
        public void Initialize(WeaponData data)
        {
            if (data == null)
            {
                Debug.LogError("Intento de inicializar arma con WeaponData null");
                return;
            }

            weaponData = data;
            InitializeFromData();
            Debug.Log($"{WeaponName} inicializado desde WeaponData con {CurrentAmmo}/{MaxMagazine} balas y {TotalAmmo} balas totales.");
        }
    }
}
