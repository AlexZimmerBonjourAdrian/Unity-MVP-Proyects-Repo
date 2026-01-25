using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Componente simple para items usables en el mundo.
    /// Al interactuar, se agrega al inventario del jugador.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class UsableItem : MonoBehaviour, Iinteract
    {
        [Header("Item Data")]
        [Tooltip("Datos del item (crear desde el menú de Unity)")]
        [SerializeField] private UsableItemDataSO itemData;

        [Tooltip("Cantidad a agregar al inventario al recoger")]
        [SerializeField] private int quantity = 1;

        [Header("Configuración")]
        [Tooltip("Si es true, el objeto se desactiva después de ser recogido")]
        [SerializeField] private bool disableAfterPickup = true;

        [Tooltip("Si es true, solo se puede recoger una vez")]
        [SerializeField] private bool oneTimeUse = true;

        private bool hasBeenPickedUp = false;

        /// <summary>
        /// Se llama cuando el jugador interactúa con el item
        /// </summary>
        public void Oninteract()
        {
            if (oneTimeUse && hasBeenPickedUp)
            {
                return;
            }

            if (itemData == null)
            {
                Debug.LogWarning($"UsableItem en '{gameObject.name}' no tiene itemData asignado");
                return;
            }

            if (InventoryManager.Instance == null)
            {
                Debug.LogWarning("InventoryManager no está disponible en la escena");
                return;
            }

            // Intentar agregar al inventario
            if (InventoryManager.Instance.AddItem(itemData, quantity))
            {
                hasBeenPickedUp = true;

                // Reproducir sonido si está configurado (usando sistema de eventos)
                CGameEvents.OnPlaySound.Publish(0); // Ejemplo, ajustar según sistema de audio

                // Desactivar objeto si está configurado
                if (disableAfterPickup)
                {
                    gameObject.SetActive(false);
                }

                Debug.Log($"Item recogido: {itemData.itemName} x{quantity}");
            }
            else
            {
                Debug.LogWarning($"No se pudo agregar {itemData.itemName} al inventario (inventario lleno)");
            }
        }

        /// <summary>
        /// Establece los datos del item (útil para configuración dinámica)
        /// </summary>
        public void SetItemData(UsableItemDataSO data)
        {
            itemData = data;
        }

        /// <summary>
        /// Establece la cantidad a recoger
        /// </summary>
        public void SetQuantity(int qty)
        {
            quantity = qty;
        }

        /// <summary>
        /// Reinicia el estado de recogido (útil para testing)
        /// </summary>
        public void ResetPickup()
        {
            hasBeenPickedUp = false;
            if (disableAfterPickup)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
