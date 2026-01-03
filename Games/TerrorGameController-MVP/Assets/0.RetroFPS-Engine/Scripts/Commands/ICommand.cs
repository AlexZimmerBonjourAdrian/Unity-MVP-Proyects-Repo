namespace RetroFPS
{
    /// <summary>
    /// Command Pattern - Interface base para comandos ejecutables.
    /// Define el contrato que deben cumplir todos los comandos.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Ejecuta la acción del comando
        /// </summary>
        void Execute();

        /// <summary>
        /// Deshace la acción del comando (si es posible)
        /// </summary>
        void Undo();

        /// <summary>
        /// Verifica si el comando puede ejecutarse en el estado actual
        /// </summary>
        /// <returns>True si puede ejecutarse</returns>
        bool CanExecute();

        /// <summary>
        /// Descripción del comando para debugging/UI
        /// </summary>
        string Description { get; }
    }
}
