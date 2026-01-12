using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RetroFPS
{

[RequireComponent(typeof(CWeapon))]
[RequireComponent(typeof(ShootSoundPlayer))]

public class CWeaponController : MonoBehaviour
{
    [Header("Configuraci?n de Disparo")]
    public float range = 100f; // Rango del raycast
    public Color laserColor = Color.red; // Color del Gizmo

    [Header("Referencias")]
    [Tooltip("Arrastra aqu? el componente ShootSoundPlayer para reproducir el sonido de disparo.")]
    public ShootSoundPlayer soundPlayer; // Asigna esto en el Inspector
    [SerializeField] private Camera mainCamera; // C?mara principal para el raycast

    private CWeapon weapon;
    private IWeaponBehavior weaponBehavior;
    private WeaponRecoil weaponRecoil;
    private WeaponReloadAnimation weaponReloadAnimation;
    private WeaponAnimationController weaponAnimationController;
    private FPSConfigManager fpsConfigManager;
    private float nextTimeToFire = 0f;
    private bool isReloading = false;

    void Start()
    {
        weapon = GetComponent<CWeapon>();
        if (weapon == null)
        {
            Debug.LogError("CWeapon no encontrado en el GameObject.");
        }

        weaponBehavior = GetWeaponBehavior();
        if (weaponBehavior == null)
        {
            Debug.LogError("No se pudo obtener IWeaponBehavior en " + gameObject.name);
        }

        weaponRecoil = GetComponent<WeaponRecoil>();
        weaponReloadAnimation = GetComponent<WeaponReloadAnimation>();
        weaponAnimationController = GetComponent<WeaponAnimationController>();
        fpsConfigManager = FindObjectOfType<FPSConfigManager>();

        if (soundPlayer == null)
        {
            soundPlayer = GetComponent<ShootSoundPlayer>();
            if (soundPlayer == null)
            {
                Debug.LogWarning("No se encontr? o asign? un ShootSoundPlayer en " + gameObject.name + ". El sonido de disparo no funcionar?.", this);
            }
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("No se encontr? la c?mara principal. El sistema de disparo no funcionar?.");
            }
        }
    }

    private IWeaponBehavior GetWeaponBehavior()
    {
        IWeaponBehavior behavior = GetComponent<IWeaponBehavior>();
        
        if (behavior == null)
        {
            behavior = gameObject.AddComponent<RaycastWeaponBehavior>();
            Debug.Log($"RaycastWeaponBehavior agregado automáticamente a {gameObject.name}");
        }
        
        return behavior;
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (weapon == null || isReloading) return;

        bool puzzleMode = false;
        if (CGameManager.Inst != null)
        {
            puzzleMode = CGameManager.Inst.GetPuzzleMode();
        }

        if (puzzleMode) return;

        KeyCode fireKey = GetFireKey();
        KeyCode reloadKey = GetReloadKey();

        if (Input.GetKey(fireKey) || (fireKey == KeyCode.Mouse0 && Input.GetButton("Fire1")))
        {
            TryFire();
        }

        if (Input.GetKeyDown(reloadKey))
        {
            StartReload();
        }
    }

    private void TryFire()
    {
        if (weapon == null || isReloading) return;

        if (weapon.CurrentAmmo <= 0)
        {
            if (weapon.TotalAmmo > 0)
            {
                StartReload();
            }
            return;
        }

        if (Time.time >= nextTimeToFire)
        {
            float timeBetweenShots = 1f / weapon.WeaponFireRate;
            nextTimeToFire = Time.time + timeBetweenShots;
            HandleFire(weapon);
        }
    }

    public void HandleFire(CWeapon weapon)
    {
        if (weapon == null || weapon.CurrentAmmo <= 0 || isReloading) return;

        if (weaponBehavior == null)
        {
            Debug.LogError("IWeaponBehavior es null en HandleFire");
            return;
        }

        WeaponData data = weapon.Data;
        if (data == null)
        {
            Debug.LogWarning("WeaponData es null, usando comportamiento legacy");
            HandleFireLegacy(weapon);
            return;
        }

        if (!weaponBehavior.CanFire(data, weapon.CurrentAmmo))
        {
            return;
        }

        weapon.CurrentAmmo--;

        Transform firePoint = transform;
        float weaponRange = weapon.WeaponRange;
        weaponBehavior.Fire(data, firePoint, mainCamera, weaponRange);

        ApplyRecoil(data);
    }

    private void ApplyRecoil(WeaponData data)
    {
        if (weaponRecoil != null && data != null)
        {
            float verticalRecoil = data.recoilAmount;
            float horizontalRecoil = data.horizontalRecoilAmount;
            weaponRecoil.ApplyRecoil(verticalRecoil, horizontalRecoil);
        }
        else if (weaponAnimationController != null && data != null)
        {
            weaponAnimationController.ApplyRecoil(data.recoilAmount, data.horizontalRecoilAmount);
        }
    }

    private void HandleFireLegacy(CWeapon weapon)
    {
        weapon.CurrentAmmo--;

        if (soundPlayer != null)
        {
            soundPlayer.PlayShootSound();
        }

        RaycastHit hit;
        Vector3 rayOrigin = mainCamera.transform.position;
        Vector3 rayDirection = mainCamera.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, range))
        {
            IDamage damageable = hit.collider.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(weapon.WeaponDamage, hit.point, hit.normal);
            }
        }
    }

    public void StartReload()
    {
        if (isReloading || weapon == null) return;

        if (weapon.CurrentAmmo >= weapon.MaxMagazine)
        {
            return;
        }

        if (weapon.TotalAmmo <= 0)
        {
            return;
        }

        StartCoroutine(ReloadCoroutine());
    }

    private System.Collections.IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        weapon.SetReloading(true);

        StartReloadAnimation(weapon.WeaponReloadTime);

        yield return new WaitForSeconds(weapon.WeaponReloadTime);

        if (weaponBehavior != null && weapon.Data != null)
        {
            weaponBehavior.OnReload(weapon.Data);
        }

        int ammoNeeded = weapon.MaxMagazine - weapon.CurrentAmmo;
        if (ammoNeeded > 0 && weapon.TotalAmmo > 0)
        {
            int ammoToReload = Mathf.Min(ammoNeeded, weapon.TotalAmmo);
            weapon.CurrentAmmo += ammoToReload;
            weapon.TotalAmmo -= ammoToReload;
        }

        isReloading = false;
        weapon.SetReloading(false);

        StopReloadAnimation();
    }

    private void StartReloadAnimation(float duration)
    {
        if (weaponReloadAnimation != null)
        {
            weaponReloadAnimation.StartReload(duration);
        }
        else if (weaponAnimationController != null)
        {
            weaponAnimationController.StartReload(duration);
        }
    }

    private void StopReloadAnimation()
    {
        if (weaponReloadAnimation != null)
        {
            weaponReloadAnimation.StopReload();
        }
        else if (weaponAnimationController != null)
        {
            weaponAnimationController.StopReload();
        }
    }

    public void HandleReload(CWeapon weapon)
    {
        StartReload();
    }

    private KeyCode GetFireKey()
    {
        if (fpsConfigManager != null && fpsConfigManager.GetConfiguration() != null)
        {
            return fpsConfigManager.GetConfiguration().fireKey;
        }
        return KeyCode.Mouse0;
    }

    private KeyCode GetReloadKey()
    {
        if (fpsConfigManager != null && fpsConfigManager.GetConfiguration() != null)
        {
            return fpsConfigManager.GetConfiguration().reloadKey;
        }
        return KeyCode.R;
    }

    void OnDrawGizmos()
    {
        if (mainCamera != null)
        {
            float weaponRange = weapon != null ? weapon.WeaponRange : range;
            Gizmos.color = laserColor;
            Gizmos.DrawLine(mainCamera.transform.position, mainCamera.transform.position + mainCamera.transform.forward * weaponRange);
        }
    }
}
}
