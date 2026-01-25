using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Tarea especializada para granja.
    /// Hereda de BaseTask y agrega lógica específica para tareas agrícolas.
    /// </summary>
    public class FarmTask : BaseTask
    {
        [Header("Configuración de Granja")]
        [SerializeField] private FarmTaskDataSO farmTaskData;

        [Header("Estado de Granja")]
        [SerializeField] private FarmTaskType farmTaskType;
        [SerializeField] private string relatedAnimalID = "";
        [SerializeField] private string relatedItemID = "";

        private void Awake()
        {
            base.Awake();

            // Inicializar desde TaskDataSO si está asignado
            if (farmTaskData != null)
            {
                InitializeFromData(farmTaskData);
            }
        }

        /// <summary>
        /// Inicializa la tarea desde un FarmTaskDataSO
        /// </summary>
        public void InitializeFromData(FarmTaskDataSO data)
        {
            if (data == null) return;

            farmTaskData = data;
            taskID = data.taskID;
            taskName = data.taskName;
            description = data.description;
            farmTaskType = data.farmTaskType;
            relatedAnimalID = data.animalID;
            relatedItemID = data.itemID;

            // Configurar progreso
            taskProgress = data.CreateTaskProgress();

            // Configurar condiciones
            if (data.activationConditions != null)
            {
                activationConditions = data.activationConditions;
            }

            // Configurar dependencias
            if (data.requiredTaskIDs != null)
            {
                requiredTaskIDs = new System.Collections.Generic.List<string>(data.requiredTaskIDs);
            }

            // Configurar subtareas
            if (data.subtaskIDs != null)
            {
                subtaskIDs = new System.Collections.Generic.List<string>(data.subtaskIDs);
            }
        }

        /// <summary>
        /// Se llama cuando la tarea se activa
        /// </summary>
        protected override void OnActivated()
        {
            base.OnActivated();
            LogDebug($"Tarea de granja activada: {taskName} (Tipo: {farmTaskType})");
        }

        /// <summary>
        /// Se llama cuando la tarea se completa
        /// </summary>
        protected override void OnCompleted()
        {
            base.OnCompleted();
            LogDebug($"Tarea de granja completada: {taskName}");

            // Activar flag de completado si está configurado
            if (farmTaskData != null && !string.IsNullOrEmpty(farmTaskData.completionFlag))
            {
                CFlagManager.SetFlag(farmTaskData.completionFlag, true);
            }

            // Disparar evento de completado si está configurado
            if (farmTaskData != null && !string.IsNullOrEmpty(farmTaskData.completionEventID))
            {
                CGameEventManager.Publish(farmTaskData.completionEventID);
            }

            // Afectar relación con animales si está configurado
            if (farmTaskData != null && farmTaskData.affectsAnimalRelationship && !string.IsNullOrEmpty(relatedAnimalID))
            {
                // Aquí se integraría con el sistema de relación con animales (futuro)
                // AnimalRelationshipSystem.Instance.ModifyRelationship(relatedAnimalID, farmTaskData.relationshipChange);
                LogDebug($"Relación con animal '{relatedAnimalID}' modificada en {farmTaskData.relationshipChange}");
            }
        }

        /// <summary>
        /// Verifica si un item puede contribuir al progreso de esta tarea
        /// </summary>
        public bool CanContributeWithItem(string itemID)
        {
            if (currentState != TaskState.Active) return false;
            if (string.IsNullOrEmpty(relatedItemID)) return false;
            return relatedItemID == itemID;
        }

        /// <summary>
        /// Verifica si un animal puede contribuir al progreso de esta tarea
        /// </summary>
        public bool CanContributeWithAnimal(string animalID)
        {
            if (currentState != TaskState.Active) return false;
            if (string.IsNullOrEmpty(relatedAnimalID)) return false;
            return relatedAnimalID == animalID;
        }

        /// <summary>
        /// Obtiene el tipo de tarea de granja
        /// </summary>
        public FarmTaskType GetFarmTaskType()
        {
            return farmTaskType;
        }

        /// <summary>
        /// Obtiene el ID del animal relacionado
        /// </summary>
        public string GetRelatedAnimalID()
        {
            return relatedAnimalID;
        }

        /// <summary>
        /// Obtiene el ID del item relacionado
        /// </summary>
        public string GetRelatedItemID()
        {
            return relatedItemID;
        }

        private void LogDebug(string message)
        {
            if (farmTaskData != null)
            {
                Debug.Log($"[FarmTask: {taskID}] {message}");
            }
        }
    }
}
