using Draconiano_PC;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWeaponManager : MonoBehaviour
{
    public GameObject First_Weapon;

    //private SpriteRenderer Sprite;
    //public List<DTDataWeapon> _List_WeaponComplete;
    //public List<DTDataWeapon> _ListWeapon_Have;

    //public List<GameObject> _ListWeapon_Complete;
    public List<GameObject> _ListWeapon_Have;
    //[SerializeField] private List<GameObject> weapons = new List<GameObject>();
    private int selectedWeapon = 0;
    private GameObject CurrentWeapon;
    [SerializeField] private Transform[] _tranformSlot = new Transform[2];
    private GameObject _previousWeapon;
    // Start is called before the first frame update
    void Start()
    {
        //Sprite = Weapon.GetComponent<SpriteRenderer>();
        //Sprite.sprite = _ListWeapon_Have[0].DT_Image_Weapon;
        
    }

    public void Update()
    {
      // changeWeapon();
    }

    //public List<DTDataWeapon> GetListWeaponComplete()
    //{
    //    return _List_WeaponComplete;
    //}
    //public List<DTDataWeapon> GetListWeaponHave()
    //{
    //    return _ListWeapon_Have;
    //}
    public List<GameObject> GetListWeapon()
    {
        return _ListWeapon_Have;
    }
    public void AddWeapon(GameObject weapon)
    {
        _ListWeapon_Have.Add(weapon);
        // Active_Weapon = _ListWeapon_Have[1];
        int previousSelectedWeapon = selectedWeapon;

    
         selectedWeapon++;

         if (selectedWeapon >= _ListWeapon_Have.Count - 1)
         {
           selectedWeapon = 0;
         }

         if ((previousSelectedWeapon != selectedWeapon))
         {
           //CurrentWeapon.SetActive(false);
           //CurrentWeapon = _ListWeapon_Have[selectedWeapon];
           //CurrentWeapon.SetActive(true);

         }

    }
    public GameObject GetCurrentWeapon()
    {
        return CurrentWeapon;
    }
    private void SelectedWeapon()
    {
        int i = 0;
       
        //foreach (Transform weapon in _tranformSlot)
        //{
           
        //    GameObject weapon_ = _ListWeapon_Have[i].fw;
        //    if (weapon.gameObject == )
        //    {
              
        //            weapon.gameObject.SetActive(true);
        //            CurrentWeapon = weapon.gameObject;
               
        //           // CurrentWeapon = _previousWeapon;
        //            SelectedWeapon();
                
        //       // weapon.SetActive(true);
        //    }
        //    else
        //    {
        //        weapon.gameObject.SetActive(false);
        //        // weapon.SetActive(false);
        //    }
        
        //}
    
    }

       
    
    //public void AddWeaponAutomatic(GameObject weapon)
    //{
    //    _ListWeapon_Have.Add(weapon);
    //    // Active_Weapon = _ListWeapon_Have[1];
    //    int previousSelectedWeapon = selectedWeapon;

    //    if (CurrentWeapon != null)
    //    {
    //        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
    //        {
    //            if (selectedWeapon >= weapons.Count - 1)
    //            {
    //                selectedWeapon = 0;
    //            }
    //            else
    //            {
    //                selectedWeapon++;
    //            }
    //        }
    //        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
    //        {
    //            if (selectedWeapon <= 0)
    //            {
    //                selectedWeapon = weapons.Count - 1;
    //            }
    //            else
    //            {
    //                selectedWeapon--;
    //            }
    //        }
    //        if ((previousSelectedWeapon != selectedWeapon))
    //        {
    //            CurrentWeapon.SetActive(false);
    //            CurrentWeapon = weapons[selectedWeapon];
    //            CurrentWeapon.SetActive(true);
    //        }

    //    }
    //}


    //public List<GameObject> GetListWeaponComplete()
    //{
    //    return _ListWeapon_Complete;
    //}
    public List<GameObject> GetListWeaponHave()
    {
        return _ListWeapon_Have;
    }
    // Update is called once per frame
    private void changeWeapon()
    {

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    Sprite.sprite = _ListWeapon_Have[0].DT_Image_Weapon;
        //}

        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Sprite.sprite = _ListWeapon_Have[1].DT_Image_Weapon;
        //}
    }
}
