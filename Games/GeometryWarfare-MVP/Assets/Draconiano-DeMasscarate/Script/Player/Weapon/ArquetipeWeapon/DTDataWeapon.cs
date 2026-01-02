using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DTDataWeapon : ScriptableObject
{
    [Range(1, 2000)]
    public int DT_Ammo_Mag_Start;

    [Range(1, 1500)]
    public int DT_Ammo_Max_Reserv;
    [Range(0.01f, 20.0f)]
    public float DT_Fire_Rate;
    
    [Range(1, 1500)]
    public int DT_Damage;

    [Range(0.01f, 1.0f)]
    public float DT_Critical_Prob;

    public Sprite DT_Image_Weapon;

    [SerializeField]
    public enum typeShoot
    {
        None = 0,
        Single = 1,
        Rafaga = 2,
        Auto = 3
    };
    [SerializeField]
    public typeShoot DT_Shoot;
}
