using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Interfaz base para todas las tareas del sistema.
    /// Define el contrato que deben cumplir todas las implementaciones de tareas.
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// ID único de la tarea
        /// </summary>
        string TaskID { get; }

        /// <summary>
        /// Nombre de la tarea
        /// </summary>
        string TaskName { get; }

        /// <summary>
        /// Descripción de la tarea
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Indica si la tarea está activa actualmente
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Indica si la tarea está completada
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Indica si la tarea está bloqueada (no puede activarse aún)
        /// </summary>
        bool IsLocked { get; }

        /// <summary>
        /// Progreso actual de la tarea (0.0 a 1.0)
        /// </summary>
        float Progress { get; }

        /// <summary>
        /// Estado actual de la tarea
        /// </summary>
        TaskState CurrentState { get; }

        /// <summary>
        /// Lista de IDs de tareas requeridas para activar esta tarea
        /// </summary>
        List<string> RequiredTaskIDs { get; }

        /// <summary>
        /// Intenta activar la tarea si se cumplen las condiciones
        /// </summary>
        /// <returns>True si la tarea se activó correctamente</returns>
        bool Activate();

        /// <summary>
        /// Completa la tarea
        /// </summary>
        /// <returns>True si la tarea se completó correctamente</returns>
        bool Complete();

        /// <summary>
        /// Verifica si la tarea puede activarse
        /// </summary>
        /// <returns>True si se cumplen todas las condiciones de activación</returns>
        bool CanActivate();

        /// <summary>
        /// Actualiza el progreso de la tarea
        /// </summary>
        /// <param name="amount">Cantidad a agregar al progreso</param>
        void UpdateProgress(float amount);

        /// <summary>
        /// Obtiene el progreso actual en formato legible
        /// </summary>
        /// <returns>String con el progreso actual</returns>
        string GetProgressText();
    }
}
