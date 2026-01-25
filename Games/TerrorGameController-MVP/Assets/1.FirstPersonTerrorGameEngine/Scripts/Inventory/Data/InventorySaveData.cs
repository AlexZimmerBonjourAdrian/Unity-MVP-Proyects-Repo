using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Datos serializables para guardar el estado del inventario.
    /// Almacena la información de todos los slots del inventario.
    /// </summary>
    [System.Serializable]
    public class InventorySaveData
    {
        [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

        public InventorySaveData()
        {
            slots = new List<InventorySlot>();
            // Inicializar con 12 slots vacíos
            for (int i = 0; i < 12; i++)
            {
                slots.Add(new InventorySlot());
            }
        }

        /// <summary>
        /// Obtiene un slot por índice
        /// </summary>
        public InventorySlot GetSlot(int index)
        {
            if (index >= 0 && index < slots.Count)
            {
                return slots[index];
            }
            return null;
        }

        /// <summary>
        /// Establece un slot por índice
        /// </summary>
        public void SetSlot(int index, InventorySlot slot)
        {
            if (index >= 0 && index < slots.Count)
            {
                slots[index] = slot;
            }
        }

        /// <summary>
        /// Obtiene todos los slots
        /// </summary>
        public List<InventorySlot> GetAllSlots()
        {
            return slots;
        }

        /// <summary>
        /// Asegura que hay exactamente 12 slots
        /// </summary>
        public void EnsureSlotCount(int count = 12)
        {
            while (slots.Count < count)
            {
                slots.Add(new InventorySlot());
            }
            while (slots.Count > count)
            {
                slots.RemoveAt(slots.Count - 1);
            }
        }
    }
}
