using UnityEngine;
using Yarn.Unity;
using Yarn.Unity.Legacy;

namespace HorrorEngine
{
    public class DialogueSystemValidator : MonoBehaviour
    {
        [Header("Validación del Sistema de Diálogos")]
        [SerializeField] private bool validateOnStart = true;
        
        void Start()
        {
            if (validateOnStart)
            {
                ValidateDialogueSystem();
            }
        }

        [ContextMenu("Validar Sistema de Diálogos")]
        public void ValidateDialogueSystem()
        {
            Debug.Log("=== VALIDACIÓN DEL SISTEMA DE DIÁLOGOS ===");
            
            // Verificar DialogueController
            if (DialogueController.Instance == null)
            {
                Debug.LogError("❌ DialogueController.Instance es null");
                Debug.LogError("   - Asegúrate de que DialogueController esté en la escena");
                Debug.LogError("   - Verifica que el GameObject tenga el componente DialogueController");
            }
            else
            {
                Debug.Log("✅ DialogueController.Instance encontrado");
                
                // Verificar DialogueRunner
                if (DialogueController.Instance.DialogueRunner == null)
                {
                    Debug.LogError("❌ DialogueRunner es null");
                    Debug.LogError("   - Verifica que DialogueRunner esté asignado en el inspector");
                    Debug.LogError("   - Busca el GameObject 'Dialogue System Horror' en la escena");
                }
                else
                {
                    Debug.Log("✅ DialogueRunner encontrado");
                }
                
                // Verificar LineView
                if (DialogueController.Instance.LineView == null)
                {
                    Debug.LogError("❌ LineView es null");
                    Debug.LogError("   - Verifica que LineView esté asignado en el inspector");
                    Debug.LogError("   - Busca el GameObject 'Line View' en la escena");
                }
                else
                {
                    Debug.Log("✅ LineView encontrado");
                }
            }
            
            // Verificar objetos en la escena
            GameObject dialogueSystem = GameObject.Find("Dialogue System Horror");
            if (dialogueSystem == null)
            {
                Debug.LogError("❌ GameObject 'Dialogue System Horror' no encontrado en la escena");
            }
            else
            {
                Debug.Log("✅ GameObject 'Dialogue System Horror' encontrado");
                
                DialogueRunner runner = dialogueSystem.GetComponent<DialogueRunner>();
                if (runner == null)
                {
                    Debug.LogError("❌ DialogueRunner no encontrado en 'Dialogue System Horror'");
                }
                else
                {
                    Debug.Log("✅ DialogueRunner encontrado en 'Dialogue System Horror'");
                }
            }
            
            GameObject lineView = GameObject.Find("Line View");
            if (lineView == null)
            {
                Debug.LogError("❌ GameObject 'Line View' no encontrado en la escena");
            }
            else
            {
                Debug.Log("✅ GameObject 'Line View' encontrado");
                
                LineView lineViewComponent = lineView.GetComponent<LineView>();
                if (lineViewComponent == null)
                {
                    Debug.LogError("❌ LineView no encontrado en 'Line View'");
                }
                else
                {
                    Debug.Log("✅ LineView encontrado en 'Line View'");
                }
            }
            
            Debug.Log("=== FIN DE VALIDACIÓN ===");
        }
    }
} 