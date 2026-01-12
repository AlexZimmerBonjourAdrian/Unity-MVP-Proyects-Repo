using UnityEngine;

namespace RetroFPS
{
    public enum WeaponType
    {
        Pistol,
        Rifle,
        Shotgun,
        Sniper,
        SMG,
        Melee
    }

    public enum WeaponFireMode
    {
        Single,    // Disparo único
        Burst,     // Ráfaga
        Auto,      // Automático
        Charge     // Carga
    }
}
