using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Comando para recoger items interactivos
    /// </summary>
    public class PickupItemCommand : InteractableCommand
    {
        private IItem item;
        private bool itemWasAddedToInventory = false;

        /// <summary>
        /// Constructor para comando de recoger item
        /// </summary>
        /// <param name="itemObject">Objeto item en el mundo</param>
        /// <param name="itemData">Datos del item</param>
        public PickupItemCommand(GameObject itemObject, IItem itemData)
            : base(itemObject)
        {
            item = itemData;
        }

        public override void Execute()
        {
            if (!CanExecute())
            {
                LogDebug("Cannot execute - conditions not met");
                return;
            }

            LogDebug($"Executing PickupItem command for {item.Name}");

            // Intentar agregar el item al inventario
            if (AddItemToInventory())
            {
                // Ocultar/destruir el objeto del mundo
                HideItemObject();

                // Publicar evento
                var pickupEvent = new PlayerItemCollectedEvent(
                    item.Name,
                    item.GetType().Name,
                    interactionPosition
                );
                EventBus.Publish(pickupEvent);

                // Actualizar observers globales
                RetroFPS.GameObservers.InventoryItemsChanged.ModifyValue(count => count + 1);

                // Si es una llave, actualizar observers específicos
                UpdateKeyObservers();

                MarkAsExecuted();
            }
            else
            {
                LogDebug("Failed to add item to inventory");
            }
        }

        public override void Undo()
        {
            if (!hasBeenExecuted)
            {
                LogDebug("Cannot undo - command was not executed");
                return;
            }

            LogDebug($"Undoing PickupItem command for {item.Name}");

            // Remover item del inventario
            if (itemWasAddedToInventory)
            {
                RemoveItemFromInventory();
                RetroFPS.GameObservers.InventoryItemsChanged.ModifyValue(count => count - 1);
            }

            // Mostrar el objeto nuevamente
            ShowItemObject();

            // Revertir cambios en observers de llaves
            RevertKeyObservers();

            MarkAsUndone();
        }

        public override bool CanExecute()
        {
            // Verificaciones base
            if (!base.CanExecute())
                return false;

            // Verificar que tenemos datos del item
            if (item == null)
            {
                LogDebug("Item data is null");
                return false;
            }

            // Verificar que hay espacio en el inventario
            if (!HasInventorySpace())
            {
                LogDebug("No inventory space available");
                return false;
            }

            return true;
        }

        public override string Description => $"Recoger {item?.Name ?? "item"}";

        #region Métodos Privados

        private bool AddItemToInventory()
        {
            // Aquí debería integrarse con el sistema de inventario real
            // Por ahora, simulamos la adición
            try
            {
                // TODO: Integrar con IInventoryManager cuando esté implementado
                // inventoryManager.AddItem(item);

                LogDebug($"Item {item.Name} added to inventory");
                itemWasAddedToInventory = true;
                return true;
            }
            catch (System.Exception ex)
            {
                LogDebug($"Failed to add item to inventory: {ex.Message}");
                return false;
            }
        }

        private void RemoveItemFromInventory()
        {
            try
            {
                // TODO: Integrar con IInventoryManager cuando esté implementado
                // inventoryManager.RemoveItem(item);

                LogDebug($"Item {item.Name} removed from inventory");
                itemWasAddedToInventory = false;
            }
            catch (System.Exception ex)
            {
                LogDebug($"Failed to remove item from inventory: {ex.Message}");
            }
        }

        private bool HasInventorySpace()
        {
            // TODO: Verificar espacio real en inventario
            // Por ahora, asumimos que hay espacio
            return true;
        }

        private void HideItemObject()
        {
            if (targetObject != null)
            {
                // Ocultar el objeto (podría destruirse o desactivarse)
                targetObject.SetActive(false);

                // Alternativa: destruir después de un delay
                // Object.Destroy(targetObject, 0.1f);

                LogDebug("Item object hidden from world");
            }
        }

        private void ShowItemObject()
        {
            if (targetObject != null)
            {
                targetObject.SetActive(true);
                LogDebug("Item object shown in world");
            }
        }

        private void UpdateKeyObservers()
        {
            // Actualizar observers específicos si es una llave
            string itemName = item.Name.ToLower();
            if (itemName.Contains("red key") || itemName.Contains("llave roja"))
            {
                RetroFPS.GameObservers.RedKeyObtained.SetValue(true);
            }
            else if (itemName.Contains("blue key") || itemName.Contains("llave azul"))
            {
                RetroFPS.GameObservers.BlueKeyObtained.SetValue(true);
            }
            else if (itemName.Contains("yellow key") || itemName.Contains("llave amarilla"))
            {
                RetroFPS.GameObservers.YellowKeyObtained.SetValue(true);
            }
        }

        private void RevertKeyObservers()
        {
            // Revertir cambios en observers de llaves
            string itemName = item.Name.ToLower();
            if (itemName.Contains("red key") || itemName.Contains("llave roja"))
            {
                RetroFPS.GameObservers.RedKeyObtained.SetValue(false);
            }
            else if (itemName.Contains("blue key") || itemName.Contains("llave azul"))
            {
                RetroFPS.GameObservers.BlueKeyObtained.SetValue(false);
            }
            else if (itemName.Contains("yellow key") || itemName.Contains("llave amarilla"))
            {
                RetroFPS.GameObservers.YellowKeyObtained.SetValue(false);
            }
        }

        #endregion
    }
}
