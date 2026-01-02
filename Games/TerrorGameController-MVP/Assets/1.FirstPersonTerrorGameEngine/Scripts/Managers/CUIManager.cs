using UnityEngine;

public class CUIManager : MonoBehaviour
{
    public static CUIManager Instance { get; private set; } // Propiedad Singleton

    [SerializeField] private GameObject[] uiPanels; // Array de paneles de UI

    void Awake()
    {
        // Implementación del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Opcional: Mantener la instancia entre escenas
    }

    // Start es llamado una vez antes de la primera ejecución de Update
    void Start()
    {
        // Inicializar todos los paneles como ocultos
        foreach (var panel in uiPanels)
        {
            panel.SetActive(false);
        }
    }

    // Método para mostrar un panel específico
    public void ShowPanel(int panelIndex)
    {
        if (panelIndex >= 0 && panelIndex < uiPanels.Length)
        {
            uiPanels[panelIndex].SetActive(true);
        }
    }

    // Método para ocultar un panel específico
    public void HidePanel(int panelIndex)
    {
        if (panelIndex >= 0 && panelIndex < uiPanels.Length)
        {
            uiPanels[panelIndex].SetActive(false);
        }
    }

    // Método para alternar la visibilidad de un panel específico
    public void TogglePanel(int panelIndex)
    {
        if (panelIndex >= 0 && panelIndex < uiPanels.Length)
        {
            bool isActive = uiPanels[panelIndex].activeSelf;
            uiPanels[panelIndex].SetActive(!isActive);
        }
    }
}
