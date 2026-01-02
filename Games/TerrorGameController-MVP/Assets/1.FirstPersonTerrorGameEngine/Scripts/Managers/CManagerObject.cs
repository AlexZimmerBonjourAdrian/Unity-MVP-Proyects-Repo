using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CManagerObject : MonoBehaviour
{
    //Por ahora, hay que ignorarlo
   //En cada nivel debe guardar todos lso objetos en el nivel en el que este Que tengan un CObjectInventorie Como padre

     public static CManagerObject Inst
    {
        get
        {
            if (_inst == null)
            {
                Debug.Log("Entra?");
                GameObject obj = new GameObject("Music");
                return obj.AddComponent<CManagerObject>();
            }
            Debug.Log("Entra en el return");
            return _inst;

        }
    }
    private static CManagerObject _inst;

  public void Awake()
    {
    if(_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
       // DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }
}
