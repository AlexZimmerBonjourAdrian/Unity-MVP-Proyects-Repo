using System.Collections.Generic;

namespace RolEngine
{
    /// <summary>
    /// Interface para persistencia de estadísticas.
    /// Permite diferentes implementaciones (PlayerPrefs, archivos, base de datos, etc.)
    /// </summary>
    public interface IStatPersistence
    {
        /// <summary>
        /// Guarda las estadísticas
        /// </summary>
        void SaveStats(Dictionary<string, int> stats);

        /// <summary>
        /// Carga las estadísticas guardadas
        /// </summary>
        Dictionary<string, int> LoadStats();

        /// <summary>
        /// Verifica si hay datos guardados
        /// </summary>
        bool HasSavedData();

        /// <summary>
        /// Elimina los datos guardados
        /// </summary>
        void ClearSavedData();
    }
}

