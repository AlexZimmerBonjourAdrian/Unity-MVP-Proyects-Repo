using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Comando para abrir puertas interactivas
    /// </summary>
    public class OpenDoorCommand : InteractableCommand
    {
        private bool wasOpen;
        private bool requiresKey;
        private string keyType;
        private IDoor doorComponent;

        /// <summary>
        /// Constructor para comando de abrir puerta
        /// </summary>
        /// <param name="door">Objeto puerta a abrir</param>
        /// <param name="requiresKey">Si requiere llave</param>
        /// <param name="keyType">Tipo de llave requerida</param>
        public OpenDoorCommand(GameObject door, bool requiresKey = false, string keyType = "")
            : base(door)
        {
            this.requiresKey = requiresKey;
            this.keyType = keyType;

            // Intentar obtener el componente de puerta
            doorComponent = door.GetComponent<IDoor>();
        }

        public override void Execute()
        {
            if (!CanExecute())
            {
                LogDebug("Cannot execute - conditions not met");
                return;
            }

            LogDebug("Executing OpenDoor command");

            // Guardar estado anterior
            wasOpen = IsDoorOpen();

            // Abrir la puerta
            OpenDoor();

            // Publicar evento
            var doorEvent = new DoorOpenedEvent(targetObject, requiresKey, keyType);
            EventBus.Publish(doorEvent);

            // Actualizar observers globales
            RetroFPS.GameObservers.DoorOpened.SetValue(targetObject.name);

            MarkAsExecuted();
        }

        public override void Undo()
        {
            if (!hasBeenExecuted)
            {
                LogDebug("Cannot undo - command was not executed");
                return;
            }

            LogDebug("Undoing OpenDoor command");

            // Solo cerrar si estaba cerrada originalmente
            if (!wasOpen)
            {
                CloseDoor();
            }

            MarkAsUndone();
        }

        public override bool CanExecute()
        {
            // Verificaciones base
            if (!base.CanExecute())
                return false;

            // Verificar si requiere llave y si el jugador la tiene
            if (requiresKey)
            {
                if (!HasRequiredKey())
                {
                    LogDebug($"Cannot open door - missing key: {keyType}");
                    return false;
                }
            }

            // Verificar si la puerta ya está abierta
            if (IsDoorOpen())
            {
                LogDebug("Door is already open");
                return false;
            }

            return true;
        }

        public override string Description => $"Abrir puerta{(requiresKey ? $" (requiere {keyType})" : "")}";

        #region Métodos Privados

        private bool IsDoorOpen()
        {
            if (doorComponent != null)
            {
                return doorComponent.IsOpen;
            }

            // Fallback: verificar animación o estado del objeto
            // Esto dependerá de la implementación específica de la puerta
            return targetObject.transform.rotation.eulerAngles.y > 45f;
        }

        private void OpenDoor()
        {
            if (doorComponent != null)
            {
                doorComponent.Open();
            }
            else
            {
                // Fallback: rotar la puerta manualmente
                targetObject.transform.Rotate(Vector3.up, 90f);
            }

            LogDebug("Door opened successfully");
        }

        private void CloseDoor()
        {
            if (doorComponent != null)
            {
                doorComponent.Close();
            }
            else
            {
                // Fallback: rotar la puerta de vuelta
                targetObject.transform.Rotate(Vector3.up, -90f);
            }

            LogDebug("Door closed");
        }

        private bool HasRequiredKey()
        {
            // Verificar con el sistema de llaves globales
            // Esto debería integrarse con el sistema de inventario/llaves
            switch (keyType.ToLower())
            {
                case "red":
                    return RetroFPS.GameObservers.RedKeyObtained.GetValue();
                case "blue":
                    return RetroFPS.GameObservers.BlueKeyObtained.GetValue();
                case "yellow":
                    return RetroFPS.GameObservers.YellowKeyObtained.GetValue();
                default:
                    LogDebug($"Unknown key type: {keyType}");
                    return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Interface para componentes de puerta
    /// </summary>
    public interface IDoor
    {
        bool IsOpen { get; }
        void Open();
        void Close();
        bool RequiresKey { get; }
        string KeyType { get; }
    }
}
