using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Manager singleton que gestiona todas las tareas del juego.
    /// Se encarga de registrar, activar, completar y persistir el estado de las tareas.
    /// </summary>
    public class TaskManager : MonoBehaviour, IDataPersistence
    {
        public static TaskManager Instance { get; private set; }

        [Header("Configuración")]
        [Tooltip("Si es true, las tareas se activarán automáticamente cuando se cumplan las condiciones")]
        [SerializeField] private bool autoActivateTasks = true;

        [Tooltip("Tiempo entre verificaciones de auto-activación (en segundos)")]
        [SerializeField] private float autoActivationCheckInterval = 1f;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = false;

        // Diccionario de todas las tareas registradas
        private Dictionary<string, ITask> registeredTasks = new Dictionary<string, ITask>();

        // Datos de progreso guardados
        private TasksData tasksData = new TasksData();

        private Coroutine autoActivationCoroutine;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple instances of TaskManager detected. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Iniciar corrutina de auto-activación si está habilitada
            if (autoActivateTasks)
            {
                autoActivationCoroutine = StartCoroutine(AutoActivationCheck());
            }
        }

        private void OnDestroy()
        {
            if (autoActivationCoroutine != null)
            {
                StopCoroutine(autoActivationCoroutine);
            }
        }

        #region Registro de Tareas

        /// <summary>
        /// Registra una tarea en el sistema
        /// </summary>
        public void RegisterTask(ITask task)
        {
            if (task == null)
            {
                LogDebug("Intento de registrar una tarea null");
                return;
            }

            if (string.IsNullOrEmpty(task.TaskID))
            {
                LogDebug("Intento de registrar una tarea sin ID");
                return;
            }

            if (registeredTasks.ContainsKey(task.TaskID))
            {
                LogDebug($"La tarea '{task.TaskID}' ya está registrada");
                return;
            }

            registeredTasks[task.TaskID] = task;
            CGameEvents.OnTaskRegistered.Publish(task.TaskID);
            LogDebug($"Tarea registrada: {task.TaskID}");

            // Cargar estado guardado si existe
            LoadTaskState(task);
        }

        /// <summary>
        /// Desregistra una tarea del sistema
        /// </summary>
        public void UnregisterTask(string taskID)
        {
            if (registeredTasks.ContainsKey(taskID))
            {
                registeredTasks.Remove(taskID);
                LogDebug($"Tarea desregistrada: {taskID}");
            }
        }

        /// <summary>
        /// Obtiene una tarea por su ID
        /// </summary>
        public ITask GetTask(string taskID)
        {
            registeredTasks.TryGetValue(taskID, out ITask task);
            return task;
        }

        /// <summary>
        /// Obtiene todas las tareas registradas
        /// </summary>
        public List<ITask> GetAllTasks()
        {
            return registeredTasks.Values.ToList();
        }

        /// <summary>
        /// Obtiene todas las tareas de un estado específico
        /// </summary>
        public List<ITask> GetTasksByState(TaskState state)
        {
            return registeredTasks.Values
                .Where(t => t is BaseTask && ((BaseTask)t).CurrentState == state)
                .ToList();
        }

        #endregion

        #region Activación y Completado

        /// <summary>
        /// Intenta activar una tarea
        /// </summary>
        public bool ActivateTask(string taskID)
        {
            var task = GetTask(taskID);
            if (task == null)
            {
                LogDebug($"No se encontró la tarea '{taskID}' para activar");
                return false;
            }

            return task.Activate();
        }

        /// <summary>
        /// Completa una tarea
        /// </summary>
        public bool CompleteTask(string taskID)
        {
            var task = GetTask(taskID);
            if (task == null)
            {
                LogDebug($"No se encontró la tarea '{taskID}' para completar");
                return false;
            }

            return task.Complete();
        }

        /// <summary>
        /// Actualiza el progreso de una tarea
        /// </summary>
        public void UpdateTaskProgress(string taskID, float amount)
        {
            var task = GetTask(taskID);
            if (task == null)
            {
                LogDebug($"No se encontró la tarea '{taskID}' para actualizar progreso");
                return;
            }

            task.UpdateProgress(amount);
        }

        #endregion

        #region Auto-Activación

        /// <summary>
        /// Corrutina que verifica periódicamente si hay tareas que pueden auto-activarse
        /// </summary>
        private System.Collections.IEnumerator AutoActivationCheck()
        {
            while (true)
            {
                yield return new WaitForSeconds(autoActivationCheckInterval);

                if (!autoActivateTasks) continue;

                foreach (var task in registeredTasks.Values)
                {
                    if (task is BaseTask baseTask)
                    {
                        // Solo verificar tareas que están en estado Available
                        if (baseTask.CurrentState == TaskState.Available && baseTask.CanActivate())
                        {
                            baseTask.Activate();
                        }
                    }
                }
            }
        }

        #endregion

        #region Persistencia

        /// <summary>
        /// Carga el estado guardado de una tarea
        /// </summary>
        private void LoadTaskState(ITask task)
        {
            if (task is BaseTask baseTask)
            {
                var savedData = tasksData.GetTaskData(task.TaskID);
                if (savedData != null)
                {
                    baseTask.LoadProgressData(savedData);
                    LogDebug($"Estado cargado para tarea: {task.TaskID}");
                }
            }
        }

        /// <summary>
        /// Guarda el estado de todas las tareas
        /// </summary>
        private void SaveAllTasksState()
        {
            tasksData.Clear();

            foreach (var task in registeredTasks.Values)
            {
                if (task is BaseTask baseTask)
                {
                    var progressData = baseTask.GetProgressData();
                    tasksData.SetTaskData(progressData);
                }
            }
        }

        #endregion

        #region IDataPersistence

        public void LoadData(GameData data)
        {
            if (data.tasksData != null)
            {
                tasksData = data.tasksData;

                // Cargar estado de todas las tareas registradas
                foreach (var task in registeredTasks.Values)
                {
                    if (task is BaseTask baseTask)
                    {
                        var savedData = tasksData.GetTaskData(task.TaskID);
                        if (savedData != null)
                        {
                            baseTask.LoadProgressData(savedData);
                        }
                    }
                }

                LogDebug("Datos de tareas cargados");
            }
        }

        public void SaveData(GameData data)
        {
            SaveAllTasksState();
            data.tasksData = tasksData;
            LogDebug("Datos de tareas guardados");
        }

        #endregion

        #region Utilidades

        private void LogDebug(string message)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[TaskManager] {message}");
            }
        }

        /// <summary>
        /// Obtiene el número total de tareas registradas
        /// </summary>
        public int GetTaskCount()
        {
            return registeredTasks.Count;
        }

        /// <summary>
        /// Obtiene el número de tareas completadas
        /// </summary>
        public int GetCompletedTaskCount()
        {
            return registeredTasks.Values.Count(t => t.IsCompleted);
        }

        #endregion
    }
}
