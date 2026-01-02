using Draconiano_PC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DTDataWeapon;


namespace Draconiano_PC
{
    [SerializeField]
    public class CWeaponLogic : CShootSystem
{
        
    public DTDataWeapon DataWeapon;


    public int DT_Ammo_Mag_Start;


    public int DT_Ammo_Max_Reserv;
   
    public float DT_Fire_Rate;


    public int DT_Damage;


    public float DT_Critical_Prob;

    public Sprite DT_Image_Weapon;

    public typeShoot DT_Shoot;



    public void Start()
    {
        DT_Ammo_Mag_Start = DataWeapon.DT_Ammo_Mag_Start;
        DT_Ammo_Mag_Start = DataWeapon.DT_Ammo_Mag_Start;
        DT_Fire_Rate = DataWeapon.DT_Fire_Rate;
        DT_Damage = DataWeapon.DT_Damage;
        DT_Critical_Prob = DataWeapon.DT_Critical_Prob;
        DT_Image_Weapon = DataWeapon.DT_Image_Weapon;
        DT_Shoot = DataWeapon.DT_Shoot;
    }

    protected override void Fire()
    {

       base.Fire();
      //  if()

    }
}
}
