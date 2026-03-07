using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Tipo de tarea de granja
    /// </summary>
    public enum FarmTaskType
    {
        Collect,        // Recolectar items
        Feed,           // Alimentar animales
        Kill,           // Matar animales
        Talk,           // Hablar con animales
        Repair,         // Reparar estructuras
        Plant,          // Plantar cultivos
        Harvest         // Cosechar
    }

    /// <summary>
    /// ScriptableObject especializado para tareas de granja.
    /// Hereda de TaskDataSO y agrega campos específicos para tareas agrícolas.
    /// </summary>
    [CreateAssetMenu(fileName = "NewFarmTask", menuName = "HorrorEngine/Tasks/Farm Task Data", order = 2)]
    public class FarmTaskDataSO : TaskDataSO
    {
        [Header("Tipo de Tarea de Granja")]
        [Tooltip("Tipo específico de tarea de granja")]
        public FarmTaskType farmTaskType = FarmTaskType.Collect;

        [Header("Animal Relacionado")]
        [Tooltip("ID del animal relacionado con esta tarea (opcional)")]
        public string animalID = "";

        [Header("Item/Objeto")]
        [Tooltip("ID del item u objeto relacionado con esta tarea (opcional)")]
        public string itemID = "";

        [Header("Cantidad")]
        [Tooltip("Cantidad requerida para tareas de tipo Counter")]
        public int requiredAmount = 1;

        [Header("Configuración Específica")]
        [Tooltip("Si es true, esta tarea afecta la relación con animales")]
        public bool affectsAnimalRelationship = false;

        [Tooltip("Cambio en la relación con animales al completar esta tarea (-1 a 1)")]
        [Range(-1f, 1f)]
        public float relationshipChange = 0f;

        /// <summary>
        /// Valida la configuración específica de la tarea de granja
        /// </summary>
        public new bool Validate()
        {
            if (!base.Validate())
            {
                return false;
            }

            if (progressType == TaskProgressType.Counter && requiredAmount <= 0)
            {
                Debug.LogWarning($"FarmTaskDataSO '{name}' tiene requiredAmount <= 0 para tipo Counter");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Configura el progreso máximo basado en el tipo de tarea y cantidad requerida
        /// </summary>
        public void SetupProgressFromType()
        {
            switch (farmTaskType)
            {
                case FarmTaskType.Collect:
                case FarmTaskType.Feed:
                case FarmTaskType.Kill:
                case FarmTaskType.Plant:
                case FarmTaskType.Harvest:
                    if (progressType == TaskProgressType.Counter)
                    {
                        maxProgressValue = requiredAmount;
                    }
                    break;

                case FarmTaskType.Talk:
                case FarmTaskType.Repair:
                    progressType = TaskProgressType.Boolean;
                    maxProgressValue = 1f;
                    break;
            }
        }
    }
}
