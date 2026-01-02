using HorrorEngine.Interfaces;
using UnityEngine;

namespace HorrorEngine.Events
{
    public class ExampleFlag : MonoBehaviour, Iinteract
    {
        [SerializeField] private string flagName = "ChangeColor";
        [SerializeField] private Color activeColor = Color.red;
        [SerializeField] private Color inactiveColor = Color.white;
        private Renderer objectRenderer;

        private void Start()
        {
            objectRenderer = GetComponent<Renderer>();
            CFlagManager.LoadFlags(); // Cargar los flags antes de actualizar el color
            UpdateColor();
        }

        public void ToggleFlag()
        {
            bool currentFlag = CFlagManager.GetFlag(flagName);
            CFlagManager.SetFlag(flagName, !currentFlag);
            UpdateColor();
            CFlagManager.SaveFlags(); // Save the flag state after toggling
        }

        private void UpdateColor()
        {
            bool isActive = CFlagManager.GetFlag(flagName);
            objectRenderer.material.color = isActive ? activeColor : inactiveColor;
        }

        public void Oninteract()
        {
          ToggleFlag();
        }
    }
}
