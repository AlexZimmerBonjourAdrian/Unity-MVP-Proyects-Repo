using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    public class CPuzzleManager : MonoBehaviour
{
    //Responsable de controlar todos los puzzles y de cada tipo 

    public static CPuzzleManager Inst
    {
        get
        {
            if (_inst == null)
            {
               
                GameObject obj = new GameObject("Music");
                return obj.AddComponent<CPuzzleManager>();
            }
           
            return _inst;

        }
    }
    private static CPuzzleManager _inst;

      public void Awake()
    {
    if(_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
     //  DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }
    }
}
