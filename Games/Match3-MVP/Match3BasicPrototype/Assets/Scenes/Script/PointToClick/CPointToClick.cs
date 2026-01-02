using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPointToClick : MonoBehaviour
{
    private static int ACTIONSTATE_NONE = 0;
    private static int ACTIONSTATE_HOVE = 1;
    private static int ACTIONSTATE_INTERACT = 2;
    GameObject anyObject;
    private int _actionState;
    private Component _actionObj;

    public static CPointToClick Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("PointMechanic");

                return obj.AddComponent<CPointToClick>();
            }
            return _inst;
        }
    }
    private static CPointToClick _inst;



    public void Awake()
    {
       
        if (_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }
    void Update()
    {
        InteractionPointToClick();
    }

    public void InteractionPointToClick()
    {
        if (_actionState == ACTIONSTATE_NONE)
        {
            GameObject obj = RayCollision();
            if (obj == null)
                return;

            Component actionObj = obj.GetComponent(typeof(Iinteract));
            if (actionObj != null)
            {
                _actionObj = actionObj;
                _actionState = ACTIONSTATE_HOVE;
            }
        }
        else if (_actionState == ACTIONSTATE_HOVE)
        {
            GameObject obj = RayCollision();
            if (obj == null)
            {
                _actionObj = null;
                _actionState = ACTIONSTATE_NONE;
                return;
            }

            Component actionObj = obj.GetComponent(typeof(Iinteract));
            if (actionObj == null)
            {
                _actionObj = null;
                _actionState = ACTIONSTATE_NONE;
            }
            else if (actionObj != _actionObj)
            {
                _actionObj = actionObj;
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                _actionState = ACTIONSTATE_INTERACT;
            }
        }
        else if (_actionState == ACTIONSTATE_INTERACT)
        {
            (_actionObj as Iinteract).Oninteract();
            _actionState = ACTIONSTATE_NONE;
            _actionObj = null;
        }

    }

    

    private GameObject RayCollision()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitinfo = Physics2D.Raycast(worldPoint, Vector2.zero);
        

        if (hitinfo.collider != null)
        {
              Debug.Log("Entra");
            // anyObject = Collider2D.;
            //(_actionObj as Iinteract).Oninteract();
            //Debug.Log(hitinfo.collider.gameObject.name);
            anyObject = hitinfo.collider.gameObject;
            return anyObject;
        }

        return null;
    }

    void OnDrawGizmos()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldPoint, .3f);

    }

}
