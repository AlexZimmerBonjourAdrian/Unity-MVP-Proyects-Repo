using UnityEngine;

namespace HorrorEngine
{
    /// <summary>
    /// Componente modular para opción de invertir eje Y de la cámara en controladores de primera persona.
    /// Versión independiente para HorrorEngine.
    /// </summary>
    public class HorrorCameraInversionComponent : MonoBehaviour
    {
        [Header("Camera Inversion Settings")]
        [SerializeField] private bool invertCamera = false;

        /// <summary>
        /// Establece si la cámara debe estar invertida
        /// </summary>
        public void SetInverted(bool inverted)
        {
            invertCamera = inverted;
        }

        /// <summary>
        /// Verifica si la cámara está invertida
        /// </summary>
        public bool IsInverted()
        {
            return invertCamera;
        }

        /// <summary>
        /// Aplica la inversión al valor del mouse Y
        /// </summary>
        /// <param name="mouseY">Valor del input del mouse Y</param>
        /// <returns>Valor invertido o normal según la configuración</returns>
        public float ApplyInversion(float mouseY)
        {
            return invertCamera ? mouseY : -mouseY;
        }

        /// <summary>
        /// Alterna el estado de inversión
        /// </summary>
        public void ToggleInversion()
        {
            invertCamera = !invertCamera;
        }
    }
}
