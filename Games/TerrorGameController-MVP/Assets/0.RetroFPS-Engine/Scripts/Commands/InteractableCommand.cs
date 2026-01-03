using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Command Pattern - Clase base abstracta para comandos interactivos.
    /// Proporciona funcionalidad común para comandos que interactúan con objetos del juego.
    /// </summary>
    public abstract class InteractableCommand : ICommand
    {
        protected GameObject targetObject;
        protected Vector3 interactionPosition;
        protected bool hasBeenExecuted = false;

        /// <summary>
        /// Constructor base para comandos interactivos
        /// </summary>
        /// <param name="target">Objeto con el que se interactúa</param>
        protected InteractableCommand(GameObject target)
        {
            targetObject = target;
            interactionPosition = target != null ? target.transform.position : Vector3.zero;
        }

        /// <summary>
        /// Ejecuta el comando interactivo
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Deshace el comando interactivo (si es soportado)
        /// </summary>
        public abstract void Undo();

        /// <summary>
        /// Verifica si el comando puede ejecutarse
        /// </summary>
        public virtual bool CanExecute()
        {
            // Verificaciones básicas
            if (targetObject == null)
            {
                Debug.LogWarning($"[{GetType().Name}] Target object is null");
                return false;
            }

            if (!targetObject.activeInHierarchy)
            {
                Debug.LogWarning($"[{GetType().Name}] Target object is not active");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Descripción del comando
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Objeto target del comando
        /// </summary>
        public GameObject TargetObject => targetObject;

        /// <summary>
        /// Posición donde ocurrió la interacción
        /// </summary>
        public Vector3 InteractionPosition => interactionPosition;

        /// <summary>
        /// Indica si el comando ya fue ejecutado
        /// </summary>
        public bool HasBeenExecuted => hasBeenExecuted;

        /// <summary>
        /// Método helper para marcar el comando como ejecutado
        /// </summary>
        protected void MarkAsExecuted()
        {
            hasBeenExecuted = true;
            LogDebug("Command executed");
        }

        /// <summary>
        /// Método helper para marcar el comando como deshecho
        /// </summary>
        protected void MarkAsUndone()
        {
            hasBeenExecuted = false;
            LogDebug("Command undone");
        }

        /// <summary>
        /// Logging interno (solo en modo debug)
        /// </summary>
        protected void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[{GetType().Name}] {message}");
#endif
        }

        /// <summary>
        /// Obtiene información de debug del comando
        /// </summary>
        public virtual string GetDebugInfo()
        {
            return $"{GetType().Name} Debug Info:\n" +
                   $"- Target: {(targetObject != null ? targetObject.name : "null")}\n" +
                   $"- Position: {interactionPosition}\n" +
                   $"- Executed: {hasBeenExecuted}\n" +
                   $"- Can Execute: {CanExecute()}";
        }
    }

    /// <summary>
    /// Extensión para facilitar el uso de comandos
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Ejecuta un comando si puede ejecutarse
        /// </summary>
        public static bool TryExecute(this ICommand command)
        {
            if (command.CanExecute())
            {
                command.Execute();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ejecuta el undo de un comando si puede hacerlo
        /// </summary>
        public static bool TryUndo(this ICommand command)
        {
            // Asumimos que si el comando existe, puede hacer undo
            // (esto puede ser sobrescrito en implementaciones específicas)
            command.Undo();
            return true;
        }
    }
}
