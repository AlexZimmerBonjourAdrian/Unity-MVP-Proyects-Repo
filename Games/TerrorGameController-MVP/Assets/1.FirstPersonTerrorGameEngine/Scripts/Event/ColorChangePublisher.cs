using UnityEngine;

namespace HorrorEngine.Events
{
    public class ColorChangePublisher : MonoBehaviour
    {
        [SerializeField] private string eventName = "ChangeColorEvent";
        [SerializeField] private Color colorToPublish = Color.red;

        private void Update()
        {
            // Publicar el evento al presionar la tecla "C"
            if (Input.GetKeyDown(KeyCode.C))
            {
                CGameEventManager.Publish(eventName, colorToPublish);
                Debug.Log($"Published color change event with color: {colorToPublish}");
            }
        }
    }
}
