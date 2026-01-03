using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Decorator Pattern - Interface base para todos los items del juego.
    /// Define el contrato que deben cumplir todos los items, permitiendo su decoración.
    /// </summary>
    public interface IItem
    {
        /// <summary>
        /// Nombre del item (puede ser modificado por decorators)
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Descripción del item (puede ser modificada por decorators)
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Icono/sprite del item (puede ser modificado por decorators)
        /// </summary>
        Sprite Icon { get; }

        /// <summary>
        /// Usa el item (acción principal)
        /// </summary>
        void Use();

        /// <summary>
        /// Equipa el item (si es equipable)
        /// </summary>
        void Equip();

        /// <summary>
        /// Desequipa el item (si está equipado)
        /// </summary>
        void Unequip();

        /// <summary>
        /// Crea una copia/clone del item
        /// </summary>
        /// <returns>Copia del item</returns>
        IItem Clone();
    }

    /// <summary>
    /// Tipos comunes de items en juegos FPS retro
    /// </summary>
    public enum ItemType
    {
        Weapon,
        Ammo,
        Health,
        Armor,
        Key,
        PowerUp,
        Collectible,
        Tool,
        Generic
    }

    /// <summary>
    /// Calidad/rareza de los items
    /// </summary>
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    /// <summary>
    /// Extensión para facilitar el trabajo con items
    /// </summary>
    public static class ItemExtensions
    {
        /// <summary>
        /// Verifica si un item es equipable
        /// </summary>
        public static bool IsEquippable(this IItem item)
        {
            // Por defecto, asumimos que los items son equipables
            // Esto puede ser sobrescrito en implementaciones específicas
            return item is IEquippableItem;
        }

        /// <summary>
        /// Verifica si un item puede ser usado
        /// </summary>
        public static bool CanBeUsed(this IItem item)
        {
            // Por defecto, asumimos que todos los items pueden ser usados
            // Esto puede ser sobrescrito en implementaciones específicas
            return true;
        }

        /// <summary>
        /// Obtiene información de debug del item
        /// </summary>
        public static string GetDebugInfo(this IItem item)
        {
            return $"{item.GetType().Name}: {item.Name}\n" +
                   $"- Description: {item.Description}\n" +
                   $"- Has Icon: {item.Icon != null}\n" +
                   $"- Equippable: {item.IsEquippable()}";
        }
    }

    /// <summary>
    /// Interface opcional para items equipables
    /// </summary>
    public interface IEquippableItem
    {
        bool IsEquipped { get; }
        void OnEquipped();
        void OnUnequipped();
    }

    /// <summary>
    /// Interface opcional para items consumibles
    /// </summary>
    public interface IConsumableItem
    {
        int MaxStack { get; }
        int CurrentStack { get; }
        bool Consume();
    }

    /// <summary>
    /// Interface opcional para items con durabilidad
    /// </summary>
    public interface IDurableItem
    {
        int MaxDurability { get; }
        int CurrentDurability { get; }
        bool Repair(int amount);
        bool IsBroken { get; }
    }
}
