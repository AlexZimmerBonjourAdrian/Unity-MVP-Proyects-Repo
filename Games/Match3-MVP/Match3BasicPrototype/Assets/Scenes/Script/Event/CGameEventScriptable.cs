using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Event;
using System;

[CreateAssetMenu(fileName = "NewEventData", menuName = "PointToClickEngine/Event")]
 [Serializable]
public class CGameEventScriptable : ScriptableObject
{
    private readonly List<CGameEventListener> _listeners = new List<CGameEventListener>();
    //private readonly List<>
    // Start is called before the first frame update

    public void Raize()
    {
        for (int i = 0; i < _listeners.Count; i++)
        {
            _listeners[i].OnEventRaise();
        }
    }

    public void RegisterLister(CGameEventListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void UnreagisterLister(CGameEventListener listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }

}
