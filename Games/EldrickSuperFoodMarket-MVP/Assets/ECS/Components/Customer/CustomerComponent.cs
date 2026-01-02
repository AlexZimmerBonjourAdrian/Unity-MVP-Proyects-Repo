using ECS.Core;

namespace ECS.Components.Customer
{
    public enum CustomerType
    {
        Lovecraftian,
        Angel,
        Demon
    }

    public enum PersonalityType
    {
        Friendly,
        Sarcastic,
        Dry
    }

    public class CustomerComponent : IComponent
    {
        public CustomerType Type { get; set; }
        public PersonalityType Personality { get; set; }
        public string Name { get; set; }
        public string OrderDescription { get; set; }
        public bool HasBeenServed { get; set; }
        public bool WillGiveTip { get; set; }

        public CustomerComponent()
        {
            HasBeenServed = false;
            WillGiveTip = false;
        }
    }
}

