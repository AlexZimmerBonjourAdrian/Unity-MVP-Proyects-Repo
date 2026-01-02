using System.Collections.Generic;
using ECS.Core;

namespace ECS.Components.Order
{
    public enum OrderComplexity
    {
        Simple,
        Medium,
        Complex
    }

    public class OrderComponent : IComponent
    {
        public string OrderDescription { get; set; }
        public OrderComplexity Complexity { get; set; }
        public List<string> RequiredComponents { get; set; }
        public List<string> SelectedComponents { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCorrect { get; set; }

        public OrderComponent()
        {
            RequiredComponents = new List<string>();
            SelectedComponents = new List<string>();
            IsCompleted = false;
            IsCorrect = false;
        }
    }
}

