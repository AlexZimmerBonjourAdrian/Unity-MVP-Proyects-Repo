using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RetroFPS
{
    /// <summary>
    /// Sistema básico de diálogos para Retro FPS Engine.
    /// Diseñado para conversaciones simples sin la complejidad del motor de terror.
    /// Soporta texto, opciones básicas y eventos.
    /// </summary>
    public class CDialogueManager : MonoBehaviour
    {
        public static CDialogueManager Instance { get; private set; }

        [Header("UI References")]
        [Tooltip("Panel principal del diálogo")]
        [SerializeField] private GameObject dialoguePanel;

        [Tooltip("Texto del nombre del hablante")]
        [SerializeField] private TextMeshProUGUI speakerNameText;

        [Tooltip("Texto del mensaje")]
        [SerializeField] private TextMeshProUGUI messageText;

        [Tooltip("Contenedor de opciones")]
        [SerializeField] private GameObject optionsContainer;

        [Tooltip("Prefab para botones de opción")]
        [SerializeField] private GameObject optionButtonPrefab;

        [Header("Configuration")]
        [Tooltip("Tiempo entre caracteres al escribir")]
        [SerializeField] private float textSpeed = 0.05f;

        [Tooltip("Habilitar logs de debug")]
        [SerializeField] private bool enableDebugLogs = true;

        // Estado del diálogo
        private bool isDialogueActive = false;
        private DialogueData currentDialogue;
        private Coroutine typingCoroutine;

        // Callbacks
        private System.Action<int> onOptionSelected;
        private System.Action onDialogueFinished;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            // Inicializar UI
            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }
        }

        private void Update()
        {
            // Skip text typing
            if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
            {
                SkipTyping();
            }

            // Close dialogue
            if (isDialogueActive && Input.GetKeyDown(KeyCode.Escape))
            {
                HideDialogue();
            }
        }

        /// <summary>
        /// Muestra un diálogo básico
        /// </summary>
        public void ShowDialogue(DialogueData dialogueData)
        {
            if (dialogueData == null)
            {
                LogDebug("Dialogue data is null");
                return;
            }

            currentDialogue = dialogueData;
            isDialogueActive = true;

            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(true);
            }

            UpdateUI();
            LogDebug($"Showing dialogue: {dialogueData.speakerName}");
        }

        /// <summary>
        /// Muestra diálogo con opciones
        /// </summary>
        public void ShowDialogueWithOptions(DialogueData dialogueData, System.Action<int> onOptionCallback)
        {
            onOptionSelected = onOptionCallback;
            ShowDialogue(dialogueData);
        }

        /// <summary>
        /// Muestra diálogo y ejecuta callback al terminar
        /// </summary>
        public void ShowDialogueWithCallback(DialogueData dialogueData, System.Action onFinishedCallback)
        {
            onDialogueFinished = onFinishedCallback;
            ShowDialogue(dialogueData);
        }

        /// <summary>
        /// Oculta el diálogo actual
        /// </summary>
        public void HideDialogue()
        {
            if (!isDialogueActive) return;

            isDialogueActive = false;

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;
            }

            if (dialoguePanel != null)
            {
                dialoguePanel.SetActive(false);
            }

            // Limpiar opciones
            ClearOptions();

            // Ejecutar callback si existe
            onDialogueFinished?.Invoke();
            onDialogueFinished = null;

            LogDebug("Dialogue hidden");
        }

        /// <summary>
        /// Actualiza la UI con los datos del diálogo actual
        /// </summary>
        private void UpdateUI()
        {
            if (currentDialogue == null) return;

            // Actualizar nombre del hablante
            if (speakerNameText != null)
            {
                speakerNameText.text = currentDialogue.speakerName;
            }

            // Iniciar escritura del mensaje
            if (messageText != null && !string.IsNullOrEmpty(currentDialogue.message))
            {
                typingCoroutine = StartCoroutine(TypeText(currentDialogue.message));
            }

            // Crear opciones si existen
            if (currentDialogue.choices != null && currentDialogue.choices.Length > 0)
            {
                CreateOptions(currentDialogue.choices);
            }
            else
            {
                // Si no hay opciones, esperar input para continuar
                StartCoroutine(WaitForContinue());
            }
        }

        /// <summary>
        /// Corrutina para escribir texto caracter por caracter
        /// </summary>
        private IEnumerator TypeText(string text)
        {
            if (messageText == null) yield break;

            messageText.text = "";
            foreach (char c in text)
            {
                messageText.text += c;
                yield return new WaitForSeconds(textSpeed);
            }

            typingCoroutine = null;
        }

        /// <summary>
        /// Salta la animación de escritura de texto
        /// </summary>
        private void SkipTyping()
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;

                if (messageText != null && currentDialogue != null)
                {
                    messageText.text = currentDialogue.message;
                }
            }
        }

        /// <summary>
        /// Crea los botones de opciones
        /// </summary>
        private void CreateOptions(string[] choices)
        {
            ClearOptions();

            if (optionsContainer == null || optionButtonPrefab == null) return;

            for (int i = 0; i < choices.Length; i++)
            {
                GameObject buttonObj = Instantiate(optionButtonPrefab, optionsContainer.transform);
                Button button = buttonObj.GetComponent<Button>();
                TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                {
                    buttonText.text = choices[i];
                }

                int choiceIndex = i; // Capturar el índice para el lambda
                button.onClick.AddListener(() => OnOptionClicked(choiceIndex));
            }

            LogDebug($"Created {choices.Length} dialogue options");
        }

        /// <summary>
        /// Limpia todas las opciones existentes
        /// </summary>
        private void ClearOptions()
        {
            if (optionsContainer == null) return;

            foreach (Transform child in optionsContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Maneja la selección de una opción
        /// </summary>
        private void OnOptionClicked(int optionIndex)
        {
            LogDebug($"Option selected: {optionIndex}");

            // Ejecutar callback si existe
            onOptionSelected?.Invoke(optionIndex);

            // Limpiar callback
            onOptionSelected = null;

            // Ocultar diálogo
            HideDialogue();
        }

        /// <summary>
        /// Espera input del jugador para continuar
        /// </summary>
        private IEnumerator WaitForContinue()
        {
            LogDebug("Waiting for player input to continue dialogue");

            while (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                yield return null;
            }

            HideDialogue();
        }

        /// <summary>
        /// Verifica si hay un diálogo activo
        /// </summary>
        public bool IsDialogueActive()
        {
            return isDialogueActive;
        }

        /// <summary>
        /// Obtiene información de debug
        /// </summary>
        public string GetDebugInfo()
        {
            return $"DialogueManager Debug Info:\n" +
                   $"- Dialogue active: {(isDialogueActive ? "Yes" : "No")}\n" +
                   $"- Current speaker: {currentDialogue?.speakerName ?? "None"}\n" +
                   $"- Typing active: {(typingCoroutine != null ? "Yes" : "No")}\n" +
                   $"- Text speed: {textSpeed}\n" +
                   $"- Debug logs: {(enableDebugLogs ? "Enabled" : "Disabled")}";
        }

        private void LogDebug(string message)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[DialogueManager] {message}");
            }
        }
    }

    /// <summary>
    /// Estructura de datos para diálogos
    /// </summary>
    [System.Serializable]
    public class DialogueData
    {
        [Tooltip("Nombre del hablante")]
        public string speakerName;

        [Tooltip("Mensaje a mostrar")]
        [TextArea(3, 10)]
        public string message;

        [Tooltip("Opciones de respuesta (opcional)")]
        public string[] choices;

        /// <summary>
        /// Constructor básico
        /// </summary>
        public DialogueData(string speaker = "", string msg = "", string[] opts = null)
        {
            speakerName = speaker;
            message = msg;
            choices = opts;
        }
    }

    /// <summary>
    /// ScriptableObject para diálogos reutilizables
    /// </summary>
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "RetroFPS/Dialogue Data", order = 1)]
    public class DialogueScriptableObject : ScriptableObject
    {
        [Tooltip("Nombre del hablante")]
        public string speakerName;

        [Tooltip("Mensaje a mostrar")]
        [TextArea(3, 10)]
        public string message;

        [Tooltip("Opciones de respuesta")]
        public string[] choices;

        /// <summary>
        /// Convierte a DialogueData
        /// </summary>
        public DialogueData ToDialogueData()
        {
            return new DialogueData(speakerName, message, choices);
        }
    }
}
