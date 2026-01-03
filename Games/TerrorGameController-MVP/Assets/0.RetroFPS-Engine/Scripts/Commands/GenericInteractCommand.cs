using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Comando genérico que envuelve llamadas a Iinteract
    /// Permite integrar el sistema legacy de Iinteract con el patrón Command
    /// </summary>
    public class GenericInteractCommand : InteractableCommand
    {
        private Iinteract interactable;

        public GenericInteractCommand(GameObject targetObject, Iinteract interactable)
            : base(targetObject)
        {
            this.interactable = interactable;
        }

        public override void Execute()
        {
            if (!CanExecute())
            {
                Debug.LogWarning($"[GenericInteractCommand] Cannot execute interaction with {targetObject.name}");
                return;
            }

            Debug.Log($"[GenericInteractCommand] Executing interaction with {targetObject.name}");
            interactable.Oninteract();
            MarkAsExecuted();
        }

        public override void Undo()
        {
            // Las interacciones legacy generalmente no son reversibles
            Debug.LogWarning($"[GenericInteractCommand] Undo not supported for legacy interaction: {targetObject.name}");
        }

        public override bool CanExecute()
        {
            // Verificaciones base
            if (!base.CanExecute()) return false;

            // Verificar que el interactable existe
            if (interactable == null)
            {
                Debug.LogError($"[GenericInteractCommand] Interactable is null for {targetObject.name}");
                return false;
            }

            return true;
        }

        public override string Description => $"Interactuar con {targetObject.name}";
    }
}

