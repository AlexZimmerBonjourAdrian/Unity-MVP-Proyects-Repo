namespace HorrorEngine
{
    /// <summary>
    /// Datos del evento de item recogido
    /// </summary>
    public class ItemPickedUpEventData
    {
        public string itemName;
        public int quantity;

        public ItemPickedUpEventData(string name, int qty)
        {
            itemName = name;
            quantity = qty;
        }
    }

    /// <summary>
    /// Datos del evento de item usado
    /// </summary>
    public class ItemUsedEventData
    {
        public string itemName;
        public int slotIndex;

        public ItemUsedEventData(string name, int slot)
        {
            itemName = name;
            slotIndex = slot;
        }
    }

    /// <summary>
    /// Datos del evento de item removido
    /// </summary>
    public class ItemRemovedEventData
    {
        public string itemName;
        public int quantity;

        public ItemRemovedEventData(string name, int qty)
        {
            itemName = name;
            quantity = qty;
        }
    }
}
