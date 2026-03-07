using System;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Tipo de condición para activar o completar una tarea
    /// </summary>
    public enum TaskConditionType
    {
        Flag,           // Condición basada en flag
        Event,          // Condición basada en evento
        Task,           // Condición basada en otra tarea
        Custom          // Condición personalizada
    }

    /// <summary>
    /// Operador lógico para combinar condiciones
    /// </summary>
    public enum TaskConditionOperator
    {
        AND,    // Todas las condiciones deben cumplirse
        OR      // Al menos una condición debe cumplirse
    }

    /// <summary>
    /// Condición individual para una tarea
    /// </summary>
    [System.Serializable]
    public class TaskCondition
    {
        [Header("Tipo de Condición")]
        [SerializeField] private TaskConditionType conditionType = TaskConditionType.Flag;

        [Header("Configuración")]
        [SerializeField] private string conditionID = "";
        [SerializeField] private bool requiredValue = true;

        [Header("Descripción")]
        [SerializeField] private string description = "";

        public TaskConditionType ConditionType => conditionType;
        public string ConditionID => conditionID;
        public bool RequiredValue => requiredValue;
        public string Description => description;

        public TaskCondition()
        {
            conditionType = TaskConditionType.Flag;
            conditionID = "";
            requiredValue = true;
            description = "";
        }

        public TaskCondition(TaskConditionType type, string id, bool value = true)
        {
            conditionType = type;
            conditionID = id;
            requiredValue = value;
            description = "";
        }

        /// <summary>
        /// Evalúa si la condición se cumple
        /// </summary>
        /// <returns>True si la condición se cumple</returns>
        public bool Evaluate()
        {
            switch (conditionType)
            {
                case TaskConditionType.Flag:
                    bool flagValue = CFlagManager.GetFlag(conditionID);
                    return flagValue == requiredValue;

                case TaskConditionType.Event:
                    // Los eventos se evalúan de forma diferente
                    // Por ahora, verificamos si el evento fue disparado (usando flags como proxy)
                    return CFlagManager.GetFlag($"event_{conditionID}") == requiredValue;

                case TaskConditionType.Task:
                    // Verificamos si la tarea está completada
                    var taskManager = TaskManager.Instance;
                    if (taskManager != null)
                    {
                        var task = taskManager.GetTask(conditionID);
                        if (task != null)
                        {
                            return (task.IsCompleted == requiredValue);
                        }
                    }
                    return false;

                case TaskConditionType.Custom:
                    // Las condiciones personalizadas deben ser evaluadas externamente
                    return false;

                default:
                    return false;
            }
        }
    }

    /// <summary>
    /// Grupo de condiciones con operador lógico
    /// </summary>
    [System.Serializable]
    public class TaskConditionGroup
    {
        [Header("Operador Lógico")]
        [SerializeField] private TaskConditionOperator conditionOperator = TaskConditionOperator.AND;

        [Header("Condiciones")]
        [SerializeField] private List<TaskCondition> conditions = new List<TaskCondition>();

        public TaskConditionOperator ConditionOperator => conditionOperator;
        public List<TaskCondition> Conditions => conditions;

        public TaskConditionGroup()
        {
            conditionOperator = TaskConditionOperator.AND;
            conditions = new List<TaskCondition>();
        }

        public TaskConditionGroup(TaskConditionOperator op)
        {
            conditionOperator = op;
            conditions = new List<TaskCondition>();
        }

        /// <summary>
        /// Evalúa todas las condiciones del grupo
        /// </summary>
        /// <returns>True si todas las condiciones se cumplen según el operador</returns>
        public bool Evaluate()
        {
            if (conditions == null || conditions.Count == 0)
                return true;

            if (conditionOperator == TaskConditionOperator.AND)
            {
                // Todas las condiciones deben cumplirse
                foreach (var condition in conditions)
                {
                    if (condition == null) continue;
                    if (!condition.Evaluate())
                        return false;
                }
                return true;
            }
            else // OR
            {
                // Al menos una condición debe cumplirse
                foreach (var condition in conditions)
                {
                    if (condition == null) continue;
                    if (condition.Evaluate())
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Agrega una condición al grupo
        /// </summary>
        public void AddCondition(TaskCondition condition)
        {
            if (conditions == null)
                conditions = new List<TaskCondition>();

            conditions.Add(condition);
        }
    }
}
