using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RetroFPS.Damage;
namespace RetroFPS.Weapon
{

[RequireComponent(typeof(CWeapon))]
[RequireComponent(typeof(ShootSoundPlayer))]

public class CWeaponController : MonoBehaviour
{   
    public float range = 100f; // Rango del raycast
    public Color laserColor = Color.red; // Color del Gizmo

    [Tooltip("Arrastra aquí el componente ShootSoundPlayer para reproducir el sonido de disparo.")]

    public ShootSoundPlayer soundPlayer; // Asigna esto en el Inspector

    [SerializeField] private Camera mainCamera; // Cámara principal para el raycast
      void Start()
        {
            // Opcional: Comprobar si la referencia a soundPlayer se asignó en el Inspector
            if (soundPlayer == null)
            {
                // Intenta encontrarlo en el mismo GameObject si no se asignó
                soundPlayer = GetComponent<ShootSoundPlayer>();
                if (soundPlayer == null)
                {
                    Debug.LogWarning("No se encontró o asignó un ShootSoundPlayer en " + gameObject.name + ". El sonido de disparo no funcionará.", this);
                }
            }
        }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Mouse0)) 
    //     {
    //         Shoot();
    //     }
    // }

    // void Shoot()
    // {
    //      if (soundPlayer != null)
    //         {
    //             soundPlayer.PlayShootSound(); 
    //         }
    //         RaycastHit hit;
    //         if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range))
    //         {
    //             IDamage damageable = hit.collider.GetComponent<IDamage>();
    //             if (damageable != null)
    //             {
    //                 damageable.OnDamage();
    //             }
    //     }
    // }

    public void HandleFire(CWeapon weapon)
    {
        if (weapon.CurrentAmmo > 0)
        {
            weapon.CurrentAmmo--;
            Debug.Log($"{weapon.Nombre} disparó. Munición restante: {weapon.CurrentAmmo}/{weapon.MaxMag}");

            if (soundPlayer != null)
            {
                soundPlayer.PlayShootSound();
            }

            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, range))
            {
                IDamage damageable = hit.collider.GetComponent<IDamage>();
                if (damageable != null)
                {
                    damageable.OnDamage();
                }
            }
        }
        else
        {
            Debug.Log($"{weapon.Nombre} no tiene munición. Recarga necesaria.");
        }
    }

    public void HandleReload(CWeapon weapon)
    {
        int ammoNeeded = weapon.MaxMag - weapon.CurrentAmmo;
        if (ammoNeeded > 0 && weapon.TotalAmmo > 0)
        {
            int ammoToReload = Mathf.Min(ammoNeeded, weapon.TotalAmmo);
            weapon.CurrentAmmo += ammoToReload;
            weapon.TotalAmmo -= ammoToReload;

            Debug.Log($"{weapon.Nombre} recargado. Munición actual: {weapon.CurrentAmmo}/{weapon.MaxMag}. Munición restante: {weapon.TotalAmmo}");
        }
        else if (weapon.TotalAmmo == 0)
        {
            Debug.Log($"{weapon.Nombre} no tiene munición restante para recargar.");
        }
        else
        {
            Debug.Log($"{weapon.Nombre} ya está completamente cargado.");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = laserColor;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * range);
    }
}

}


