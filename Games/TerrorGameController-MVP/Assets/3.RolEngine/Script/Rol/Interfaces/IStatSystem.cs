using System.Collections.Generic;

namespace RolEngine
{
    /// <summary>
    /// Interface base para cualquier sistema de estadísticas.
    /// Permite desacoplamiento y reutilización en múltiples proyectos.
    /// </summary>
    /// <typeparam name="TStatType">Tipo enum de las estadísticas</typeparam>
    public interface IStatSystem<TStatType> where TStatType : System.Enum
    {
        /// <summary>
        /// Obtiene el valor de una estadística
        /// </summary>
        int GetStat(TStatType stat);

        /// <summary>
        /// Establece el valor de una estadística
        /// </summary>
        void SetStat(TStatType stat, int value);

        /// <summary>
        /// Aumenta una estadística en una cantidad específica
        /// </summary>
        void IncreaseStat(TStatType stat, int amount);

        /// <summary>
        /// Disminuye una estadística en una cantidad específica
        /// </summary>
        void DecreaseStat(TStatType stat, int amount);

        /// <summary>
        /// Verifica si una estadística cumple un requisito mínimo
        /// </summary>
        bool CheckStatRequirement(TStatType stat, int requiredValue);

        /// <summary>
        /// Obtiene todas las estadísticas actuales
        /// </summary>
        Dictionary<TStatType, int> GetAllStats();
    }
}

