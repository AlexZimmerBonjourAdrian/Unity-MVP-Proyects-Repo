using System;
using System.Collections.Generic;
using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Observer Pattern - Clase genérica que permite observar cambios en valores.
    /// Los observers se notifican automáticamente cuando el valor observado cambia.
    /// </summary>
    /// <typeparam name="T">Tipo de dato a observar</typeparam>
    public class GameObserver<T>
    {
        // Lista de observers suscritos
        private readonly List<Action<T>> observers = new List<Action<T>>();

        // Valor actual observado
        private T currentValue;

        /// <summary>
        /// Constructor opcional con valor inicial
        /// </summary>
        public GameObserver(T initialValue = default)
        {
            currentValue = initialValue;
        }

        /// <summary>
        /// Suscribe un observer al cambio de valor
        /// </summary>
        /// <param name="observer">Método a llamar cuando cambia el valor</param>
        public void Attach(Action<T> observer)
        {
            if (observer != null && !observers.Contains(observer))
            {
                observers.Add(observer);
                LogDebug($"Attached observer. Total observers: {observers.Count}");

                // Notificar inmediatamente con el valor actual (para inicialización)
                try
                {
                    observer.Invoke(currentValue);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error in observer initialization: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Desuscribe un observer
        /// </summary>
        /// <param name="observer">Método a remover de la lista de observers</param>
        public void Detach(Action<T> observer)
        {
            if (observers.Remove(observer))
            {
                LogDebug($"Detached observer. Remaining observers: {observers.Count}");
            }
        }

        /// <summary>
        /// Notifica a todos los observers con el nuevo valor
        /// </summary>
        /// <param name="newValue">Nuevo valor a notificar</param>
        public void Notify(T newValue)
        {
            currentValue = newValue;

            if (observers.Count == 0)
            {
                LogDebug($"No observers to notify for value change: {newValue}");
                return;
            }

            LogDebug($"Notifying {observers.Count} observers of value change: {newValue}");

            // Crear copia de la lista para evitar modificaciones durante iteración
            var observersCopy = new List<Action<T>>(observers);

            foreach (var observer in observersCopy)
            {
                if (observer != null)
                {
                    try
                    {
                        observer.Invoke(newValue);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error in observer notification: {ex.Message}");

                        // Remover observer problemático
                        observers.Remove(observer);
                        LogDebug("Removed problematic observer");
                    }
                }
                else
                {
                    // Remover observers nulos
                    observers.Remove(observer);
                    LogDebug("Removed null observer");
                }
            }
        }

        /// <summary>
        /// Obtiene el valor actual observado
        /// </summary>
        public T GetValue()
        {
            return currentValue;
        }

        /// <summary>
        /// Establece un nuevo valor y notifica a los observers
        /// </summary>
        public void SetValue(T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                Notify(newValue);
            }
        }

        /// <summary>
        /// Modifica el valor usando una función y notifica el cambio
        /// </summary>
        public void ModifyValue(Func<T, T> modifier)
        {
            T newValue = modifier(currentValue);
            SetValue(newValue);
        }

        /// <summary>
        /// Limpia todos los observers (útil para cleanup)
        /// </summary>
        public void Clear()
        {
            int observerCount = observers.Count;
            observers.Clear();
            LogDebug($"Cleared {observerCount} observers");
        }

        /// <summary>
        /// Obtiene el número de observers suscritos
        /// </summary>
        public int Count => observers.Count;

        /// <summary>
        /// Verifica si hay observers suscritos
        /// </summary>
        public bool HasObservers => observers.Count > 0;

        /// <summary>
        /// Obtiene información de debug
        /// </summary>
        public string GetDebugInfo()
        {
            return $"GameObserver<{typeof(T).Name}> Debug Info:\n" +
                   $"- Current Value: {currentValue}\n" +
                   $"- Observer Count: {observers.Count}\n" +
                   $"- Has Observers: {HasObservers}";
        }

        private void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[GameObserver<{typeof(T).Name}>] {message}");
#endif
        }
    }

    /// <summary>
    /// Extensión para facilitar el uso de GameObserver con tipos comunes
    /// </summary>
    public static class GameObserverExtensions
    {
        /// <summary>
        /// Crea un observer que se actualiza automáticamente cuando cambia el valor
        /// </summary>
        public static GameObserver<T> CreateObserver<T>(T initialValue = default)
        {
            return new GameObserver<T>(initialValue);
        }

        /// <summary>
        /// Suscribe un observer con lambda simplificada
        /// </summary>
        public static void Subscribe<T>(this GameObserver<T> observer, Action<T> callback)
        {
            observer.Attach(callback);
        }

        /// <summary>
        /// Desuscribe un observer con lambda simplificada
        /// </summary>
        public static void Unsubscribe<T>(this GameObserver<T> observer, Action<T> callback)
        {
            observer.Detach(callback);
        }

        /// <summary>
        /// Notifica un cambio de valor de forma simplificada
        /// </summary>
        public static void UpdateValue<T>(this GameObserver<T> observer, T newValue)
        {
            observer.SetValue(newValue);
        }
    }
}
