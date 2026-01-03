using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;


namespace RetroFPS
{
    /// <summary>
    /// Sistema de eventos existente - MANTENIDO POR COMPATIBILIDAD.
    /// Esta clase se mantiene para compatibilidad con código existente.
    /// Para nuevos desarrollos, se recomienda usar RetroFPS.EventBus.
    ///
    /// INTEGRACIÓN: Los eventos publicados aquí también se propagan al EventBus
    /// si existe una conversión automática disponible.
    /// </summary>
    public class CGameEvent<T>
    {
        private readonly List<Action<T>> listeners = new List<Action<T>>();
        private static bool eventBusAvailable = false;
        private static Type eventBusType = null;

        static CGameEvent()
        {
            // Verificar si EventBus está disponible
            try
            {
                eventBusType = Type.GetType("RetroFPS.EventBus, Assembly-CSharp");
                eventBusAvailable = eventBusType != null;
                if (eventBusAvailable)
                {
                    Debug.Log("[CGameEvent] EventBus integration available");
                }
            }
            catch
            {
                eventBusAvailable = false;
                Debug.Log("[CGameEvent] EventBus integration not available - using legacy system only");
            }
        }

        public void Subscribe(Action<T> listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[CGameEvent<{typeof(T).Name}>] Listener subscribed. Total: {listeners.Count}");
#endif
            }
        }

        public void Unsubscribe(Action<T> listener)
        {
            if (listeners.Remove(listener))
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.Log($"[CGameEvent<{typeof(T).Name}>] Listener unsubscribed. Remaining: {listeners.Count}");
#endif
            }
        }

        public void Publish(T eventData)
        {
            // Notificar listeners del sistema legacy
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                try
                {
                    listeners[i]?.Invoke(eventData);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error in legacy event listener: {ex.Message}");
                    // Remover listener problemático
                    listeners.RemoveAt(i);
                }
            }

            // Intentar propagar al EventBus si está disponible
            TryPropagateToEventBus(eventData);
        }

        /// <summary>
        /// Intenta propagar el evento al nuevo EventBus
        /// </summary>
        private void TryPropagateToEventBus(T eventData)
        {
            if (!eventBusAvailable || eventBusType == null) return;

            try
            {
                // Verificar si el tipo T implementa IEvent
                if (typeof(RetroFPS.IEvent).IsAssignableFrom(typeof(T)))
                {
                    // Convertir y publicar en EventBus
                    var publishMethod = eventBusType.GetMethod("Publish", new[] { typeof(T) });
                    if (publishMethod != null)
                    {
                        publishMethod.Invoke(null, new object[] { eventData });
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                        Debug.Log($"[CGameEvent] Event propagated to EventBus: {typeof(T).Name}");
#endif
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[CGameEvent] Failed to propagate to EventBus: {ex.Message}");
                // Deshabilitar integración si falla persistentemente
                eventBusAvailable = false;
            }
        }

        /// <summary>
        /// Obtiene el número de listeners suscritos
        /// </summary>
        public int ListenerCount => listeners.Count;

        /// <summary>
        /// Limpia todos los listeners (útil para limpieza)
        /// </summary>
        public void Clear()
        {
            int count = listeners.Count;
            listeners.Clear();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[CGameEvent<{typeof(T).Name}>] Cleared {count} listeners");
#endif
        }
    }
}