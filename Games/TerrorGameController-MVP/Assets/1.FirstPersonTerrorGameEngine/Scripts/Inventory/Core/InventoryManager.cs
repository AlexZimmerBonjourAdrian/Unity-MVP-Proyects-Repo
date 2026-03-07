using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Manager singleton que gestiona el inventario del jugador.
    /// Sistema simple con 12 slots fijos.
    /// </summary>
    public class InventoryManager : MonoBehaviour, IDataPersistence
    {
        public static InventoryManager Instance { get; private set; }

        [Header("Configuración")]
        [Tooltip("Número de slots del inventario")]
        [SerializeField] private int inventorySize = 12;

        [Header("Debug")]
        [SerializeField] private bool enableDebugLogs = false;

        private InventorySlot[] slots;
        private InventorySaveData saveData = new InventorySaveData();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("Multiple instances of InventoryManager detected. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Inicializar slots
            slots = new InventorySlot[inventorySize];
            for (int i = 0; i < inventorySize; i++)
            {
                slots[i] = new InventorySlot();
            }
        }

        private void Update()
        {
            // Manejar input de teclas numéricas para usar items (1-9)
            HandleNumberKeyInput();
        }

        /// <summary>
        /// Maneja el input de teclas numéricas para usar items
        /// </summary>
        private void HandleNumberKeyInput()
        {
            // Teclas 1-9 corresponden a slots 0-8
            for (int i = 0; i < 9; i++)
            {
                KeyCode key = KeyCode.Alpha1 + i;
                if (Input.GetKeyDown(key))
                {
                    UseItem(i);
                }
            }
        }

        /// <summary>
        /// Agrega un item al inventario
        /// </summary>
        /// <param name="itemData">Datos del item a agregar</param>
        /// <param name="quantity">Cantidad a agregar</param>
        /// <returns>True si se agregó exitosamente</returns>
        public bool AddItem(UsableItemDataSO itemData, int quantity = 1)
        {
            if (itemData == null)
            {
                LogDebug("Intento de agregar item null al inventario");
                return false;
            }

            if (quantity <= 0)
            {
                LogDebug($"Intento de agregar cantidad inválida: {quantity}");
                return false;
            }

            // Si el item es apilable, buscar slot existente con espacio
            if (itemData.maxStackSize > 1)
            {
                for (int i = 0; i < slots.Length; i++)
                {
                    if (!slots[i].IsEmpty() && slots[i].ItemData.itemName == itemData.itemName)
                    {
                        int availableSpace = slots[i].GetAvailableSpace();
                        if (availableSpace > 0)
                        {
                            int amountToAdd = Mathf.Min(quantity, availableSpace);
                            slots[i].AddQuantity(amountToAdd);
                            quantity -= amountToAdd;

                            LogDebug($"Agregado {amountToAdd} de {itemData.itemName} al slot {i}");

                            if (quantity <= 0)
                            {
                                var eventData = new ItemPickedUpEventData(itemData.itemName, amountToAdd);
                                CGameEvents.OnItemPickedUp.Publish(eventData);
                                return true;
                            }
                        }
                    }
                }
            }

            int totalAdded = 0;

            // Buscar slot vacío para el resto
            while (quantity > 0)
            {
                int emptySlotIndex = FindEmptySlot();
                if (emptySlotIndex == -1)
                {
                    LogDebug("Inventario lleno, no se puede agregar más items");
                    if (totalAdded > 0)
                    {
                        var eventData = new ItemPickedUpEventData(itemData.itemName, totalAdded);
                        CGameEvents.OnItemPickedUp.Publish(eventData);
                    }
                    return totalAdded > 0;
                }

                int amountToAdd = Mathf.Min(quantity, itemData.maxStackSize);
                slots[emptySlotIndex].SetItem(itemData, amountToAdd);
                quantity -= amountToAdd;
                totalAdded += amountToAdd;

                LogDebug($"Agregado {amountToAdd} de {itemData.itemName} al slot {emptySlotIndex}");
            }

            if (totalAdded > 0)
            {
                var eventData = new ItemPickedUpEventData(itemData.itemName, totalAdded);
                CGameEvents.OnItemPickedUp.Publish(eventData);
            }
            return true;
        }

        /// <summary>
        /// Remueve cantidad de un item del inventario
        /// </summary>
        public bool RemoveItem(int slotIndex, int amount = 1)
        {
            if (slotIndex < 0 || slotIndex >= slots.Length)
            {
                return false;
            }

            if (slots[slotIndex].IsEmpty())
            {
                return false;
            }

            string itemName = slots[slotIndex].ItemData.itemName;
            slots[slotIndex].RemoveQuantity(amount);

            var eventData = new ItemRemovedEventData(itemName, amount);
            CGameEvents.OnItemRemoved.Publish(eventData);
            LogDebug($"Removido {amount} de {itemName} del slot {slotIndex}");

            return true;
        }

        /// <summary>
        /// Usa el item en el slot especificado
        /// </summary>
        public bool UseItem(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= slots.Length)
            {
                return false;
            }

            if (slots[slotIndex].IsEmpty())
            {
                return false;
            }

            var itemData = slots[slotIndex].ItemData;

            // Crear instancia temporal del item y usar
            // Por ahora, solo llamamos a un método Use genérico
            // Esto se puede extender para crear instancias específicas según el tipo de item
            UseItemInternal(itemData);

            // Si es consumible, reducir cantidad
            if (itemData.isConsumable)
            {
                slots[slotIndex].RemoveQuantity(1);
            }

            var eventData = new ItemUsedEventData(itemData.itemName, slotIndex);
            CGameEvents.OnItemUsed.Publish(eventData);
            LogDebug($"Item usado: {itemData.itemName} del slot {slotIndex}");

            return true;
        }

        /// <summary>
        /// Usa el item internamente (lógica de uso)
        /// </summary>
        private void UseItemInternal(UsableItemDataSO itemData)
        {
            // Aquí se puede agregar lógica específica según el tipo de item
            // Por ahora, solo un log
            LogDebug($"Usando item: {itemData.itemName}");

            // Ejemplo: si el item tiene un nombre específico, hacer algo
            // Esto se puede extender con un sistema de efectos o usando el sistema de tareas
            if (itemData.itemName.Contains("Health") || itemData.itemName.Contains("Potion"))
            {
                // Ejemplo: curar al jugador
                if (Player.Instance != null)
                {
                    Player.Instance.Heal(25); // Ejemplo de curación
                }
            }
        }

        /// <summary>
        /// Verifica si el inventario tiene un item específico
        /// </summary>
        public bool HasItem(string itemName)
        {
            return GetItemCount(itemName) > 0;
        }

        /// <summary>
        /// Obtiene la cantidad total de un item en el inventario
        /// </summary>
        public int GetItemCount(string itemName)
        {
            int total = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                if (!slots[i].IsEmpty() && slots[i].ItemData.itemName == itemName)
                {
                    total += slots[i].Quantity;
                }
            }
            return total;
        }

        /// <summary>
        /// Obtiene el slot en el índice especificado
        /// </summary>
        public InventorySlot GetSlot(int index)
        {
            if (index >= 0 && index < slots.Length)
            {
                return slots[index];
            }
            return null;
        }

        /// <summary>
        /// Obtiene el número de slots del inventario
        /// </summary>
        public int GetInventorySize()
        {
            return slots.Length;
        }

        /// <summary>
        /// Busca el primer slot vacío
        /// </summary>
        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].IsEmpty())
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Limpia todo el inventario
        /// </summary>
        public void ClearInventory()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].Clear();
            }
            LogDebug("Inventario limpiado");
        }

        #region IDataPersistence

        public void LoadData(GameData data)
        {
            if (data.inventoryData != null)
            {
                saveData = data.inventoryData;
                saveData.EnsureSlotCount(inventorySize);

                // Cargar slots desde saveData
                for (int i = 0; i < slots.Length && i < saveData.GetAllSlots().Count; i++)
                {
                    var savedSlot = saveData.GetSlot(i);
                    if (savedSlot != null)
                    {
                        slots[i] = savedSlot;
                    }
                }

                LogDebug("Datos de inventario cargados");
            }
        }

        public void SaveData(GameData data)
        {
            // Guardar slots en saveData
            saveData.EnsureSlotCount(inventorySize);
            for (int i = 0; i < slots.Length; i++)
            {
                saveData.SetSlot(i, slots[i]);
            }

            data.inventoryData = saveData;
            LogDebug("Datos de inventario guardados");
        }

        #endregion

        #region Utilidades

        private void LogDebug(string message)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[InventoryManager] {message}");
            }
        }

        #endregion
    }
}
