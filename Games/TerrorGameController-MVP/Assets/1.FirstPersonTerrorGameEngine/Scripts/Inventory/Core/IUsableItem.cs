namespace HorrorEngine
{
    /// <summary>
    /// Interfaz simple para items que pueden usarse desde el inventario.
    /// Similar a IItem pero específica para el sistema de inventario.
    /// </summary>
    public interface IUsableItem
    {
        /// <summary>
        /// Usa el item. Se llama cuando el jugador usa el item desde el inventario.
        /// </summary>
        void Use();
    }
}
