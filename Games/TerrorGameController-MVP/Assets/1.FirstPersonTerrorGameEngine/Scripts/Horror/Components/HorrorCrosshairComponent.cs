using UnityEngine;
using UnityEngine.UI;

namespace HorrorEngine
{
    /// <summary>
    /// Componente modular para sistema de crosshair en controladores de primera persona.
    /// Versión independiente para HorrorEngine.
    /// </summary>
    public class HorrorCrosshairComponent : MonoBehaviour
    {
        [Header("Crosshair Settings")]
        [SerializeField] private bool enableCrosshair = true;
        [SerializeField] private Sprite crosshairImage;
        [SerializeField] private Color crosshairColor = Color.white;

        private Image crosshairObject;

        private void Awake()
        {
            // Buscar el componente Image en los hijos
            crosshairObject = GetComponentInChildren<Image>();
            
            if (crosshairObject == null)
            {
                Debug.LogWarning("HorrorCrosshairComponent: No se encontró componente Image para crosshair. Buscando en toda la escena...");
                crosshairObject = FindObjectOfType<Image>();
            }

            if (crosshairObject == null)
            {
                Debug.LogError("HorrorCrosshairComponent: No se encontró componente Image. Crea un GameObject con Image para el crosshair.");
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            if (enableCrosshair)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        /// <summary>
        /// Muestra el crosshair
        /// </summary>
        public void Show()
        {
            if (crosshairObject != null)
            {
                crosshairObject.gameObject.SetActive(true);
                if (crosshairImage != null)
                {
                    crosshairObject.sprite = crosshairImage;
                }
                crosshairObject.color = crosshairColor;
            }
        }

        /// <summary>
        /// Oculta el crosshair
        /// </summary>
        public void Hide()
        {
            if (crosshairObject != null)
            {
                crosshairObject.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Establece el color del crosshair
        /// </summary>
        public void SetColor(Color color)
        {
            crosshairColor = color;
            if (crosshairObject != null)
            {
                crosshairObject.color = color;
            }
        }

        /// <summary>
        /// Establece el sprite del crosshair
        /// </summary>
        public void SetSprite(Sprite sprite)
        {
            crosshairImage = sprite;
            if (crosshairObject != null && sprite != null)
            {
                crosshairObject.sprite = sprite;
            }
        }

        /// <summary>
        /// Obtiene el color actual del crosshair
        /// </summary>
        public Color GetColor()
        {
            return crosshairColor;
        }

        /// <summary>
        /// Obtiene el sprite actual del crosshair
        /// </summary>
        public Sprite GetSprite()
        {
            return crosshairImage;
        }

        /// <summary>
        /// Verifica si el crosshair está visible
        /// </summary>
        public bool IsVisible()
        {
            return crosshairObject != null && crosshairObject.gameObject.activeSelf;
        }
    }
}
