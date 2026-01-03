using System;
using System.Collections.Generic;

namespace RolEngine
{
    /// <summary>
    /// Observers para el sistema de rol.
    /// Permite que otros sistemas se suscriban a cambios de estadísticas.
    /// </summary>
    public static class StatObservers
    {
        /// <summary>
        /// Observer para cambios en Sanity
        /// </summary>
        public static readonly StatObserver<int> SanityChanged = new StatObserver<int>(5);

        /// <summary>
        /// Observer para cambios en Charm
        /// </summary>
        public static readonly StatObserver<int> CharmChanged = new StatObserver<int>(5);

        /// <summary>
        /// Observer para cambios en Wits
        /// </summary>
        public static readonly StatObserver<int> WitsChanged = new StatObserver<int>(5);

        /// <summary>
        /// Observer para cambios en Composure
        /// </summary>
        public static readonly StatObserver<int> ComposureChanged = new StatObserver<int>(5);

        /// <summary>
        /// Observer para cambios en Empathy
        /// </summary>
        public static readonly StatObserver<int> EmpathyChanged = new StatObserver<int>(5);

        /// <summary>
        /// Observer para cambios de template
        /// </summary>
        public static readonly StatObserver<string> TemplateChanged = new StatObserver<string>("");

        /// <summary>
        /// Limpia todos los observers
        /// </summary>
        public static void ClearAll()
        {
            SanityChanged.Clear();
            CharmChanged.Clear();
            WitsChanged.Clear();
            ComposureChanged.Clear();
            EmpathyChanged.Clear();
            TemplateChanged.Clear();
        }
    }

    /// <summary>
    /// Observer genérico para estadísticas
    /// </summary>
    public class StatObserver<T>
    {
        private T currentValue;
        private List<Action<T>> observers = new List<Action<T>>();

        public StatObserver(T initialValue)
        {
            currentValue = initialValue;
        }

        /// <summary>
        /// Obtiene el valor actual
        /// </summary>
        public T GetValue() => currentValue;

        /// <summary>
        /// Establece un nuevo valor y notifica a los observers
        /// </summary>
        public void SetValue(T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                T oldValue = currentValue;
                currentValue = newValue;
                Notify(newValue);
            }
        }

        /// <summary>
        /// Suscribe un observer
        /// </summary>
        public void Attach(Action<T> observer)
        {
            if (observer != null && !observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        /// <summary>
        /// Desuscribe un observer
        /// </summary>
        public void Detach(Action<T> observer)
        {
            if (observer != null)
            {
                observers.Remove(observer);
            }
        }

        /// <summary>
        /// Notifica a todos los observers
        /// </summary>
        private void Notify(T value)
        {
            foreach (var observer in observers)
            {
                observer?.Invoke(value);
            }
        }

        /// <summary>
        /// Limpia todos los observers
        /// </summary>
        public void Clear()
        {
            observers.Clear();
        }
    }
}

