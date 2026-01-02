using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Dynamic;
using UnityEditor;
public class Principal : MonoBehaviour
{

    public static Principal Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("Principal");
                return obj.AddComponent<Principal>();
            }

            return _inst;
        }
    }

    private static Principal _inst;

    public int ID;


    void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        _inst = this;

        if(PlayerPrefs.HasKey("ID"))
        {
            ID = PlayerPrefs.GetInt("ID");
        }

    }

    public void Save()
    {
        PlayerPrefs.SetInt("ID", ID);
    }

    public int GetId()
    {
        return ID;
    }

    public void SetId(int id)
    {
        ID = id;
    }

    public void InteractObjects()
    {
       
    }

    
}
