using ECS.Core;

namespace ECS.Components.Save
{
    public class SaveComponent : IComponent
    {
        public bool NeedsSave { get; set; }
        public string SaveData { get; set; }
        public int SaveSlot { get; set; }

        public SaveComponent()
        {
            NeedsSave = false;
            SaveData = string.Empty;
            SaveSlot = 0;
        }
    }
}

