using System;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Estado de una tarea
    /// </summary>
    public enum TaskState
    {
        Locked,     // Bloqueada, no puede activarse
        Available,  // Disponible, puede activarse
        Active,     // Activa, en progreso
        Completed   // Completada
    }

    /// <summary>
    /// Clase base abstracta para todas las tareas.
    /// Implementa la lógica común de gestión de estado, progreso y condiciones.
    /// </summary>
    public abstract class BaseTask : MonoBehaviour, ITask
    {
        [Header("Identificación")]
        [SerializeField] protected string taskID = "";
        [SerializeField] protected string taskName = "";
        [TextArea(3, 5)]
        [SerializeField] protected string description = "";

        [Header("Progreso")]
        [SerializeField] protected TaskProgress taskProgress = new TaskProgress();

        [Header("Condiciones de Activación")]
        [SerializeField] protected TaskConditionGroup activationConditions = new TaskConditionGroup();

        [Header("Estado")]
        [SerializeField] protected TaskState currentState = TaskState.Locked;

        [Header("Dependencias")]
        [SerializeField] protected List<string> requiredTaskIDs = new List<string>();

        [Header("Subtareas")]
        [SerializeField] protected List<string> subtaskIDs = new List<string>();

        // Propiedades de ITask
        public string TaskID => taskID;
        public string TaskName => taskName;
        public string Description => description;
        public float Progress => taskProgress.NormalizedProgress;

        public bool IsActive => currentState == TaskState.Active;
        public bool IsCompleted => currentState == TaskState.Completed;
        public bool IsLocked => currentState == TaskState.Locked;

        public TaskState CurrentState => currentState;
        
        // Propiedades públicas para acceso desde TaskManager
        public List<string> RequiredTaskIDs => requiredTaskIDs;

        protected virtual void Awake()
        {
            if (string.IsNullOrEmpty(taskID))
            {
                taskID = gameObject.name;
            }
        }

        protected virtual void Start()
        {
            // Registrar la tarea en el TaskManager
            if (TaskManager.Instance != null)
            {
                TaskManager.Instance.RegisterTask(this);
            }
        }

        protected virtual void OnDestroy()
        {
            // Desregistrar la tarea
            if (TaskManager.Instance != null)
            {
                TaskManager.Instance.UnregisterTask(taskID);
            }
        }

        /// <summary>
        /// Intenta activar la tarea si se cumplen las condiciones
        /// </summary>
        public virtual bool Activate()
        {
            if (currentState == TaskState.Active || currentState == TaskState.Completed)
            {
                return false;
            }

            if (!CanActivate())
            {
                return false;
            }

            currentState = TaskState.Active;
            OnActivated();

            // Disparar evento
            CGameEvents.OnTaskActivated.Publish(taskID);

            return true;
        }

        /// <summary>
        /// Completa la tarea
        /// </summary>
        public virtual bool Complete()
        {
            if (currentState == TaskState.Completed)
            {
                return false;
            }

            if (!taskProgress.IsComplete)
            {
                // Forzar el progreso a completo si no lo está
                taskProgress.SetProgress(taskProgress.MaxValue);
            }

            currentState = TaskState.Completed;
            OnCompleted();

            // Disparar evento
            CGameEvents.OnTaskCompleted.Publish(taskID);

            // Desbloquear tareas dependientes
            UnlockDependentTasks();

            return true;
        }

        /// <summary>
        /// Verifica si la tarea puede activarse
        /// </summary>
        public virtual bool CanActivate()
        {
            // Verificar estado actual
            if (currentState == TaskState.Active || currentState == TaskState.Completed)
            {
                return false;
            }

            // Verificar condiciones de activación
            if (activationConditions != null && !activationConditions.Evaluate())
            {
                return false;
            }

            // Verificar tareas requeridas
            if (requiredTaskIDs != null && requiredTaskIDs.Count > 0)
            {
                var taskManager = TaskManager.Instance;
                if (taskManager != null)
                {
                    foreach (var requiredTaskID in requiredTaskIDs)
                    {
                        var requiredTask = taskManager.GetTask(requiredTaskID);
                        if (requiredTask == null || !requiredTask.IsCompleted)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Actualiza el progreso de la tarea
        /// </summary>
        public virtual void UpdateProgress(float amount)
        {
            if (currentState != TaskState.Active)
            {
                return;
            }

            float oldProgress = taskProgress.NormalizedProgress;
            taskProgress.UpdateProgress(amount);

            // Disparar evento de cambio de progreso
            if (Mathf.Abs(taskProgress.NormalizedProgress - oldProgress) > 0.01f)
            {
                CGameEvents.OnTaskProgressChanged.Publish(taskID);
            }

            // Verificar si se completó
            if (taskProgress.IsComplete && currentState == TaskState.Active)
            {
                Complete();
            }
        }

        /// <summary>
        /// Obtiene el texto de progreso
        /// </summary>
        public virtual string GetProgressText()
        {
            return taskProgress.GetProgressText();
        }

        /// <summary>
        /// Desbloquea las tareas que dependen de esta
        /// </summary>
        protected virtual void UnlockDependentTasks()
        {
            if (TaskManager.Instance == null) return;

            // Buscar tareas que tienen esta tarea como requerida
            var allTasks = TaskManager.Instance.GetAllTasks();
            foreach (var task in allTasks)
            {
                if (task != null && task.RequiredTaskIDs != null && task.RequiredTaskIDs.Contains(taskID))
                {
                    // Cambiar estado a Available si está Locked
                    if (task.CurrentState == TaskState.Locked)
                    {
                        // Hacer cast a BaseTask para poder cambiar el estado
                        if (task is BaseTask baseTask)
                        {
                            baseTask.SetState(TaskState.Available);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Se llama cuando la tarea se activa
        /// </summary>
        protected virtual void OnActivated()
        {
            // Override en clases derivadas
        }

        /// <summary>
        /// Se llama cuando la tarea se completa
        /// </summary>
        protected virtual void OnCompleted()
        {
            // Override en clases derivadas
        }

        /// <summary>
        /// Establece el estado de la tarea (para carga de guardado)
        /// </summary>
        public virtual void SetState(TaskState state, float progressValue = 0f)
        {
            currentState = state;
            if (progressValue > 0f)
            {
                taskProgress.SetProgress(progressValue);
            }
        }

        /// <summary>
        /// Obtiene el estado actual de la tarea para guardado
        /// </summary>
        public virtual TaskProgressData GetProgressData()
        {
            return new TaskProgressData
            {
                taskID = taskID,
                state = currentState,
                progressValue = taskProgress.CurrentValue,
                isComplete = taskProgress.IsComplete
            };
        }

        /// <summary>
        /// Carga el estado de la tarea desde datos guardados
        /// </summary>
        public virtual void LoadProgressData(TaskProgressData data)
        {
            if (data.taskID != taskID) return;

            SetState(data.state, data.progressValue);
        }
    }
}
