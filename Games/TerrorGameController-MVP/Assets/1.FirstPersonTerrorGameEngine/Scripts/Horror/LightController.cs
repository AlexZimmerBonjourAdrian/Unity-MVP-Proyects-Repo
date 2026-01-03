using UnityEngine;

namespace HorrorEngine
{
    public class LightController : MonoBehaviour
    {
        private Light lightSource; // Reference to the Light component
        private bool isLightOn = true; // State of the light
        
        // Initialize the light source in Start
        void Start()
        {
            lightSource = GetComponent<Light>();
            if (lightSource == null)
            {
                Debug.LogError("Light component not found on the GameObject.");
            }
        }

        // Toggle the light on/off when the player presses the "F" key
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleLight();
            }
        }

        // Method to toggle the light state
        private void ToggleLight()
        {
            if (lightSource != null)
            {
                isLightOn = !isLightOn;
                lightSource.enabled = isLightOn;
            }
        }
    }
}
