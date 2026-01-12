using UnityEngine;

namespace RetroFPS
{
    public interface IWeaponBehavior
    {
        void Fire(WeaponData data, Transform firePoint, Camera camera, float range);
        bool CanFire(WeaponData data, int currentAmmo);
        void OnReload(WeaponData data);
    }
}
