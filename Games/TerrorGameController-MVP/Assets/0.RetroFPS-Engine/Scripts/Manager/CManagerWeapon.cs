using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace RetroFPS
{
    public class CManagerWeapon : MonoBehaviour //IDataPersistence
    {
        [Header("Sistema de Armas")]
        [SerializeField] private WeaponInventory weaponInventory;
        [SerializeField] private List<GameObject> weapons; // Lista legacy para compatibilidad
        [SerializeField] private float weaponSwitchDelay = 0.3f; // Tiempo de cambio de arma

        private int currentWeaponIndex = 0;
        private bool isSwitchingWeapon = false;

        private void Start()
        {
            InitializeWeapons();
        }

        private void InitializeWeapons()
        {
            if (weaponInventory != null)
            {
                weapons = weaponInventory.GetAllWeapons();
            }
            else
            {
                if (weapons == null || weapons.Count == 0)
                {
                    weapons = GetComponentsInChildren<CWeaponController>(true)
                                      .Select(weaponComponent => weaponComponent.gameObject)
                                      .ToList();
                }
            }

            if (weapons.Count > 0)
            {
                SwitchWeapon(0);
            }
            else
            {
                Debug.LogWarning("No se encontraron armas para inicializar");
            }
        }
    
    private void Update()
    {
        if (isSwitchingWeapon) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            NextWeapon();
        }
        else if (scroll < 0f)
        {
            PreviousWeapon();
        }

        HandleQuickSlots();
    }

    private void HandleQuickSlots()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int slotIndex = i - 1;
                if (slotIndex < weapons.Count)
                {
                    SwitchWeapon(slotIndex);
                }
            }
        }
    }

        private void NextWeapon()
        {
            currentWeaponIndex++;
            if (currentWeaponIndex >= weapons.Count)
            {
                currentWeaponIndex = 0; // Vuelta al inicio
            }

            SwitchWeapon(currentWeaponIndex);
        }

        private void PreviousWeapon()
        {
            currentWeaponIndex--;
            if (currentWeaponIndex < 0)
            {
                currentWeaponIndex = weapons.Count - 1; // Vuelta al final
            }

            SwitchWeapon(currentWeaponIndex);
        }

        private void SwitchWeapon(int newWeaponIndex)
        {
            if (newWeaponIndex < 0 || newWeaponIndex >= weapons.Count)
            {
                Debug.LogWarning($"Índice de arma inválido: {newWeaponIndex}");
                return;
            }

            if (newWeaponIndex == currentWeaponIndex && weapons[currentWeaponIndex].activeSelf)
            {
                return;
            }

            StartCoroutine(SwitchWeaponCoroutine(newWeaponIndex));
        }

        private IEnumerator SwitchWeaponCoroutine(int newWeaponIndex)
        {
            isSwitchingWeapon = true;

            if (weapons[currentWeaponIndex] != null)
            {
                weapons[currentWeaponIndex].SetActive(false);
            }

            yield return new WaitForSeconds(weaponSwitchDelay);

            currentWeaponIndex = newWeaponIndex;
            if (weapons[currentWeaponIndex] != null)
            {
                weapons[currentWeaponIndex].SetActive(true);
            }

            isSwitchingWeapon = false;
        }

        public void AddWeapon(GameObject weaponPrefab)
        {
            if (weaponPrefab == null)
            {
                Debug.LogWarning("Intento de agregar arma null");
                return;
            }

            if (weapons.Contains(weaponPrefab))
            {
                Debug.LogWarning($"Arma {weaponPrefab.name} ya está en la lista");
                return;
            }

            weapons.Add(weaponPrefab);
            weaponPrefab.SetActive(false);

            if (weaponInventory != null)
            {
                weaponInventory.AddWeapon(weaponPrefab);
            }

            Debug.Log($"Arma {weaponPrefab.name} agregada al manager");
        }

        public void RemoveWeapon(int index)
        {
            if (index < 0 || index >= weapons.Count)
            {
                Debug.LogWarning($"Índice inválido para remover arma: {index}");
                return;
            }

            GameObject weapon = weapons[index];
            weapons.RemoveAt(index);

            if (weaponInventory != null)
            {
                weaponInventory.RemoveWeapon(weapon);
            }

            if (currentWeaponIndex >= weapons.Count && weapons.Count > 0)
            {
                currentWeaponIndex = weapons.Count - 1;
                SwitchWeapon(currentWeaponIndex);
            }
            else if (weapons.Count == 0)
            {
                currentWeaponIndex = 0;
            }

            Debug.Log($"Arma en índice {index} removida del manager");
        }

        public GameObject GetCurrentWeapon()
        {
            if (currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count)
            {
                return weapons[currentWeaponIndex];
            }
            return null;
        }

        public int GetCurrentWeaponIndex()
        {
            return currentWeaponIndex;
        }

        public List<GameObject> GetAllWeapons()
        {
            return new List<GameObject>(weapons);
        }
    }
}


