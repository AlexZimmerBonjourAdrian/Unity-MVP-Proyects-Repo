using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Event
{
    public class CGameEventListener : MonoBehaviour
  {
        

        [SerializeField]
        private CGameEventScriptable _event;

        [SerializeField]
        private UnityEvent _respose;

        private void OnEnable() => _event?.RegisterLister(this);

        private void OnDisable() => _event?.UnreagisterLister(this);

        public void OnEventRaise() => _respose?.Invoke();

    }
}
