using UnityEngine;

namespace DeliriumArchne
{
    /// <summary>
    /// Interfaz que define el contrato para los modos de juego.
    /// Patrón Strategy: Permite cambiar algoritmos de comportamiento en tiempo de ejecución.
    /// </summary>
    public interface IGameModeStrategy
    {
        /// <summary>
        /// Nombre del modo de juego
        /// </summary>
        string ModeName { get; }

        /// <summary>
        /// Inicializa el modo de juego
        /// </summary>
        void Initialize();

        /// <summary>
        /// Actualiza la lógica del modo cada frame
        /// </summary>
        void Update();

        /// <summary>
        /// Se llama cuando el modo se activa
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Se llama cuando el modo se desactiva
        /// </summary>
        void OnExit();

        /// <summary>
        /// Limpia recursos cuando el modo termina
        /// </summary>
        void Cleanup();
    }
}
