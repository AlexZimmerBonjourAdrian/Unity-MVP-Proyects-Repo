using UnityEngine;

namespace RetroFPS
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Retro FPS/Weapons/Weapon Data", order = 1)]
    public class WeaponData : ScriptableObject
    {
        [Header("Información Básica")]
        public string weaponName;
        [TextArea(2, 4)]
        public string description;
        public Sprite icon;
        
        [Header("Estadísticas")]
        public int damage = 10;
        public int maxMagazine = 30;
        public float fireRate = 10f;
        public float reloadTime = 2f;
        public float range = 100f;
        public int maxAmmo = 200;
        
        [Header("Tipo de Disparo")]
        public WeaponFireMode fireMode = WeaponFireMode.Auto;
        public WeaponType weaponType = WeaponType.Rifle;
        
        [Header("Audio")]
        public AudioClip shootSound;
        public AudioClip reloadSound;
        public AudioClip emptySound;
        
        [Header("Visual")]
        public GameObject weaponModel;
        public GameObject muzzleFlash;

        [Header("Animation Settings")]
        public float swayAmount = 0.02f;
        public float recoilAmount = 2f;
        public float horizontalRecoilAmount = 1f;
        public float adsFOV = 40f;
        public Vector3 adsPosition = new Vector3(0, -0.1f, 0.2f);
    }
}
