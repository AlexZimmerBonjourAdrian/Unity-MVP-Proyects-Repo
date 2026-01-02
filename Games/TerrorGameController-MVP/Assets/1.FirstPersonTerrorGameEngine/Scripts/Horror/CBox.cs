using UnityEngine;
using HorrorEngine.Interfaces;
using HorrorEngine.Events;
public class CBox : MonoBehaviour, Iinteract
{
    public void Oninteract()
    {
        Debug.Log("Interact with Box");
        UnlockDoor();
        DestoryBox();
    }

    public void UnlockDoor()
    {
        CGameEvents.OnUnlockDoor.Publish();
        Debug.Log("Unlock Door with Box");
    }
    private void DestoryBox()
    {
        Destroy(gameObject);
    }

   
}
