using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Decorator Pattern - Clase base abstracta para decoradores de items.
    /// Permite agregar funcionalidades a items sin modificar su código base.
    /// </summary>
    public abstract class ItemDecorator : IItem
    {
        protected IItem wrappedItem;

        /// <summary>
        /// Constructor que envuelve un item
        /// </summary>
        /// <param name="item">Item a decorar</param>
        protected ItemDecorator(IItem item)
        {
            wrappedItem = item ?? throw new System.ArgumentNullException(nameof(item));
        }

        #region IItem Implementation (Delegation)

        public virtual string Name => wrappedItem.Name;
        public virtual string Description => wrappedItem.Description;
        public virtual Sprite Icon => wrappedItem.Icon;

        public virtual void Use()
        {
            // Aplicar modificaciones antes del uso
            OnBeforeUse();

            // Usar el item decorado
            wrappedItem.Use();

            // Aplicar modificaciones después del uso
            OnAfterUse();
        }

        public virtual void Equip()
        {
            // Aplicar modificaciones antes de equipar
            OnBeforeEquip();

            // Equipar el item decorado
            wrappedItem.Equip();

            // Aplicar modificaciones después de equipar
            OnAfterEquip();
        }

        public virtual void Unequip()
        {
            // Aplicar modificaciones antes de desequipar
            OnBeforeUnequip();

            // Desequipar el item decorado
            wrappedItem.Unequip();

            // Aplicar modificaciones después de desequipar
            OnAfterUnequip();
        }

        public virtual IItem Clone()
        {
            // Crear un nuevo decorator del mismo tipo con el item clonado
            return CreateDecorator(wrappedItem.Clone());
        }

        #endregion

        #region Hook Methods for Subclasses

        /// <summary>
        /// Método hook llamado antes de usar el item
        /// </summary>
        protected virtual void OnBeforeUse() { }

        /// <summary>
        /// Método hook llamado después de usar el item
        /// </summary>
        protected virtual void OnAfterUse() { }

        /// <summary>
        /// Método hook llamado antes de equipar el item
        /// </summary>
        protected virtual void OnBeforeEquip() { }

        /// <summary>
        /// Método hook llamado después de equipar el item
        /// </summary>
        protected virtual void OnAfterEquip() { }

        /// <summary>
        /// Método hook llamado antes de desequipar el item
        /// </summary>
        protected virtual void OnBeforeUnequip() { }

        /// <summary>
        /// Método hook llamado después de desequipar el item
        /// </summary>
        protected virtual void OnAfterUnequip() { }

        #endregion

        #region Factory Method

        /// <summary>
        /// Método factory para crear una instancia del mismo tipo de decorator
        /// </summary>
        /// <param name="item">Item a decorar</param>
        /// <returns>Nueva instancia del decorator</returns>
        protected abstract ItemDecorator CreateDecorator(IItem item);

        #endregion

        #region Utility Methods

        /// <summary>
        /// Obtiene el item original sin decoradores
        /// </summary>
        public IItem GetBaseItem()
        {
            IItem current = wrappedItem;
            while (current is ItemDecorator decorator)
            {
                current = decorator.wrappedItem;
            }
            return current;
        }

        /// <summary>
        /// Verifica si el item tiene un decorator específico
        /// </summary>
        public bool HasDecorator<T>() where T : ItemDecorator
        {
            IItem current = this;
            while (current is ItemDecorator decorator)
            {
                if (current is T)
                    return true;
                current = decorator.wrappedItem;
            }
            return false;
        }

        /// <summary>
        /// Remueve un decorator específico si existe
        /// </summary>
        public IItem RemoveDecorator<T>() where T : ItemDecorator
        {
            if (this is T)
            {
                return wrappedItem;
            }

            if (wrappedItem is ItemDecorator decorator)
            {
                wrappedItem = decorator.RemoveDecorator<T>();
            }

            return this;
        }

        /// <summary>
        /// Obtiene información de debug incluyendo la cadena de decoradores
        /// </summary>
        public virtual string GetDecoratorChain()
        {
            System.Text.StringBuilder chain = new System.Text.StringBuilder();
            chain.Append($"[{GetType().Name}]");

            IItem current = wrappedItem;
            while (current is ItemDecorator decorator)
            {
                chain.Append($" -> [{decorator.GetType().Name}]");
                current = decorator.wrappedItem;
            }
            chain.Append($" -> [{current.GetType().Name}]");

            return chain.ToString();
        }

        #endregion
    }
}
