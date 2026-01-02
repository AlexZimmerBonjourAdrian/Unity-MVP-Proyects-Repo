using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CInteractObjects : MonoBehaviour
{

    public enum InteracteObject
    {
        None,
        Hove,
        Iinteract,
        Not 
    }

    InteracteObject InteracteState = InteracteObject.None;
    GameObject anyObject;

    private Component _actionObj;

    private GameObject RayCollision()
    {

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitinfo = Physics2D.Raycast(worldPoint, Vector2.zero);
        if (hitinfo.collider != null)
        {
            
             (_actionObj as Iinteract).Oninteract();
            //Debug.Log(hitinfo.collider.gameObject.name);
            anyObject = hitinfo.collider.gameObject;
            return anyObject;
        }

        return null;
    }

    public void SetState(InteracteObject state)
   {
     InteracteState = state;
   }

    public InteracteObject GetState()
   {
        return InteracteState;
   }
   

   void Update()
   {
    IntertiveObject();
   }
    
    private void IntertiveObject()
    {
        Debug.Log("State: " + (int)InteracteState + " " + InteracteState);
        switch((int)InteracteState)
        {
            case (int)InteracteObject.None:
            GameObject obj = RayCollision();
                if (obj == null)
                    return;

                    Component actionObj = obj.GetComponent(typeof(Iinteract));
                if (actionObj != null)
                {
                    _actionObj = actionObj;
                    SetState(InteracteObject.Hove);
                }
            break;
        
            case (int)InteracteObject.Iinteract:
                obj = RayCollision();
                if (obj == null)
                {
                    _actionObj = null;
                    SetState(InteracteObject.None);
                    return;
                }

                actionObj = obj.GetComponent(typeof(Iinteract));
                if (actionObj == null)
                {
                    _actionObj = null;
                    SetState(InteracteObject.None);
                }
                else if (actionObj != _actionObj)
                {
                    _actionObj = actionObj;
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    SetState(InteracteObject.Iinteract);
                }
            break;

            case (int)InteracteObject.Hove:
            (_actionObj as Iinteract).Oninteract();
                SetState(InteracteObject.None);
                _actionObj = null;
            break;
            }
        
    }
    
    
    void OnDrawGizmos()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldPoint, .3f);

    }
}
