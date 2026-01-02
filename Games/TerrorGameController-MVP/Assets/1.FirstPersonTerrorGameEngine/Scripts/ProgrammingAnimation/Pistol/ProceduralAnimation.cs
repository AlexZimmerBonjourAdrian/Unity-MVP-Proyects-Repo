using UnityEngine;


[RequireComponent(typeof(WeaponAnimationManager))]
[RequireComponent(typeof(WeaponRecoil))]
[RequireComponent(typeof(WeaponSway))]
[RequireComponent(typeof(WeaponReload))]
[RequireComponent(typeof(CameraShake))]
[RequireComponent(typeof(WeaponAim))]
public class ProceduralAnimation : MonoBehaviour
{
    private WeaponAnimationManager weaponAnimationManager;
    private WeaponRecoil weaponRecoil;
    private WeaponSway weaponSway;
    private WeaponReload weaponReload;
    private CameraShake cameraShake;
    private WeaponAim weaponAim;

    [SerializeField] protected Transform InitialTransform;

    [SerializeField] protected  Transform RecoilTransform;

    [SerializeField] protected  Transform RealoadTransform;

     [SerializeField] protected  Transform AimTransform;

     [SerializeField] protected  Quaternion SwayTransform;

    private bool isAiming = false; // Tracks if the weapon is aiming

    void Start()
    {
        // Obtener referencias a los componentes necesarios
        weaponAnimationManager = GetComponent<WeaponAnimationManager>();
        weaponRecoil = GetComponent<WeaponRecoil>();
        weaponSway = GetComponent<WeaponSway>();
        weaponReload = GetComponent<WeaponReload>();
        cameraShake = GetComponent<CameraShake>();
        weaponAim = GetComponent<WeaponAim>();

        // Verificar que todos los componentes estén presentes
        if (weaponAnimationManager == null) Debug.LogError("WeaponAnimationManager not found.");
        if (weaponRecoil == null) Debug.LogError("WeaponRecoil not found.");
        if (weaponSway == null) Debug.LogError("WeaponSway not found.");
        if (weaponReload == null) Debug.LogError("WeaponReload not found.");
        if (cameraShake == null) Debug.LogError("CameraShake not found.");
        if (weaponAim == null) Debug.LogError("WeaponAim not found.");
    }

    void Update()
    {
        // Ejemplo de uso: Disparar y aplicar retroceso al presionar el botón izquierdo del ratón
        if (Input.GetMouseButtonDown(0)) // Botón izquierdo del ratón
        {
            weaponRecoil.ApplyRecoil();
        }

        // Ejemplo de uso: Recargar al presionar la tecla R
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        //     weaponAnimationManager.TriggerReload();
        // }
    }
    
    public Transform GetInitialTransform()
    {
        return InitialTransform;
    }
    public Transform GetRecoilTransform()
    {
        return RecoilTransform;
    }
    public Transform GetReloadTransform()
    {
        return RealoadTransform;
    }
    public Transform GetAimTransform()
    {
        return AimTransform;
    }
    public bool IsAiming()
    {
        return isAiming;
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }

    public WeaponAnimationManager GetWeaponAnimationManager()
    {
        return weaponAnimationManager;
    }

    public WeaponRecoil GetWeaponRecoil()
    {
        return weaponRecoil;
    }

    public WeaponSway GetWeaponSway()
    {
        return weaponSway;
    }

    public WeaponReload GetWeaponReload()
    {
        return weaponReload;
    }

    public CameraShake GetCameraShake()
    {
        return cameraShake;
    }

    public WeaponAim GetWeaponAim()
    {
        return weaponAim;
    }

    public void OnDrawGizmos()
    {
        // Dibuja una esfera en la posición inicial del objeto para visualizarlo en el editor
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(InitialTransform.position, 0.1f);
    }
}