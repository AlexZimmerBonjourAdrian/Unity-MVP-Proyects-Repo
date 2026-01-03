using System;
using System.Collections.Generic;
using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// EventBus Pattern - Sistema centralizado de eventos para comunicación desacoplada.
    /// Permite publicar eventos y suscribirse a ellos sin dependencias directas entre componentes.
    /// </summary>
    public static class EventBus
    {
        // Diccionario que mapea tipos de eventos con sus handlers
        private static readonly Dictionary<Type, List<object>> eventHandlers = new Dictionary<Type, List<object>>();

        /// <summary>
        /// Suscribe un handler a un tipo específico de evento
        /// </summary>
        /// <typeparam name="T">Tipo del evento</typeparam>
        /// <param name="handler">Método que manejará el evento</param>
        public static void Subscribe<T>(Action<T> handler) where T : IEvent
        {
            var eventType = typeof(T);

            // Crear lista si no existe
            if (!eventHandlers.ContainsKey(eventType))
            {
                eventHandlers[eventType] = new List<object>();
            }

            // Agregar handler si no está ya suscrito
            if (!eventHandlers[eventType].Contains(handler))
            {
                eventHandlers[eventType].Add(handler);
                LogDebug($"Subscribed handler to event '{eventType.Name}'");
            }
            else
            {
                LogDebug($"Handler already subscribed to event '{eventType.Name}'");
            }
        }

        /// <summary>
        /// Desuscribe un handler de un tipo específico de evento
        /// </summary>
        /// <typeparam name="T">Tipo del evento</typeparam>
        /// <param name="handler">Método a desuscribir</param>
        public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
        {
            var eventType = typeof(T);

            if (eventHandlers.ContainsKey(eventType))
            {
                eventHandlers[eventType].Remove(handler);
                LogDebug($"Unsubscribed handler from event '{eventType.Name}'");

                // Limpiar diccionario si no quedan handlers
                if (eventHandlers[eventType].Count == 0)
                {
                    eventHandlers.Remove(eventType);
                    LogDebug($"Cleaned up empty handler list for event '{eventType.Name}'");
                }
            }
            else
            {
                LogDebug($"Event type '{eventType.Name}' not found in handlers");
            }
        }

        /// <summary>
        /// Publica un evento, notificando a todos los handlers suscritos
        /// </summary>
        /// <typeparam name="T">Tipo del evento</typeparam>
        /// <param name="eventData">Datos del evento</param>
        public static void Publish<T>(T eventData) where T : IEvent
        {
            var eventType = typeof(T);

            if (eventHandlers.ContainsKey(eventType))
            {
                LogDebug($"Publishing event '{eventType.Name}' to {eventHandlers[eventType].Count} handlers");

                // Crear copia de la lista para evitar modificaciones durante iteración
                var handlers = new List<object>(eventHandlers[eventType]);

                foreach (var handler in handlers)
                {
                    try
                    {
                        ((Action<T>)handler)?.Invoke(eventData);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error in event handler for '{eventType.Name}': {ex.Message}");

                        // Remover handler problemático para evitar errores futuros
                        eventHandlers[eventType].Remove(handler);
                        LogDebug($"Removed problematic handler for event '{eventType.Name}'");
                    }
                }
            }
            else
            {
                LogDebug($"No handlers registered for event '{eventType.Name}'");
            }
        }

        /// <summary>
        /// Limpia todos los handlers registrados (útil para cambios de escena)
        /// </summary>
        public static void Clear()
        {
            int totalHandlers = 0;
            foreach (var handlers in eventHandlers.Values)
            {
                totalHandlers += handlers.Count;
            }

            eventHandlers.Clear();
            LogDebug($"Cleared all event handlers. Total removed: {totalHandlers}");
        }

        /// <summary>
        /// Obtiene información de debug sobre el estado del EventBus
        /// </summary>
        /// <returns>String con información de debug</returns>
        public static string GetDebugInfo()
        {
            string info = $"EventBus Debug Info:\n";
            info += $"- Event types registered: {eventHandlers.Count}\n";

            foreach (var kvp in eventHandlers)
            {
                info += $"- {kvp.Key.Name}: {kvp.Value.Count} handlers\n";
            }

            return info;
        }

        /// <summary>
        /// Verifica si hay handlers registrados para un tipo de evento
        /// </summary>
        /// <typeparam name="T">Tipo del evento</typeparam>
        /// <returns>True si hay handlers registrados</returns>
        public static bool HasHandlers<T>() where T : IEvent
        {
            return eventHandlers.ContainsKey(typeof(T)) && eventHandlers[typeof(T)].Count > 0;
        }

        /// <summary>
        /// Obtiene el número de handlers para un tipo de evento
        /// </summary>
        /// <typeparam name="T">Tipo del evento</typeparam>
        /// <returns>Número de handlers registrados</returns>
        public static int GetHandlerCount<T>() where T : IEvent
        {
            return eventHandlers.ContainsKey(typeof(T)) ? eventHandlers[typeof(T)].Count : 0;
        }

        /// <summary>
        /// Logging interno (solo en modo debug)
        /// </summary>
        private static void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[EventBus] {message}");
#endif
        }
    }
}
