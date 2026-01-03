using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Decorator Pattern - Clase base para todos los items usando ScriptableObjects.
    /// Esta clase puede ser decorada con diferentes modificadores (daño, encantamientos, etc.).
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Retro FPS/Items/Base Item", order = 1)]
    public class BaseItem : ScriptableObject, IItem
    {
        [Header("Basic Information")]
        [SerializeField] protected string itemName = "New Item";
        [SerializeField] protected string itemDescription = "Item description";
        [SerializeField] protected Sprite itemIcon;

        [Header("Item Properties")]
        [SerializeField] protected ItemType itemType = ItemType.Generic;
        [SerializeField] protected ItemRarity rarity = ItemRarity.Common;
        [SerializeField] protected bool isStackable = false;
        [SerializeField] protected int maxStackSize = 1;

        [Header("Audio")]
        [SerializeField] protected AudioClip useSound;
        [SerializeField] protected AudioClip equipSound;

        #region IItem Implementation

        public virtual string Name => itemName;
        public virtual string Description => itemDescription;
        public virtual Sprite Icon => itemIcon;

        public virtual void Use()
        {
            LogDebug($"Using item: {Name}");

            // Reproducir sonido de uso
            PlayUseSound();

            // Lógica específica de uso (sobrescribir en subclases)
            OnUse();
        }

        public virtual void Equip()
        {
            LogDebug($"Equipping item: {Name}");

            // Reproducir sonido de equipar
            PlayEquipSound();

            // Lógica específica de equipar (sobrescribir en subclases)
            OnEquip();
        }

        public virtual void Unequip()
        {
            LogDebug($"Unequipping item: {Name}");

            // Lógica específica de desequipar (sobrescribir en subclases)
            OnUnequip();
        }

        public virtual IItem Clone()
        {
            // Crear una instancia nueva del ScriptableObject
            BaseItem clonedItem = Instantiate(this);

            // Copiar valores serializados
            clonedItem.itemName = this.itemName;
            clonedItem.itemDescription = this.itemDescription;
            clonedItem.itemIcon = this.itemIcon;

            LogDebug($"Item cloned: {clonedItem.Name}");
            return clonedItem;
        }

        #endregion

        #region Virtual Methods for Subclassing

        /// <summary>
        /// Lógica específica cuando se usa el item
        /// </summary>
        protected virtual void OnUse()
        {
            // Implementación base vacía - sobrescribir en subclases
        }

        /// <summary>
        /// Lógica específica cuando se equipa el item
        /// </summary>
        protected virtual void OnEquip()
        {
            // Implementación base vacía - sobrescribir en subclases
        }

        /// <summary>
        /// Lógica específica cuando se desequipa el item
        /// </summary>
        protected virtual void OnUnequip()
        {
            // Implementación base vacía - sobrescribir en subclases
        }

        #endregion

        #region Audio Methods

        protected virtual void PlayUseSound()
        {
            if (useSound != null)
            {
                // TODO: Integrar con sistema de audio
                // AudioManager.PlayOneShot(useSound);
                LogDebug($"Playing use sound: {useSound.name}");
            }
        }

        protected virtual void PlayEquipSound()
        {
            if (equipSound != null)
            {
                // TODO: Integrar con sistema de audio
                // AudioManager.PlayOneShot(equipSound);
                LogDebug($"Playing equip sound: {equipSound.name}");
            }
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Verifica si el item puede ser apilado
        /// </summary>
        public bool IsStackable => isStackable;

        /// <summary>
        /// Tamaño máximo de pila
        /// </summary>
        public int MaxStackSize => maxStackSize;

        /// <summary>
        /// Tipo del item
        /// </summary>
        public ItemType Type => itemType;

        /// <summary>
        /// Rareza del item
        /// </summary>
        public ItemRarity Rarity => rarity;

        /// <summary>
        /// Obtiene información detallada del item para debug
        /// </summary>
        public virtual string GetDetailedInfo()
        {
            return $"Item: {Name}\n" +
                   $"Type: {Type}\n" +
                   $"Rarity: {Rarity}\n" +
                   $"Stackable: {IsStackable}\n" +
                   $"Max Stack: {MaxStackSize}\n" +
                   $"Description: {Description}";
        }

        /// <summary>
        /// Compara dos items para verificar si son del mismo tipo
        /// </summary>
        public virtual bool IsSameType(IItem other)
        {
            return other != null && Name == other.Name && Type == (other as BaseItem)?.Type;
        }

        /// <summary>
        /// Método para validar el item en el editor
        /// </summary>
        protected virtual void OnValidate()
        {
            // Validaciones básicas
            if (string.IsNullOrEmpty(itemName))
            {
                itemName = "Unnamed Item";
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }

            if (maxStackSize < 1)
            {
                maxStackSize = 1;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(this);
#endif
            }
        }

        #endregion

        #region Private Methods

        private void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[BaseItem: {Name}] {message}");
#endif
        }

        #endregion
    }
}
