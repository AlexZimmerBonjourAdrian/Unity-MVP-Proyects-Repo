using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CNode : ScriptableObject
{
    public enum State
    {
        Running,
        Faiulure,
        Success
    }

    [HideInInspector]public State state = State.Running;
    [HideInInspector]public bool started = false;
    [HideInInspector]public string guid;
    [HideInInspector]public Vector2 position;
    public State Update() {
        if (!started) { 
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if(state == State.Faiulure || state == State.Success) {
            OnStop();
            started = false;
        }
        return state;
    }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();


}
