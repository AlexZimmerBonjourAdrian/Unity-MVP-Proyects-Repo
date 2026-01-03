using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Comando para activar/desactivar switches/interruptores
    /// </summary>
    public class UseSwitchCommand : InteractableCommand
    {
        private bool wasActivated;
        private ISwitch switchComponent;

        /// <summary>
        /// Constructor para comando de usar switch
        /// </summary>
        /// <param name="switchObject">Objeto switch a activar</param>
        public UseSwitchCommand(GameObject switchObject)
            : base(switchObject)
        {
            // Intentar obtener el componente de switch
            switchComponent = switchObject.GetComponent<ISwitch>();
        }

        public override void Execute()
        {
            if (!CanExecute())
            {
                LogDebug("Cannot execute - conditions not met");
                return;
            }

            LogDebug("Executing UseSwitch command");

            // Guardar estado anterior
            wasActivated = IsSwitchActivated();

            // Activar/desactivar el switch
            ToggleSwitch();

            // Publicar evento
            var switchEvent = new SwitchActivatedEvent(
                targetObject,
                GetSwitchType(),
                IsSwitchActivated()
            );
            EventBus.Publish(switchEvent);

            MarkAsExecuted();
        }

        public override void Undo()
        {
            if (!hasBeenExecuted)
            {
                LogDebug("Cannot undo - command was not executed");
                return;
            }

            LogDebug("Undoing UseSwitch command");

            // Revertir al estado anterior
            SetSwitchState(wasActivated);

            MarkAsUndone();
        }

        public override bool CanExecute()
        {
            // Verificaciones base
            if (!base.CanExecute())
                return false;

            // Verificar si el switch puede ser usado
            if (switchComponent != null)
            {
                return switchComponent.CanBeActivated;
            }

            // Fallback: asumir que puede ser usado
            return true;
        }

        public override string Description => $"Usar switch {targetObject.name}";

        #region Métodos Privados

        private bool IsSwitchActivated()
        {
            if (switchComponent != null)
            {
                return switchComponent.IsActivated;
            }

            // Fallback: verificar estado del objeto
            // Esto dependerá de la implementación visual del switch
            return targetObject.transform.localScale.y < 0.5f; // Ejemplo: switch presionado
        }

        private void ToggleSwitch()
        {
            bool newState = !IsSwitchActivated();
            SetSwitchState(newState);

            LogDebug($"Switch toggled to: {(newState ? "activated" : "deactivated")}");
        }

        private void SetSwitchState(bool activated)
        {
            if (switchComponent != null)
            {
                if (activated)
                    switchComponent.Activate();
                else
                    switchComponent.Deactivate();
            }
            else
            {
                // Fallback: cambiar apariencia visual
                if (activated)
                {
                    // Activar visualmente (ejemplo: presionar el switch)
                    targetObject.transform.localScale = new Vector3(1f, 0.3f, 1f);
                    // Cambiar material/color para mostrar activado
                }
                else
                {
                    // Desactivar visualmente
                    targetObject.transform.localScale = new Vector3(1f, 1f, 1f);
                    // Restaurar material/color original
                }
            }
        }

        private string GetSwitchType()
        {
            if (switchComponent != null)
            {
                return switchComponent.SwitchType;
            }

            // Tipo por defecto
            return "Generic";
        }

        #endregion
    }

    /// <summary>
    /// Interface para componentes de switch/interruptor
    /// </summary>
    public interface ISwitch
    {
        bool IsActivated { get; }
        bool CanBeActivated { get; }
        string SwitchType { get; }

        void Activate();
        void Deactivate();
        void Toggle();
    }

    /// <summary>
    /// Implementación básica de switch que puede heredarse
    /// </summary>
    public class BaseSwitch : MonoBehaviour, ISwitch
    {
        [SerializeField] protected bool isActivated = false;
        [SerializeField] protected bool canBeActivated = true;
        [SerializeField] protected string switchType = "Generic";

        public virtual bool IsActivated => isActivated;
        public virtual bool CanBeActivated => canBeActivated;
        public virtual string SwitchType => switchType;

        public virtual void Activate()
        {
            if (!canBeActivated) return;

            isActivated = true;
            OnActivated();
        }

        public virtual void Deactivate()
        {
            isActivated = false;
            OnDeactivated();
        }

        public virtual void Toggle()
        {
            if (isActivated)
                Deactivate();
            else
                Activate();
        }

        /// <summary>
        /// Método que puede ser sobrescrito para lógica específica al activar
        /// </summary>
        protected virtual void OnActivated()
        {
            // Lógica específica (animaciones, sonidos, etc.)
            Debug.Log($"Switch {gameObject.name} activated");
        }

        /// <summary>
        /// Método que puede ser sobrescrito para lógica específica al desactivar
        /// </summary>
        protected virtual void OnDeactivated()
        {
            // Lógica específica (animaciones, sonidos, etc.)
            Debug.Log($"Switch {gameObject.name} deactivated");
        }
    }
}
