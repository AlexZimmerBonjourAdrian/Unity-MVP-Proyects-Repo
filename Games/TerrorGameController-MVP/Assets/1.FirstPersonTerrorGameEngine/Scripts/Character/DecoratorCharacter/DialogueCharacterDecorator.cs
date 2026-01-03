using UnityEngine;
using Yarn;

namespace HorrorEngine
{
    public class DialogueCharacterDecorator : CharacterDecorator
{
    // Aquí puedes añadir propiedades específicas para el decorador de diálogo
    
   [SerializeField]private string _dialogueNode;
    
    // Método para iniciar el diálogo
    public void StartDialogue()
    {
        // Verificar que DialogueController.Instance existe
        if (DialogueController.Instance == null)
        {
            Debug.LogError("DialogueController.Instance es null. Asegúrate de que DialogueController esté en la escena.");
            return;
        }

        // Verificar que DialogueRunner existe
        if (DialogueController.Instance.DialogueRunner == null)
        {
            Debug.LogError("DialogueRunner es null. Verifica que esté asignado en el inspector.");
            return;
        }

        // Verificar que el nodo de diálogo no esté vacío
        if (string.IsNullOrEmpty(_dialogueNode))
        {
            Debug.LogError("_dialogueNode está vacío. Asigna un nodo de diálogo en el inspector.");
            return;
        }

        try
        {
            DialogueController.Instance.DialogueRunner.Stop();
            DialogueController.Instance.DialogueRunner.StartDialogue(_dialogueNode);
            Debug.Log($"Diálogo iniciado: {_dialogueNode}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al iniciar el diálogo: {e.Message}");
        }
    }
    
    // Método para finalizar el diálogo
    public void EndDialogue()
    {
        Debug.Log("Dialogue ended.");
        // Aquí podrías añadir lógica para finalizar el diálogo con el jugador
    }

      public override void Inicilizate()
    {
        _character?.Inicilizate();
    }

    public override void Oninteract()
    {
         StartDialogue();
         _interact?.Oninteract();
    }
    }
}
