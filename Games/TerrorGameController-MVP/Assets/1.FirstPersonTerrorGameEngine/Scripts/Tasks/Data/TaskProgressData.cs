using System;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Datos serializables para el progreso de una tarea.
    /// Se usa para guardar y cargar el estado de las tareas.
    /// </summary>
    [System.Serializable]
    public class TaskProgressData
    {
        public string taskID = "";
        public TaskState state = TaskState.Locked;
        public float progressValue = 0f;
        public bool isComplete = false;
    }

    /// <summary>
    /// Datos serializables para todas las tareas del juego.
    /// Se guarda en GameData para persistencia.
    /// </summary>
    [System.Serializable]
    public class TasksData
    {
        public List<TaskProgressData> tasks = new List<TaskProgressData>();

        public TasksData()
        {
            tasks = new List<TaskProgressData>();
        }

        /// <summary>
        /// Obtiene los datos de progreso de una tarea específica
        /// </summary>
        public TaskProgressData GetTaskData(string taskID)
        {
            return tasks.Find(t => t.taskID == taskID);
        }

        /// <summary>
        /// Establece o actualiza los datos de progreso de una tarea
        /// </summary>
        public void SetTaskData(TaskProgressData data)
        {
            var existing = GetTaskData(data.taskID);
            if (existing != null)
            {
                existing.state = data.state;
                existing.progressValue = data.progressValue;
                existing.isComplete = data.isComplete;
            }
            else
            {
                tasks.Add(data);
            }
        }

        /// <summary>
        /// Elimina los datos de una tarea
        /// </summary>
        public void RemoveTaskData(string taskID)
        {
            tasks.RemoveAll(t => t.taskID == taskID);
        }

        /// <summary>
        /// Limpia todos los datos de tareas
        /// </summary>
        public void Clear()
        {
            tasks.Clear();
        }
    }
}
