using System.Collections.Generic;
using UnityEngine;

namespace RetroFPS
{
    public class FPSConfigManager : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private FPSConfig config;

        [Header("Referencias Automáticas")]
        [SerializeField] private bool autoFindComponents = true;

        private CPlayer3DController playerController;
        private Camera mainCamera;
        private CManagerWeapon weaponManager;
        private HeadBob headBob;

        void Start()
        {
            if (autoFindComponents)
            {
                FindComponents();
            }

            if (config != null)
            {
                ApplyConfiguration();
            }
            else
            {
                Debug.LogWarning("[FPSConfigManager] No hay FPSConfig asignado. Asigna uno en el Inspector.");
            }
        }

        private void FindComponents()
        {
            if (playerController == null)
                playerController = FindObjectOfType<CPlayer3DController>();

            if (mainCamera == null)
                mainCamera = Camera.main;

            if (weaponManager == null)
                weaponManager = FindObjectOfType<CManagerWeapon>();

            if (headBob == null && mainCamera != null)
                headBob = mainCamera.GetComponent<HeadBob>();
        }

        public void ApplyConfiguration()
        {
            if (config == null)
            {
                Debug.LogWarning("[FPSConfigManager] No hay configuraciÃ³n para aplicar.");
                return;
            }

            FindComponents();

            if (playerController != null)
            {
                config.ApplyToPlayer(playerController);
                if (config.showDebugInfo)
                    Debug.Log("[FPSConfigManager] ConfiguraciÃ³n aplicada al jugador");
            }

            if (mainCamera != null)
            {
                config.ApplyToCamera(mainCamera);
                if (config.showDebugInfo)
                    Debug.Log("[FPSConfigManager] ConfiguraciÃ³n aplicada a la cÃ¡mara");
            }

            if (headBob != null)
            {
                config.ApplyToHeadBob(headBob);
                if (config.showDebugInfo)
                    Debug.Log("[FPSConfigManager] ConfiguraciÃ³n aplicada al HeadBob");
            }

            ApplyToAllWeapons();

            if (GlobalVariables.Instance != null)
            {
                if (config.godMode)
                    GlobalVariables.Instance.ToggleGodMode();
                if (config.infiniteAmmo)
                    GlobalVariables.Instance.ToggleInfiniteAmmo();
            }
        }

        public void ApplyToAllWeapons()
        {
            if (weaponManager == null) return;

            List<GameObject> weapons = weaponManager.GetAllWeapons();
            if (weapons == null) return;

            foreach (GameObject weaponObj in weapons)
            {
                if (weaponObj == null) continue;

                WeaponSway weaponSway = weaponObj.GetComponent<WeaponSway>();
                if (weaponSway != null)
                {
                    config.ApplyToWeaponSway(weaponSway);
                }

                WeaponRecoil weaponRecoil = weaponObj.GetComponent<WeaponRecoil>();
                if (weaponRecoil != null)
                {
                    config.ApplyToWeaponRecoil(weaponRecoil);
                }

                WeaponADS weaponADS = weaponObj.GetComponent<WeaponADS>();
                if (weaponADS != null)
                {
                    config.ApplyToWeaponADS(weaponADS);
                }

                WeaponSprint weaponSprint = weaponObj.GetComponent<WeaponSprint>();
                if (weaponSprint != null)
                {
                    config.ApplyToWeaponSprint(weaponSprint);
                }

                WeaponAnimationController animController = weaponObj.GetComponent<WeaponAnimationController>();
                if (animController != null)
                {
                    animController.enableAnimations = config.enableWeaponAnimations;
                }
            }

            if (config.showDebugInfo)
                Debug.Log($"[FPSConfigManager] ConfiguraciÃ³n aplicada a {weapons.Count} armas");
        }

        public void SetConfiguration(FPSConfig newConfig)
        {
            config = newConfig;
            ApplyConfiguration();
        }

        public FPSConfig GetConfiguration()
        {
            return config;
        }

        public void ReloadConfiguration()
        {
            ApplyConfiguration();
        }

        void OnValidate()
        {
            if (config != null && Application.isPlaying)
            {
                ApplyConfiguration();
            }
        }
    }
}
