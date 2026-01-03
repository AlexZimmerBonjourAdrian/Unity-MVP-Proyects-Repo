using UnityEngine;
using RolEngine;

namespace RolEngine.Examples
{
    /// <summary>
    /// Ejemplo de cómo integrar el sistema de rol con otros sistemas.
    /// Demuestra el uso de eventos, observers y la interfaz IStatSystem.
    /// </summary>
    public class StatSystemIntegrationExample : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private UnityEngine.UI.Text sanityText;
        [SerializeField] private UnityEngine.UI.Text charmText;

        private void Start()
        {
            // Ejemplo 1: Suscribirse a eventos del sistema
            CMICILSPSystem.Instance.OnStatChanged += OnStatChanged;
            CMICILSPSystem.Instance.OnTemplateApplied += OnTemplateApplied;

            // Ejemplo 2: Usar Observers para UI reactiva
            StatObservers.SanityChanged.Attach(UpdateSanityUI);
            StatObservers.CharmChanged.Attach(UpdateCharmUI);
            StatObservers.TemplateChanged.Attach(OnTemplateChanged);

            // Ejemplo 3: Usar la interfaz IStatSystem (desacoplado)
            IStatSystem<CMICILSPSystem.Stats> statSystem = CMICILSPSystem.Instance;
            
            // Verificar requisitos
            if (statSystem.CheckStatRequirement(CMICILSPSystem.Stats.Charm, 7))
            {
                Debug.Log("Puedes usar la opción de diálogo de carisma");
            }
        }

        private void OnDestroy()
        {
            // IMPORTANTE: Desuscribirse para evitar memory leaks
            if (CMICILSPSystem.Instance != null)
            {
                CMICILSPSystem.Instance.OnStatChanged -= OnStatChanged;
                CMICILSPSystem.Instance.OnTemplateApplied -= OnTemplateApplied;
            }

            StatObservers.SanityChanged.Detach(UpdateSanityUI);
            StatObservers.CharmChanged.Detach(UpdateCharmUI);
            StatObservers.TemplateChanged.Detach(OnTemplateChanged);
        }

        // Ejemplo de uso de eventos
        private void OnStatChanged(StatChangedEvent evt)
        {
            Debug.Log($"Stat {evt.StatName} cambió: {evt.OldValue} -> {evt.NewValue} (cambio: {evt.ChangeAmount})");
            
            // Reaccionar a cambios específicos
            if (evt.StatName == "Sanity" && evt.NewValue <= 3)
            {
                Debug.LogWarning("¡Sanity crítica! El personaje está en peligro");
                // Activar efectos visuales, sonidos, etc.
            }
        }

        private void OnTemplateApplied(TemplateAppliedEvent evt)
        {
            Debug.Log($"Template aplicado: {evt.TemplateName}");
            // Actualizar UI, mostrar mensaje, etc.
        }

        // Ejemplo de uso de observers para UI
        private void UpdateSanityUI(int newValue)
        {
            if (sanityText != null)
            {
                sanityText.text = $"Sanity: {newValue}";
            }
        }

        private void UpdateCharmUI(int newValue)
        {
            if (charmText != null)
            {
                charmText.text = $"Charm: {newValue}";
            }
        }

        private void OnTemplateChanged(string templateName)
        {
            Debug.Log($"Template cambió a: {templateName}");
        }

        // Ejemplo de integración con sistema de diálogos
        public bool CanUseCharmDialogueOption()
        {
            return CMICILSPSystem.Instance.CheckStatRequirement(
                CMICILSPSystem.Stats.Charm, 7
            );
        }

        // Ejemplo de integración con sistema de eventos del juego
        public void HandleGameEvent(string eventType)
        {
            switch (eventType)
            {
                case "Trauma":
                    CMICILSPSystem.Instance.DecreaseStat(CMICILSPSystem.Stats.Sanity, 2);
                    CMICILSPSystem.Instance.DecreaseStat(CMICILSPSystem.Stats.Composure, 1);
                    break;

                case "SocialSuccess":
                    CMICILSPSystem.Instance.IncreaseStat(CMICILSPSystem.Stats.Charm, 1);
                    CMICILSPSystem.Instance.IncreaseStat(CMICILSPSystem.Stats.Empathy, 1);
                    break;
            }
        }
    }
}

