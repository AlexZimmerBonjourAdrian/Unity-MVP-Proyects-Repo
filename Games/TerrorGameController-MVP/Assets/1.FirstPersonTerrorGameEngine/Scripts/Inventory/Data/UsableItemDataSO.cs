using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// ScriptableObject minimalista para configurar items usables.
    /// Solo contiene los datos esenciales sin complejidad adicional.
    /// </summary>
    [CreateAssetMenu(fileName = "NewUsableItem", menuName = "HorrorEngine/Inventory/Usable Item Data", order = 1)]
    public class UsableItemDataSO : ScriptableObject
    {
        [Header("Información Básica")]
        [Tooltip("Nombre único del item")]
        public string itemName = "New Item";

        [TextArea(2, 4)]
        [Tooltip("Descripción del item (opcional)")]
        public string description = "";

        [Tooltip("Icono del item (opcional, para UI futura)")]
        public Sprite itemIcon;

        [Header("Configuración")]
        [Tooltip("Si es true, el item se consume al usarlo")]
        public bool isConsumable = false;

        [Tooltip("Cantidad máxima que se puede apilar (1 = único, >1 = apilable)")]
        [Range(1, 99)]
        public int maxStackSize = 1;

        /// <summary>
        /// Valida que la configuración del item sea correcta
        /// </summary>
        public bool Validate()
        {
            if (string.IsNullOrEmpty(itemName))
            {
                Debug.LogWarning($"UsableItemDataSO '{name}' tiene un itemName vacío");
                return false;
            }

            if (maxStackSize < 1)
            {
                Debug.LogWarning($"UsableItemDataSO '{name}' tiene maxStackSize < 1");
                return false;
            }

            return true;
        }
    }
}
