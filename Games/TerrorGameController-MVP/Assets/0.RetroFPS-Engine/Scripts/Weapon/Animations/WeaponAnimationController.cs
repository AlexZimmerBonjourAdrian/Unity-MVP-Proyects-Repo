using UnityEngine;

namespace RetroFPS
{
    public class WeaponAnimationController : MonoBehaviour
    {
        [Header("Component References")]
        public WeaponSway weaponSway;
        public WeaponRecoil weaponRecoil;
        public WeaponADS weaponADS;
        public WeaponSprint weaponSprint;
        public WeaponReloadAnimation weaponReloadAnimation;
        public WeaponIdle weaponIdle;

        [Header("Settings")]
        public bool enableAnimations = true;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            if (weaponSway == null)
                weaponSway = GetComponent<WeaponSway>();

            if (weaponRecoil == null)
                weaponRecoil = GetComponent<WeaponRecoil>();

            if (weaponADS == null)
                weaponADS = GetComponent<WeaponADS>();

            if (weaponSprint == null)
                weaponSprint = GetComponent<WeaponSprint>();

            if (weaponReloadAnimation == null)
                weaponReloadAnimation = GetComponent<WeaponReloadAnimation>();

            if (weaponIdle == null)
                weaponIdle = GetComponent<WeaponIdle>();
        }

        public void SetWalking(bool walking)
        {
            if (weaponSway != null)
                weaponSway.SetWalking(walking);
        }

        public void SetSprinting(bool sprinting)
        {
            if (weaponSprint != null)
                weaponSprint.SetSprinting(sprinting);

            if (weaponIdle != null)
                weaponIdle.SetActive(!sprinting);
        }

        public void ApplyRecoil(float verticalAmount, float horizontalAmount)
        {
            if (weaponRecoil != null && enableAnimations)
                weaponRecoil.ApplyRecoil(verticalAmount, horizontalAmount);
        }

        public void StartReload(float duration)
        {
            if (weaponReloadAnimation != null && enableAnimations)
                weaponReloadAnimation.StartReload(duration);
        }

        public void StopReload()
        {
            if (weaponReloadAnimation != null)
                weaponReloadAnimation.StopReload();
        }

        public bool IsAiming()
        {
            return weaponADS != null && weaponADS.IsAiming();
        }

        public bool IsSprinting()
        {
            return weaponSprint != null && weaponSprint.IsSprinting();
        }

        public void SetAiming(bool aiming)
        {
            if (weaponADS != null)
                weaponADS.SetAimingFromExternal(aiming);
        }
    }
}
