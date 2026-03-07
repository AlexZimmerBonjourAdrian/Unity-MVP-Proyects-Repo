using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Componente que permite a objetos interactuables completar o actualizar tareas.
    /// Se puede usar en objetos del mundo que el jugador puede interactuar.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TaskInteractable : MonoBehaviour, Iinteract
    {
        [Header("Configuración de Tarea")]
        [Tooltip("ID de la tarea que se completará o actualizará")]
        [SerializeField] private string taskID = "";

        [Header("Tipo de Acción")]
        [Tooltip("Si es true, completa la tarea. Si es false, solo actualiza el progreso")]
        [SerializeField] private bool completeTask = false;

        [Header("Actualización de Progreso")]
        [Tooltip("Cantidad de progreso a agregar (solo si completeTask es false)")]
        [SerializeField] private float progressAmount = 1f;

        [Header("Configuración")]
        [Tooltip("Si es true, el objeto se desactiva después de la interacción")]
        [SerializeField] private bool disableAfterInteraction = false;

        [Tooltip("Si es true, solo se puede interactuar una vez")]
        [SerializeField] private bool oneTimeUse = true;

        [Header("Condiciones")]
        [Tooltip("Flag que debe estar activa para poder interactuar")]
        [SerializeField] private string requiredFlag = "";

        [Tooltip("Si es true, requiere que la flag esté activa. Si es false, requiere que esté inactiva")]
        [SerializeField] private bool flagMustBeActive = true;

        private bool hasBeenUsed = false;

        /// <summary>
        /// Se llama cuando el jugador interactúa con el objeto
        /// </summary>
        public void Oninteract()
        {
            // Verificar si ya se usó
            if (oneTimeUse && hasBeenUsed)
            {
                return;
            }

            // Verificar condiciones
            if (!CanInteract())
            {
                return;
            }

            // Procesar la interacción
            ProcessTaskInteraction();

            // Marcar como usado
            hasBeenUsed = true;

            // Desactivar si es necesario
            if (disableAfterInteraction)
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Verifica si el objeto puede ser interactuado
        /// </summary>
        private bool CanInteract()
        {
            // Verificar flag requerida
            if (!string.IsNullOrEmpty(requiredFlag))
            {
                bool flagValue = CFlagManager.GetFlag(requiredFlag);
                if (flagValue != flagMustBeActive)
                {
                    return false;
                }
            }

            // Verificar que la tarea existe
            if (TaskManager.Instance == null)
            {
                Debug.LogWarning($"TaskInteractable en '{gameObject.name}': TaskManager no está disponible");
                return false;
            }

            var task = TaskManager.Instance.GetTask(taskID);
            if (task == null)
            {
                Debug.LogWarning($"TaskInteractable en '{gameObject.name}': Tarea '{taskID}' no encontrada");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Procesa la interacción con la tarea
        /// </summary>
        private void ProcessTaskInteraction()
        {
            if (TaskManager.Instance == null) return;

            if (completeTask)
            {
                // Completar la tarea
                TaskManager.Instance.CompleteTask(taskID);
                Debug.Log($"Tarea '{taskID}' completada por interacción con '{gameObject.name}'");
            }
            else
            {
                // Actualizar el progreso
                TaskManager.Instance.UpdateTaskProgress(taskID, progressAmount);
                Debug.Log($"Progreso de tarea '{taskID}' actualizado en {progressAmount} por interacción con '{gameObject.name}'");
            }
        }

        /// <summary>
        /// Establece el ID de la tarea (útil para configuración dinámica)
        /// </summary>
        public void SetTaskID(string newTaskID)
        {
            taskID = newTaskID;
        }

        /// <summary>
        /// Establece si debe completar la tarea o solo actualizar progreso
        /// </summary>
        public void SetCompleteTask(bool complete)
        {
            completeTask = complete;
        }

        /// <summary>
        /// Establece la cantidad de progreso a agregar
        /// </summary>
        public void SetProgressAmount(float amount)
        {
            progressAmount = amount;
        }

        /// <summary>
        /// Reinicia el estado de uso (útil para testing o reinicios)
        /// </summary>
        public void ResetUsage()
        {
            hasBeenUsed = false;
            if (disableAfterInteraction)
            {
                gameObject.SetActive(true);
            }
        }

        private void OnValidate()
        {
            // Validar que el collider esté configurado como trigger si es necesario
            Collider col = GetComponent<Collider>();
            if (col != null && !col.isTrigger)
            {
                // No forzamos que sea trigger, pero avisamos
                // El sistema de interacción puede usar raycast en lugar de triggers
            }
        }
    }
}
