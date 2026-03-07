using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Slot individual del inventario.
    /// Almacena una referencia a un item y su cantidad.
    /// </summary>
    [System.Serializable]
    public class InventorySlot
    {
        [SerializeField] private UsableItemDataSO itemData;
        [SerializeField] private int quantity = 0;

        public UsableItemDataSO ItemData => itemData;
        public int Quantity => quantity;

        public InventorySlot()
        {
            itemData = null;
            quantity = 0;
        }

        public InventorySlot(UsableItemDataSO data, int qty)
        {
            itemData = data;
            quantity = qty;
        }

        /// <summary>
        /// Verifica si el slot está vacío
        /// </summary>
        public bool IsEmpty()
        {
            return itemData == null || quantity <= 0;
        }

        /// <summary>
        /// Limpia el slot
        /// </summary>
        public void Clear()
        {
            itemData = null;
            quantity = 0;
        }

        /// <summary>
        /// Establece el item y cantidad del slot
        /// </summary>
        public void SetItem(UsableItemDataSO data, int qty)
        {
            itemData = data;
            quantity = qty;
        }

        /// <summary>
        /// Agrega cantidad al slot
        /// </summary>
        public void AddQuantity(int amount)
        {
            quantity += amount;
        }

        /// <summary>
        /// Remueve cantidad del slot
        /// </summary>
        public void RemoveQuantity(int amount)
        {
            quantity = Mathf.Max(0, quantity - amount);
            if (quantity <= 0)
            {
                Clear();
            }
        }

        /// <summary>
        /// Verifica si el slot puede agregar más cantidad de este item
        /// </summary>
        public bool CanAddMore(int amount)
        {
            if (itemData == null) return false;
            return (quantity + amount) <= itemData.maxStackSize;
        }

        /// <summary>
        /// Obtiene el espacio disponible en el slot
        /// </summary>
        public int GetAvailableSpace()
        {
            if (itemData == null) return 0;
            return Mathf.Max(0, itemData.maxStackSize - quantity);
        }
    }
}
