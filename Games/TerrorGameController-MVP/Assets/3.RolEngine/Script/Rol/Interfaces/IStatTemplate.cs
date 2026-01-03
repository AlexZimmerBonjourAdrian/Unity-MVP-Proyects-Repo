using System.Collections.Generic;

namespace RolEngine
{
    /// <summary>
    /// Interface para templates de estadísticas.
    /// Permite diferentes implementaciones (clases, ScriptableObjects, etc.)
    /// </summary>
    /// <typeparam name="TStatType">Tipo enum de las estadísticas</typeparam>
    public interface IStatTemplate<TStatType> where TStatType : System.Enum
    {
        /// <summary>
        /// Nombre del template
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Diccionario con las estadísticas base del template
        /// </summary>
        Dictionary<TStatType, int> BaseStats { get; }
    }
}

