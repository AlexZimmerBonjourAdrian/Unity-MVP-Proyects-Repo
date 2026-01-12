using UnityEngine;

namespace RetroFPS
{
    public class RaycastWeaponBehavior : MonoBehaviour, IWeaponBehavior
    {
        private ShootSoundPlayer soundPlayer;

        private void Awake()
        {
            soundPlayer = GetComponent<ShootSoundPlayer>();
            if (soundPlayer == null)
            {
                soundPlayer = gameObject.AddComponent<ShootSoundPlayer>();
            }
        }

        public void Fire(WeaponData data, Transform firePoint, Camera camera, float range)
        {
            if (data == null || camera == null)
            {
                Debug.LogWarning("WeaponData o Camera es null en RaycastWeaponBehavior.Fire()");
                return;
            }

            if (soundPlayer != null && data.shootSound != null)
            {
                soundPlayer.shootSoundClip = data.shootSound;
                soundPlayer.PlayShootSound();
            }

            RaycastHit hit;
            Vector3 rayOrigin = camera.transform.position;
            Vector3 rayDirection = camera.transform.forward;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, range))
            {
                IDamage damageable = hit.collider.GetComponent<IDamage>();
                if (damageable != null)
                {
                    damageable.TakeDamage(data.damage, hit.point, hit.normal);
                }
            }
        }

        public bool CanFire(WeaponData data, int currentAmmo)
        {
            return data != null && currentAmmo > 0;
        }

        public void OnReload(WeaponData data)
        {
            if (data != null && data.reloadSound != null && soundPlayer != null)
            {
                soundPlayer.shootSoundClip = data.reloadSound;
                soundPlayer.PlayShootSound();
            }
        }
    }
}
