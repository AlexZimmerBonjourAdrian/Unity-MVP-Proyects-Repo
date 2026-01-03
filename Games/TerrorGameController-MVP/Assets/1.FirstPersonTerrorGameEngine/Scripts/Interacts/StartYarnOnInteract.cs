using UnityEngine;

namespace HorrorEngine
{
    public class StartYarnOnInteract : MonoBehaviour, Iinteract
    {
        [SerializeField] private string nodeName;

        public void Oninteract()
        {
            var controller = DialogueController.Instance;
            if (controller == null || controller.DialogueRunner == null)
            {
                Debug.LogWarning("DialogueController o DialogueRunner no están disponibles.");
                return;
            }

            if (string.IsNullOrEmpty(nodeName))
            {
                Debug.LogWarning("nodeName no asignado en StartYarnOnInteract.");
                return;
            }

            controller.DialogueRunner.Stop();
            controller.DialogueRunner.StartDialogue(nodeName);
        }
    }
}


