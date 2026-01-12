using UnityEngine;

namespace HorrorEngine
{
    public class WeaponAnimationManager : MonoBehaviour
{// Reference to WeaponAim for aiming animations

     private ProceduralAnimation proceduralAnimaton;
    private WeaponRecoil weaponRecoil;
    private WeaponReload weaponReload;
    private CameraShake cameraShake;
    
    void Start()
    {
        proceduralAnimaton = GetComponent<ProceduralAnimation>();
        weaponRecoil = GetComponent<WeaponRecoil>();
        weaponReload = GetComponent<WeaponReload>();
        cameraShake = GetComponent<CameraShake>();
    }
    }
}