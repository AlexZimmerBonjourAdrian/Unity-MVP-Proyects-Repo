using UnityEngine;


namespace Draconiano.Items
{
    public class CPickUp : MonoBehaviour//, ICollect
    {
        // Start is called before the first frame update
        //public void OnCollection()
        //{

        //}
        [SerializeField]
        public GameObject WeponShootPrefab;

        public CWeaponManager ManagerWeapon;

        public void Start()
        {

            ManagerWeapon = FindObjectOfType<CWeaponManager>();
            
        }
        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (ManagerWeapon != null)
            {
                if (collision.gameObject.tag == "Player")
                {
                    Debug.Log("Entra en la funcion de collider");
                   
                    if (ManagerWeapon.GetListWeapon().Count <= 1)
                    {
                        ManagerWeapon.AddWeapon(WeponShootPrefab);
                        Destroy(this.gameObject);
                    }

                }
            }
        }
    }

}