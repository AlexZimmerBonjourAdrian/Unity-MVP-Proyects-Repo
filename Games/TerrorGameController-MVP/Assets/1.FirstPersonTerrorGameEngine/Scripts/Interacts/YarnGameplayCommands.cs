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
    }
}


