using System;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Tipo de progreso que puede tener una tarea
    /// </summary>
    public enum TaskProgressType
    {
        Boolean,    // Tarea completada o no (0 o 1)
        Counter,    // Contador incremental (0 a maxValue)
        Percentage  // Porcentaje (0.0 a 1.0)
    }

    /// <summary>
    /// Sistema de progreso para tareas.
    /// Maneja diferentes tipos de progreso y su actualización.
    /// </summary>
    [System.Serializable]
    public class TaskProgress
    {
        [Header("Configuración")]
        [SerializeField] private TaskProgressType progressType = TaskProgressType.Boolean;
        [SerializeField] private float currentValue = 0f;
        [SerializeField] private float maxValue = 1f;
        [SerializeField] private float minValue = 0f;

        [Header("Estado")]
        [SerializeField] private bool isComplete = false;

        public TaskProgressType ProgressType => progressType;
        public float CurrentValue => currentValue;
        public float MaxValue => maxValue;
        public float MinValue => minValue;
        public bool IsComplete => isComplete;

        /// <summary>
        /// Progreso normalizado (0.0 a 1.0)
        /// </summary>
        public float NormalizedProgress
        {
            get
            {
                if (maxValue == minValue) return 0f;
                return Mathf.Clamp01((currentValue - minValue) / (maxValue - minValue));
            }
        }

        public TaskProgress()
        {
            progressType = TaskProgressType.Boolean;
            currentValue = 0f;
            maxValue = 1f;
            minValue = 0f;
            isComplete = false;
        }

        public TaskProgress(TaskProgressType type, float max = 1f, float min = 0f)
        {
            progressType = type;
            currentValue = min;
            maxValue = max;
            minValue = min;
            isComplete = false;
        }

        /// <summary>
        /// Actualiza el progreso de la tarea
        /// </summary>
        /// <param name="amount">Cantidad a agregar (o establecer según el tipo)</param>
        public void UpdateProgress(float amount)
        {
            switch (progressType)
            {
                case TaskProgressType.Boolean:
                    currentValue = amount > 0 ? maxValue : minValue;
                    break;

                case TaskProgressType.Counter:
                    currentValue = Mathf.Clamp(currentValue + amount, minValue, maxValue);
                    break;

                case TaskProgressType.Percentage:
                    currentValue = Mathf.Clamp01(amount);
                    break;
            }

            CheckCompletion();
        }

        /// <summary>
        /// Establece el progreso a un valor específico
        /// </summary>
        /// <param name="value">Valor a establecer</param>
        public void SetProgress(float value)
        {
            switch (progressType)
            {
                case TaskProgressType.Boolean:
                    currentValue = value > 0 ? maxValue : minValue;
                    break;

                case TaskProgressType.Counter:
                case TaskProgressType.Percentage:
                    currentValue = Mathf.Clamp(value, minValue, maxValue);
                    break;
            }

            CheckCompletion();
        }

        /// <summary>
        /// Verifica si la tarea está completada
        /// </summary>
        private void CheckCompletion()
        {
            switch (progressType)
            {
                case TaskProgressType.Boolean:
                    isComplete = currentValue >= maxValue;
                    break;

                case TaskProgressType.Counter:
                    isComplete = currentValue >= maxValue;
                    break;

                case TaskProgressType.Percentage:
                    isComplete = currentValue >= 1.0f;
                    break;
            }
        }

        /// <summary>
        /// Obtiene el texto de progreso formateado
        /// </summary>
        /// <returns>String con el progreso actual</returns>
        public string GetProgressText()
        {
            switch (progressType)
            {
                case TaskProgressType.Boolean:
                    return isComplete ? "Completado" : "No completado";

                case TaskProgressType.Counter:
                    return $"{currentValue:F0} / {maxValue:F0}";

                case TaskProgressType.Percentage:
                    return $"{NormalizedProgress * 100:F0}%";

                default:
                    return "Desconocido";
            }
        }

        /// <summary>
        /// Reinicia el progreso a su valor inicial
        /// </summary>
        public void Reset()
        {
            currentValue = minValue;
            isComplete = false;
        }
    }
}
