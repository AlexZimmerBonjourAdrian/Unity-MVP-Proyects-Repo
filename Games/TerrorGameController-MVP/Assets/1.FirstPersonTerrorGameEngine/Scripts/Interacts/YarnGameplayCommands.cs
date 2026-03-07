using UnityEngine;
using Yarn.Unity;

namespace HorrorEngine
{
    public class YarnGameplayCommands : MonoBehaviour
    {
        [YarnCommand("play_sfx")] // uso: <<play_sfx 12>>
        public void PlaySfx(string id)
        {
            if (int.TryParse(id, out var soundId))
            {
                CGameEvents.OnPlaySound.Publish(soundId);
            }
            else
            {
                Debug.LogWarning($"play_sfx recibió id no numérico: {id}");
            }
        }

        [YarnCommand("unlock_door")] // uso: <<unlock_door>>
        public void UnlockDoor()
        {
            CGameEvents.OnUnlockDoor.Publish();
        }

        [YarnCommand("set_interactions")] // uso: <<set_interactions false>>
        public void SetInteractions(string enabled)
        {
            if (!bool.TryParse(enabled, out var isEnabled))
            {
                Debug.LogWarning($"set_interactions requiere true/false. Valor: {enabled}");
                return;
            }

            var ray = FindObjectOfType<CInteractRayCast>();
            if (ray == null)
            {
                Debug.LogWarning("CInteractRayCast no encontrado en escena.");
                return;
            }

            ray.SendMessage("SetInteractionsEnabled", isEnabled, SendMessageOptions.DontRequireReceiver);
        }

        [YarnCommand("activate_task")] // uso: <<activate_task task_id>>
        public void ActivateTask(string taskID)
        {
            if (TaskManager.Instance == null)
            {
                Debug.LogWarning("TaskManager no está disponible.");
                return;
            }

            if (string.IsNullOrEmpty(taskID))
            {
                Debug.LogWarning("activate_task requiere un taskID.");
                return;
            }

            bool activated = TaskManager.Instance.ActivateTask(taskID);
            if (!activated)
            {
                Debug.LogWarning($"No se pudo activar la tarea '{taskID}'.");
            }
        }

        [YarnCommand("complete_task")] // uso: <<complete_task task_id>>
        public void CompleteTask(string taskID)
        {
            if (TaskManager.Instance == null)
            {
                Debug.LogWarning("TaskManager no está disponible.");
                return;
            }

            if (string.IsNullOrEmpty(taskID))
            {
                Debug.LogWarning("complete_task requiere un taskID.");
                return;
            }

            bool completed = TaskManager.Instance.CompleteTask(taskID);
            if (!completed)
            {
                Debug.LogWarning($"No se pudo completar la tarea '{taskID}'.");
            }
        }

        [YarnCommand("update_task_progress")] // uso: <<update_task_progress task_id amount>>
        public void UpdateTaskProgress(string taskID, string amount)
        {
            if (TaskManager.Instance == null)
            {
                Debug.LogWarning("TaskManager no está disponible.");
                return;
            }

            if (string.IsNullOrEmpty(taskID))
            {
                Debug.LogWarning("update_task_progress requiere un taskID.");
                return;
            }

            if (!float.TryParse(amount, out float progressAmount))
            {
                Debug.LogWarning($"update_task_progress requiere un número válido. Valor recibido: {amount}");
                return;
            }

            TaskManager.Instance.UpdateTaskProgress(taskID, progressAmount);
        }

        [YarnCommand("set_task_flag")] // uso: <<set_task_flag task_id flag_name>>
        public void SetTaskFlag(string taskID, string flagName)
        {
            if (string.IsNullOrEmpty(taskID) || string.IsNullOrEmpty(flagName))
            {
                Debug.LogWarning("set_task_flag requiere taskID y flagName.");
                return;
            }

            var task = TaskManager.Instance?.GetTask(taskID);
            if (task != null && task.IsCompleted)
            {
                CFlagManager.SetFlag(flagName, true);
            }
            else
            {
                Debug.LogWarning($"La tarea '{taskID}' no está completada o no existe.");
            }
        }
    }
}


