using UnityEngine;
using Yarn.Unity;

namespace HorrorEngine
{
    /// <summary>
    /// Funciones Yarn para interactuar con el sistema de tareas desde scripts Yarn.
    /// Estas funciones pueden ser llamadas desde los scripts Yarn usando la sintaxis: $functionName()
    /// Yarn Spinner detecta automáticamente los métodos marcados con [YarnFunction]
    /// </summary>
    public static class YarnTaskFunctions
    {
        /// <summary>
        /// Verifica si una tarea está activa actualmente
        /// Uso en Yarn: $IsTaskActive("task_id")
        /// </summary>
        [YarnFunction("IsTaskActive")]
        public static bool IsTaskActive(string taskID)
        {
            if (TaskManager.Instance == null) return false;
            var task = TaskManager.Instance.GetTask(taskID);
            return task != null && task.IsActive;
        }

        /// <summary>
        /// Verifica si una tarea está completada
        /// Uso en Yarn: $IsTaskCompleted("task_id")
        /// </summary>
        [YarnFunction("IsTaskCompleted")]
        public static bool IsTaskCompleted(string taskID)
        {
            if (TaskManager.Instance == null) return false;
            var task = TaskManager.Instance.GetTask(taskID);
            return task != null && task.IsCompleted;
        }

        /// <summary>
        /// Verifica si una tarea está bloqueada
        /// Uso en Yarn: $IsTaskLocked("task_id")
        /// </summary>
        [YarnFunction("IsTaskLocked")]
        public static bool IsTaskLocked(string taskID)
        {
            if (TaskManager.Instance == null) return true;
            var task = TaskManager.Instance.GetTask(taskID);
            return task == null || task.IsLocked;
        }

        /// <summary>
        /// Obtiene el progreso de una tarea (0.0 a 1.0)
        /// Uso en Yarn: $GetTaskProgress("task_id")
        /// </summary>
        [YarnFunction("GetTaskProgress")]
        public static float GetTaskProgress(string taskID)
        {
            if (TaskManager.Instance == null) return 0f;
            var task = TaskManager.Instance.GetTask(taskID);
            return task != null ? task.Progress : 0f;
        }

        /// <summary>
        /// Obtiene el texto de progreso de una tarea
        /// Uso en Yarn: $GetTaskProgressText("task_id")
        /// </summary>
        [YarnFunction("GetTaskProgressText")]
        public static string GetTaskProgressText(string taskID)
        {
            if (TaskManager.Instance == null) return "0%";
            var task = TaskManager.Instance.GetTask(taskID);
            return task != null ? task.GetProgressText() : "0%";
        }
    }
}
