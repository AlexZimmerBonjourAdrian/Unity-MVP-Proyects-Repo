using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RetroFPS.Weapon
{
    public class CWeapon : MonoBehaviour
    {
        private CWeaponController weaponController; // Referencia al controlador de armas

        // Datos del arma
        [SerializeField]public string Nombre; // Nombre del arma
        [SerializeField]public int Damage; // Daño que inflige el arma
        [SerializeField]public int MaxMag; // Capacidad máxima del cargador
        [SerializeField]public float FireRate; // Velocidad de disparo (balas por segundo)
        [SerializeField]public float ReloadTime; // Tiempo de recarga en segundos
        [SerializeField]public int CurrentAmmo; // Munición actual en el cargador
        [SerializeField]public int TotalAmmo; // Munición total disponible

        private void Awake()
        {
            // Obtener referencia al controlador de armas
            weaponController = GetComponent<CWeaponController>();
            if (weaponController == null)
            {
                Debug.LogError("CWeaponController no encontrado en el GameObject.");
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

        public void Update()
        {
            Fire();
        }
        // Método para inicializar el arma con valores predeterminados
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
    }
}
