using UnityEngine;
using HorrorEngine.Interfaces;
using HorrorEngine.Manager;
using HorrorEngine.Events;

public class RevolverPickup : MonoBehaviour, Iinteract
{
    public void Oninteract()
    {
        Debug.Log("Player picked up the Revolver!");
        // Notifica al sistema que el revólver fue recogido
         CManagerSFX.Inst.PlaySound(15); 
        CGameEvents.OnPickupRevolver.Publish();
        // Opcional: Destruir el objeto, añadir al inventario, etc.
        Destroy(gameObject);
    }



    
}
