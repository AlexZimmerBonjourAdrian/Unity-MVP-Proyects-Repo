using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// ScriptableObject genérico para configurar tareas.
    /// Permite a los diseñadores crear y configurar tareas desde el editor de Unity.
    /// </summary>
    [CreateAssetMenu(fileName = "NewTask", menuName = "HorrorEngine/Tasks/Task Data", order = 1)]
    public class TaskDataSO : ScriptableObject
    {
        [Header("Identificación")]
        [Tooltip("ID único de la tarea (debe ser único en todo el juego)")]
        public string taskID = "";

        [Tooltip("Nombre de la tarea que se mostrará al jugador")]
        public string taskName = "";

        [TextArea(3, 5)]
        [Tooltip("Descripción de la tarea")]
        public string description = "";

        [Header("Progreso")]
        [Tooltip("Tipo de progreso de la tarea")]
        public TaskProgressType progressType = TaskProgressType.Boolean;

        [Tooltip("Valor máximo para tareas de tipo Counter")]
        public float maxProgressValue = 1f;

        [Tooltip("Valor mínimo para tareas de tipo Counter")]
        public float minProgressValue = 0f;

        [Header("Condiciones de Activación")]
        [Tooltip("Grupo de condiciones que deben cumplirse para activar la tarea")]
        public TaskConditionGroup activationConditions = new TaskConditionGroup();

        [Header("Dependencias")]
        [Tooltip("IDs de tareas que deben completarse antes de que esta pueda activarse")]
        public List<string> requiredTaskIDs = new List<string>();

        [Header("Subtareas")]
        [Tooltip("IDs de subtareas relacionadas con esta tarea")]
        public List<string> subtaskIDs = new List<string>();

        [Header("Eventos")]
        [Tooltip("Flag que se activará cuando la tarea se complete")]
        public string completionFlag = "";

        [Tooltip("ID de evento que se disparará cuando la tarea se complete")]
        public string completionEventID = "";

        [Header("Configuración Avanzada")]
        [Tooltip("Si es true, la tarea se activará automáticamente cuando se cumplan las condiciones")]
        public bool autoActivate = false;

        [Tooltip("Si es true, la tarea se completará automáticamente cuando alcance el progreso máximo")]
        public bool autoComplete = true;

        /// <summary>
        /// Valida que la configuración de la tarea sea correcta
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(taskID))
            {
                Debug.LogWarning($"TaskDataSO '{name}' tiene un taskID vacío");
                return false;
            }

            if (string.IsNullOrEmpty(taskName))
            {
                Debug.LogWarning($"TaskDataSO '{name}' tiene un taskName vacío");
                return false;
            }

            if (maxProgressValue < minProgressValue)
            {
                Debug.LogWarning($"TaskDataSO '{name}' tiene maxProgressValue menor que minProgressValue");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Crea una instancia de TaskProgress basada en la configuración
        /// </summary>
        public TaskProgress CreateTaskProgress()
        {
            return new TaskProgress(progressType, maxProgressValue, minProgressValue);
        }
    }
}
