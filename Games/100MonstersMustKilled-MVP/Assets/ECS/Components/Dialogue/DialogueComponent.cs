using System.Collections.Generic;
using ECS.Core;

namespace ECS.Components.Dialogue
{
    public class DialogueOption
    {
        public string Text { get; set; }
        public int NextDialogueId { get; set; }
    }

    public class DialogueComponent : IComponent
    {
        public string Name { get; set; }
        public string InitialText { get; set; }
        public List<DialogueOption> Options { get; set; }
        public bool HasSpoken { get; set; }
        public bool WillDefend { get; set; }
        public bool WillSurrender { get; set; }

        public DialogueComponent(string name, string initialText)
        {
            Name = name;
            InitialText = initialText;
            Options = new List<DialogueOption>();
            HasSpoken = false;
            WillDefend = false;
            WillSurrender = false;
        }
    }
}

