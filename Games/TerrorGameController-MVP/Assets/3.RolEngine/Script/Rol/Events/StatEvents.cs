using System;

namespace RolEngine
{
    /// <summary>
    /// Eventos del sistema de rol para integración con otros sistemas.
    /// Permite comunicación desacoplada mediante eventos.
    /// </summary>
    
    /// <summary>
    /// Evento que se dispara cuando una estadística cambia
    /// </summary>
    public class StatChangedEvent
    {
        public string StatName { get; set; }
        public int OldValue { get; set; }
        public int NewValue { get; set; }
        public int ChangeAmount { get; set; }

        public StatChangedEvent(string statName, int oldValue, int newValue)
        {
            StatName = statName;
            OldValue = oldValue;
            NewValue = newValue;
            ChangeAmount = newValue - oldValue;
        }
    }

    /// <summary>
    /// Evento que se dispara cuando se aplica un template
    /// </summary>
    public class TemplateAppliedEvent
    {
        public string TemplateName { get; set; }
        public System.Collections.Generic.Dictionary<string, int> NewStats { get; set; }

        public TemplateAppliedEvent(string templateName, System.Collections.Generic.Dictionary<string, int> newStats)
        {
            TemplateName = templateName;
            NewStats = newStats;
        }
    }

    /// <summary>
    /// Evento que se dispara cuando se verifica un requisito de stat
    /// </summary>
    public class StatRequirementCheckEvent
    {
        public string StatName { get; set; }
        public int RequiredValue { get; set; }
        public int CurrentValue { get; set; }
        public bool Passed { get; set; }

        public StatRequirementCheckEvent(string statName, int requiredValue, int currentValue, bool passed)
        {
            StatName = statName;
            RequiredValue = requiredValue;
            CurrentValue = currentValue;
            Passed = passed;
        }
    }
}

