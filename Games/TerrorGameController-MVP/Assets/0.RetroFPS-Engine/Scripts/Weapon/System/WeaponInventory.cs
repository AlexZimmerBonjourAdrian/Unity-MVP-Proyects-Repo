using System.Collections.Generic;
using UnityEngine;

namespace RetroFPS
{
    public class WeaponInventory : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private int maxWeapons = 10;
        
        [Header("Armas Disponibles")]
        [SerializeField] private List<GameObject> availableWeapons = new List<GameObject>();

        private List<GameObject> currentWeapons = new List<GameObject>();

        public int WeaponCount => currentWeapons.Count;
        public bool IsFull => currentWeapons.Count >= maxWeapons;

        private void Awake()
        {
            InitializeWeapons();
        }

        private void InitializeWeapons()
        {
            foreach (var weapon in availableWeapons)
            {
                if (weapon != null)
                {
                    AddWeapon(weapon);
                }
            }
        }

        public bool AddWeapon(GameObject weaponPrefab)
        {
            if (weaponPrefab == null)
            {
                Debug.LogWarning("Intento de agregar arma null al inventario");
                return false;
            }

            if (IsFull)
            {
                Debug.LogWarning("Inventario de armas lleno");
                return false;
            }

            if (currentWeapons.Contains(weaponPrefab))
            {
                Debug.LogWarning($"Arma {weaponPrefab.name} ya está en el inventario");
                return false;
            }

            currentWeapons.Add(weaponPrefab);
            Debug.Log($"Arma {weaponPrefab.name} agregada al inventario");
            return true;
        }

        public bool RemoveWeapon(GameObject weaponPrefab)
        {
            if (weaponPrefab == null)
            {
                return false;
            }

            bool removed = currentWeapons.Remove(weaponPrefab);
            if (removed)
            {
                Debug.Log($"Arma {weaponPrefab.name} removida del inventario");
            }
            return removed;
        }

        public bool RemoveWeapon(int index)
        {
            if (index < 0 || index >= currentWeapons.Count)
            {
                return false;
            }

            GameObject weapon = currentWeapons[index];
            currentWeapons.RemoveAt(index);
            Debug.Log($"Arma en índice {index} removida del inventario");
            return true;
        }

        public GameObject GetWeapon(int index)
        {
            if (index < 0 || index >= currentWeapons.Count)
            {
                return null;
            }
            return currentWeapons[index];
        }

        public List<GameObject> GetAllWeapons()
        {
            return new List<GameObject>(currentWeapons);
        }

        public void Clear()
        {
            currentWeapons.Clear();
            Debug.Log("Inventario de armas limpiado");
        }
    }
}
