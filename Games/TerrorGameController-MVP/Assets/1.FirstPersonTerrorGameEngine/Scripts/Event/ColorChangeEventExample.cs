using UnityEngine;

namespace HorrorEngine
{
    public class ColorChangeEventExample : MonoBehaviour
    {
        [SerializeField] private string eventName = "ChangeColorEvent";
        private Renderer objectRenderer;

        private void Start()
        {
            objectRenderer = GetComponent<Renderer>();
            // Suscribirse al evento genérico que pasa un Color
            CGameEventManager.Subscribe<Color>(eventName, OnColorChange);
        }

        private void OnDestroy()
        {
            // Desuscribirse del evento al destruir el objeto
            CGameEventManager.Unsubscribe<Color>(eventName, OnColorChange);
        }

        private void OnColorChange(Color newColor)
        {
            // Cambiar el color del material del objeto
            objectRenderer.material.color = newColor;
        }
    }
}
