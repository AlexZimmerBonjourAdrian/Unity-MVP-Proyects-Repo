# Sistema de Diálogos - Retro FPS Engine

## 💬 Sistema Básico de Diálogos

El sistema de diálogos en Retro FPS Engine está diseñado para ser **simple y efectivo**, perfecto para juegos FPS retro. A diferencia del sistema complejo del motor de terror, este se enfoca en conversaciones básicas sin narrativa avanzada.

## 🏗️ Arquitectura

### **CDialogueManager**
Sistema central que maneja todos los diálogos:

```csharp
namespace RetroFPS
{
    public class CDialogueManager : MonoBehaviour
    {
        // Singleton para acceso global
        public static CDialogueManager Instance { get; private set; }

        // Componentes UI básicos
        public GameObject dialoguePanel;
        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI messageText;
    }
}
```

### **DialogueData**
Estructura simple para datos de diálogo:

```csharp
[System.Serializable]
public class DialogueData
{
    public string speakerName;    // Nombre del hablante
    public string message;        // Texto del mensaje
    public string[] choices;      // Opciones (opcional)
}
```

## 🚀 Configuración Inicial

### 1. **Crear UI Básica**
```bash
# Crear Canvas con:
- Panel (DialoguePanel) - Para mostrar/ocultar diálogo
- Text (SpeakerName) - Nombre del hablante
- Text (Message) - Texto del mensaje
- Panel (OptionsContainer) - Contenedor de opciones
- Button Prefab (OptionButton) - Prefab para opciones
```

### 2. **Configurar DialogueManager**
```csharp
// Crear GameObject "DialogueManager"
// Agregar componente CDialogueManager
// Asignar referencias en el Inspector:
// - Dialogue Panel
// - Speaker Name Text
// - Message Text
// - Options Container
// - Option Button Prefab
```

### 3. **Ajustes Opcionales**
```csharp
// En DialogueManager:
textSpeed = 0.05f;        // Velocidad de escritura
enableDebugLogs = true;   // Para desarrollo
```

## 🎮 Uso Básico

### **Diálogo Simple (Solo Texto)**
```csharp
// Crear datos de diálogo
var dialogue = new DialogueData
{
    speakerName = "Guía",
    message = "¡Bienvenido a Retro FPS! Presiona ESPACIO para continuar."
};

// Mostrar diálogo
CDialogueManager.Instance.ShowDialogue(dialogue);
```

### **Diálogo con Opciones**
```csharp
var dialogue = new DialogueData
{
    speakerName = "NPC",
    message = "¿Qué tipo de arma prefieres?",
    choices = new string[] {
        "Pistola rápida",
        "Escopeta poderosa",
        "No necesito armas"
    }
};

// Mostrar con callback para manejar selección
CDialogueManager.Instance.ShowDialogueWithOptions(dialogue, OnWeaponChoice);
```

```csharp
private void OnWeaponChoice(int choiceIndex)
{
    switch (choiceIndex)
    {
        case 0:
            PlayerInventory.Instance.GiveWeapon(WeaponType.Pistol);
            break;
        case 1:
            PlayerInventory.Instance.GiveWeapon(WeaponType.Shotgun);
            break;
        case 2:
            // No dar arma
            break;
    }
}
```

### **Diálogo con Callback al Terminar**
```csharp
var dialogue = new DialogueData
{
    speakerName = "Narrador",
    message = "La aventura comienza..."
};

// Mostrar y ejecutar acción al terminar
CDialogueManager.Instance.ShowDialogueWithCallback(dialogue, StartGame);
```

## 🎨 Interacción con NPCs

### **NPC Básico**
```csharp
public class NPC : MonoBehaviour, Iinteract
{
    [SerializeField] private DialogueData npcDialogue;

    public void Oninteract()
    {
        if (npcDialogue != null)
        {
            CDialogueManager.Instance.ShowDialogue(npcDialogue);
        }
    }
}
```

### **NPC con Estado**
```csharp
public class QuestNPC : MonoBehaviour, Iinteract
{
    private bool questCompleted = false;

    public void Oninteract()
    {
        DialogueData dialogue;

        if (!questCompleted)
        {
            dialogue = new DialogueData(
                "NPC",
                "Ayúdame a encontrar mi llave perdida.",
                new string[] { "Claro, te ayudo", "No tengo tiempo" }
            );

            CDialogueManager.Instance.ShowDialogueWithOptions(dialogue, OnQuestChoice);
        }
        else
        {
            dialogue = new DialogueData(
                "NPC",
                "¡Gracias por tu ayuda! Toma esta recompensa."
            );

            CDialogueManager.Instance.ShowDialogueWithCallback(dialogue, GiveReward);
        }
    }

    private void OnQuestChoice(int choice)
    {
        if (choice == 0) // Ayudar
        {
            // Activar quest
            QuestManager.Instance.StartQuest("FindKey");
        }
    }

    private void GiveReward()
    {
        PlayerInventory.Instance.AddItem("HealthPotion");
        questCompleted = true;
    }
}
```

## 📝 Scriptable Objects para Diálogos

### **Crear Asset de Diálogo**
```bash
# Assets → Create → RetroFPS → Dialogue Data
# Configurar:
- Speaker Name: "Capitán"
- Message: "La base está bajo ataque..."
- Choices: (vacío para diálogo simple)
```

### **Uso en Código**
```csharp
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogueAsset;

    private void Start()
    {
        // Convertir asset a datos de diálogo
        var dialogueData = dialogueAsset.ToDialogueData();
        CDialogueManager.Instance.ShowDialogue(dialogueData);
    }
}
```

## 🎮 Controles del Jugador

### **Controles Básicos**
```csharp
// En DialogueManager.Update():
if (isDialogueActive)
{
    // ESPACIO: Saltar animación de texto
    if (Input.GetKeyDown(KeyCode.Space))
        SkipTyping();

    // ESC: Cerrar diálogo
    if (Input.GetKeyDown(KeyCode.Escape))
        HideDialogue();

    // ENTER: Continuar (sin opciones)
    if (Input.GetKeyDown(KeyCode.Return))
        ContinueDialogue();
}
```

### **Bloquear Movimiento Durante Diálogo**
```csharp
public class PlayerController : MonoBehaviour
{
    private void Update()
    {
        // Bloquear movimiento durante diálogo
        if (CDialogueManager.Instance.IsDialogueActive())
            return;

        // Lógica normal de movimiento
        HandleMovement();
        HandleShooting();
    }
}
```

## 🎨 Personalización Visual

### **Estilos de Diálogo**
```csharp
// Cambiar colores según tipo de hablante
public enum SpeakerType { Player, NPC, Narrator, Enemy }

public void SetDialogueStyle(SpeakerType type)
{
    switch (type)
    {
        case SpeakerType.Player:
            speakerNameText.color = Color.blue;
            break;
        case SpeakerType.NPC:
            speakerNameText.color = Color.green;
            break;
        case SpeakerType.Narrator:
            speakerNameText.color = Color.yellow;
            break;
    }
}
```

### **Animaciones Básicas**
```csharp
// Agregar fade in/out al panel
public void ShowPanelAnimated()
{
    dialoguePanel.SetActive(true);
    // Agregar tweening aquí (DOTween, LeanTween, etc.)
}

public void HidePanelAnimated()
{
    // Agregar tweening aquí
    dialoguePanel.SetActive(false);
}
```

## 🔧 Sistema de Eventos

### **Integración con Event System**
```csharp
// Publicar eventos de diálogo
public static class DialogueEvents
{
    public static event System.Action<DialogueData> OnDialogueStarted;
    public static event System.Action<int> OnOptionSelected;
    public static event System.Action OnDialogueFinished;
}

// En DialogueManager
private void TriggerDialogueEvents(DialogueData data)
{
    DialogueEvents.OnDialogueStarted?.Invoke(data);
}

private void TriggerOptionSelected(int index)
{
    DialogueEvents.OnOptionSelected?.Invoke(index);
}

private void TriggerDialogueFinished()
{
    DialogueEvents.OnDialogueFinished?.Invoke();
}
```

## 📊 Casos de Uso en FPS Retro

### **Tutorial Básico**
```csharp
public class TutorialManager : MonoBehaviour
{
    private void Start()
    {
        ShowTutorialStep(0);
    }

    private void ShowTutorialStep(int step)
    {
        DialogueData tutorialDialogue = null;

        switch (step)
        {
            case 0:
                tutorialDialogue = new DialogueData(
                    "Tutorial",
                    "Bienvenido a Retro FPS. Usa WASD para moverte.",
                    new string[] { "Entendido" }
                );
                CDialogueManager.Instance.ShowDialogueWithOptions(
                    tutorialDialogue,
                    (choice) => ShowTutorialStep(1)
                );
                break;

            case 1:
                tutorialDialogue = new DialogueData(
                    "Tutorial",
                    "Presiona el botón izquierdo del mouse para disparar."
                );
                CDialogueManager.Instance.ShowDialogueWithCallback(
                    tutorialDialogue,
                    () => EnableShootingTutorial()
                );
                break;
        }
    }
}
```

### **Sistema de Misiones**
```csharp
public class QuestDialogue : MonoBehaviour, Iinteract
{
    [SerializeField] private string questId;
    [SerializeField] private DialogueData questStartDialogue;
    [SerializeField] private DialogueData questCompleteDialogue;

    public void Oninteract()
    {
        bool isCompleted = QuestManager.Instance.IsQuestCompleted(questId);

        if (isCompleted)
        {
            CDialogueManager.Instance.ShowDialogue(questCompleteDialogue);
        }
        else
        {
            CDialogueManager.Instance.ShowDialogueWithOptions(
                questStartDialogue,
                (choice) => {
                    if (choice == 0) // Aceptar quest
                    {
                        QuestManager.Instance.StartQuest(questId);
                    }
                }
            );
        }
    }
}
```

## 🐛 Troubleshooting

### **Diálogo no aparece**
```csharp
// Verificar:
1. DialogueManager existe en escena
2. UI Panel está asignado correctamente
3. DialogueData no es null
4. Debug: CDialogueManager.Instance.GetDebugInfo()
```

### **Texto no se escribe**
```csharp
// Verificar:
1. MessageText está asignado
2. TextSpeed > 0
3. Texto no está vacío
4. Coroutine no fue interrumpida
```

### **Opciones no funcionan**
```csharp
// Verificar:
1. OptionsContainer está asignado
2. OptionButtonPrefab tiene Button component
3. OptionButtonPrefab tiene TextMeshProUGUI
4. Callbacks están asignados correctamente
```

## 📈 Limitaciones vs Sistema Avanzado

### **Este Sistema (Básico)**
- ✅ Simple y rápido de implementar
- ✅ Perfecto para FPS retro
- ✅ Bajo overhead de memoria
- ✅ Fácil de mantener
- ❌ Sin narrativa compleja
- ❌ Sin branching avanzado
- ❌ Sin variables de estado

### **Sistema del Motor de Terror**
- ✅ Narrativa rica y compleja
- ✅ Sistema de Yarn Spinner
- ✅ Variables y estados
- ✅ Branching avanzado
- ❌ Complejo de configurar
- ❌ Alto overhead
- ❌ Overkill para juegos simples

## 🎯 Mejores Prácticas

### ✅ **Mantener Simplicidad**
```csharp
// Preferir diálogos cortos y directos
var dialogue = new DialogueData("NPC", "¡Cuidado con los enemigos!");

// Evitar diálogos largos
// MAL: diálogos de párrafos completos
```

### ✅ **Usar Opciones Limitadas**
```csharp
// Máximo 2-3 opciones por diálogo
choices = new string[] { "Sí", "No" };

// Evitar menús complejos
```

### ✅ **Integrar con Gameplay**
```csharp
// Los diálogos deben avanzar el juego
CDialogueManager.Instance.ShowDialogueWithCallback(
    unlockDoorDialogue,
    () => DoorManager.Instance.UnlockDoor(doorId)
);
```

---

**Versión**: 1.0.0  
**Complejidad**: Baja (perfecto para FPS retro)  
**UI Requirements**: TextMeshPro, Canvas, Buttons  
**Integration**: Fácil con sistema existente  
**Extensibility**: Fácil agregar nuevas funcionalidades
