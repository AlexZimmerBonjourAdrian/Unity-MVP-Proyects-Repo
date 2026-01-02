# Documentación Managers - Eldrick Super Food Market MVP

## Índice

1. [Introducción](#introducción)
2. [GameManager](#gamemanager)
3. [DialogueManager](#dialoguemanager)
4. [UIManager](#uimanager)
5. [Integración entre Managers](#integración-entre-managers)
6. [Ejemplos de Uso](#ejemplos-de-uso)
7. [Guía de Implementación](#guía-de-implementación)

---

## Introducción

Los Managers son MonoBehaviour singletons que gestionan diferentes aspectos del juego. Cada Manager tiene una responsabilidad específica y se comunica con los sistemas ECS y otros Managers.

### Patrón Singleton

Todos los Managers utilizan el patrón Singleton para garantizar una única instancia en toda la aplicación:

```csharp
private static ManagerName instance;
public static ManagerName Instance { get { return instance; } }
```

### Managers Disponibles

1. **GameManager**: Gestiona estados del juego y flujo principal
2. **DialogueManager**: Gestiona diálogos e interacciones con clientes
3. **UIManager**: Gestiona toda la interfaz de usuario

---

## GameManager

**Ubicación**: `Assets/Managers/GameManager.cs`

Gestiona los estados del juego y el flujo principal de la aplicación.

### Estados del Juego

```csharp
public enum GameState
{
    Menu,        // Menú principal
    Playing,     // Jugando (día de trabajo)
    Paused,      // Pausado
    GameOver,    // Game Over (explosión de ira o quiebra)
    Victory,     // Victoria (alcanzó $5,000)
    Dialogue,    // En diálogo con cliente
    Combat       // (No usado en MVP, heredado del juego anterior)
}
```

### Propiedades

- `static GameManager Instance`: Instancia singleton del GameManager
- `GameState currentState`: Estado actual del juego
- `World world`: Referencia al World ECS
- `ECS.ECSManager ecsManager`: Referencia al ECSManager

### Métodos Públicos

#### Gestión de Estados

```csharp
public void ChangeState(GameState newState)
```
Cambia el estado actual del juego.

**Parámetros**:
- `newState`: Nuevo estado del juego

**Ejemplo**:
```csharp
GameManager.Instance.ChangeState(GameState.Playing);
```

```csharp
public GameState GetCurrentState()
```
Obtiene el estado actual del juego.

**Retorna**: Estado actual del juego

**Ejemplo**:
```csharp
if (GameManager.Instance.GetCurrentState() == GameState.Playing)
{
    // El juego está en curso
}
```

#### Control del Juego

```csharp
public void StartGame()
```
Inicia el juego (sin implementar).

**Uso previsto**:
- Cambiar estado a `Playing`
- Inicializar sistemas ECS
- Resetear valores del jugador
- Generar primer cliente

**Ejemplo de Implementación**:
```csharp
public void StartGame()
{
    ChangeState(GameState.Playing);
    
    // Resetear jugador
    var anger = world.GetComponent<AngerComponent>(playerEntity);
    if (anger != null) anger.CurrentAnger = 0f;
    
    var money = world.GetComponent<MoneyComponent>(playerEntity);
    if (money != null) money.CurrentMoney = 0f;
    
    // Generar primer cliente
    CustomerSystem customerSystem = // obtener referencia
    customerSystem.SpawnCustomer();
}
```

```csharp
public void PauseGame()
```
Pausa el juego (sin implementar).

**Uso previsto**:
- Cambiar estado a `Paused`
- Pausar sistemas ECS
- Mostrar UI de pausa

```csharp
public void ResumeGame()
```
Reanuda el juego (sin implementar).

**Uso previsto**:
- Cambiar estado a `Playing`
- Reanudar sistemas ECS
- Ocultar UI de pausa

```csharp
public void GameOver()
```
Maneja el Game Over (sin implementar).

**Uso previsto**:
- Cambiar estado a `GameOver`
- Mostrar UI de Game Over
- Detener sistemas ECS

**Ejemplo de Implementación**:
```csharp
public void GameOver()
{
    ChangeState(GameState.GameOver);
    UIManager.Instance.ShowGameOverUI("Explotaste de ira y fuiste despedido");
}
```

```csharp
public void Victory()
```
Maneja la victoria (sin implementar).

**Uso previsto**:
- Cambiar estado a `Victory`
- Mostrar UI de victoria
- Detener sistemas ECS

**Ejemplo de Implementación**:
```csharp
public void Victory()
{
    ChangeState(GameState.Victory);
    UIManager.Instance.ShowVictoryUI("¡Lograste tu meta del primer día!");
}
```

```csharp
public void RestartGame()
```
Reinicia el juego (sin implementar).

**Uso previsto**:
- Resetear todos los valores
- Volver al menú principal
- Reiniciar sistemas ECS

### Inicialización

El GameManager se inicializa en `Awake()`:
- Verifica si ya existe una instancia (Singleton)
- Si no existe, se establece como instancia y se marca como `DontDestroyOnLoad`
- Si ya existe, se destruye el GameObject duplicado
- Llama a `Initialize()`

### Ejemplo de Uso Completo

```csharp
// Iniciar juego
GameManager.Instance.StartGame();

// Verificar estado
if (GameManager.Instance.GetCurrentState() == GameState.Playing)
{
    // El juego está activo
}

// Cambiar a diálogo
GameManager.Instance.ChangeState(GameState.Dialogue);

// Game Over
GameManager.Instance.GameOver();

// Victoria
GameManager.Instance.Victory();
```

---

## DialogueManager

**Ubicación**: `Assets/Managers/DialogueManager.cs`

Gestiona los diálogos e interacciones con los clientes.

### Propiedades

- `static DialogueManager Instance`: Instancia singleton del DialogueManager
- `World world`: Referencia al World ECS
- `Entity currentDialogueEntity`: Entidad del diálogo actual
- `bool isInDialogue`: Si está en un diálogo activo
- `DialogueComponent currentDialogue`: Componente de diálogo actual

### Métodos Públicos

#### Gestión de Diálogos

```csharp
public void StartDialogue(Entity entity)
```
Inicia un diálogo con una entidad (sin implementar).

**Parámetros**:
- `entity`: Entidad con la que iniciar el diálogo

**Uso previsto**:
- Obtener DialogueComponent de la entidad
- Establecer como diálogo actual
- Cambiar estado del juego a `Dialogue`
- Mostrar UI de diálogo

**Ejemplo de Implementación**:
```csharp
public void StartDialogue(Entity entity)
{
    var dialogue = world.GetComponent<DialogueComponent>(entity);
    if (dialogue == null) return;

    currentDialogueEntity = entity;
    currentDialogue = dialogue;
    isInDialogue = true;

    GameManager.Instance.ChangeState(GameState.Dialogue);
    UIManager.Instance.ShowDialogueUI();
    ShowDialogue(dialogue.InitialText);
    
    if (dialogue.Options.Count > 0)
    {
        List<string> options = dialogue.Options.Select(o => o.Text).ToList();
        ShowDialogueOptions(options);
    }
}
```

```csharp
public void EndDialogue()
```
Termina el diálogo actual (sin implementar).

**Uso previsto**:
- Resetear diálogo actual
- Cambiar estado del juego a `Playing`
- Ocultar UI de diálogo

**Ejemplo de Implementación**:
```csharp
public void EndDialogue()
{
    isInDialogue = false;
    currentDialogueEntity = new Entity(0);
    currentDialogue = null;

    GameManager.Instance.ChangeState(GameState.Playing);
    UIManager.Instance.HideDialogueUI();
}
```

#### Visualización de Diálogos

```csharp
public void ShowDialogue(string text)
```
Muestra un texto de diálogo (sin implementar).

**Parámetros**:
- `text`: Texto a mostrar

**Uso previsto**:
- Actualizar UI con el texto
- Mostrar panel de diálogo

**Ejemplo de Implementación**:
```csharp
public void ShowDialogue(string text)
{
    UIManager.Instance.dialogueText.text = text;
}
```

```csharp
public void ShowDialogueOptions(List<string> options)
```
Muestra opciones de diálogo (sin implementar).

**Parámetros**:
- `options`: Lista de opciones de diálogo

**Uso previsto**:
- Mostrar botones con las opciones
- Configurar callbacks para cada opción

**Ejemplo de Implementación**:
```csharp
public void ShowDialogueOptions(List<string> options)
{
    for (int i = 0; i < options.Count && i < UIManager.Instance.dialogueOptionButtons.Length; i++)
    {
        var button = UIManager.Instance.dialogueOptionButtons[i];
        button.gameObject.SetActive(true);
        button.GetComponentInChildren<TextMeshProUGUI>().text = options[i];
        
        int index = i; // Captura para closure
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => SelectDialogueOption(index));
    }
}
```

```csharp
public void SelectDialogueOption(int optionIndex)
```
Selecciona una opción de diálogo (sin implementar).

**Parámetros**:
- `optionIndex`: Índice de la opción seleccionada

**Uso previsto**:
- Procesar la selección
- Aplicar efectos (ira, dinero, etc.)
- Avanzar al siguiente diálogo o terminar

**Ejemplo de Implementación**:
```csharp
public void SelectDialogueOption(int optionIndex)
{
    if (currentDialogue == null) return;
    if (optionIndex < 0 || optionIndex >= currentDialogue.Options.Count) return;

    var option = currentDialogue.Options[optionIndex];
    
    // Aplicar efectos según la opción
    // Por ejemplo, aumentar/reducir ira, añadir propina, etc.
    
    if (option.NextDialogueId > 0)
    {
        // Cargar siguiente diálogo
    }
    else
    {
        EndDialogue();
    }
}
```

#### Utilidades

```csharp
public bool IsInDialogue()
```
Verifica si está en un diálogo activo.

**Retorna**: `true` si está en diálogo, `false` en caso contrario

**Ejemplo**:
```csharp
if (DialogueManager.Instance.IsInDialogue())
{
    // No procesar otras acciones
}
```

```csharp
public void SetWorld(World world)
```
Establece la referencia al World ECS.

**Parámetros**:
- `world`: Referencia al World

**Ejemplo**:
```csharp
DialogueManager.Instance.SetWorld(ecsManager.GetWorld());
```

```csharp
public void UpdateFlags(string flagName, bool value)
```
Actualiza flags del juego (sin implementar).

**Parámetros**:
- `flagName`: Nombre del flag
- `value`: Valor del flag

**Uso previsto**:
- Actualizar flags globales
- Usar para decisiones del jugador

### Inicialización

El DialogueManager se inicializa en `Awake()`:
- Verifica si ya existe una instancia (Singleton)
- Si no existe, se establece como instancia
- Si ya existe, se destruye el GameObject duplicado
- Llama a `Initialize()` que establece `isInDialogue = false`

### Ejemplo de Uso Completo

```csharp
// Iniciar diálogo con cliente
Entity customer = // obtener cliente
DialogueManager.Instance.StartDialogue(customer);

// Verificar si está en diálogo
if (DialogueManager.Instance.IsInDialogue())
{
    // Procesar input de diálogo
}

// Seleccionar opción
DialogueManager.Instance.SelectDialogueOption(0);

// Terminar diálogo
DialogueManager.Instance.EndDialogue();
```

---

## UIManager

**Ubicación**: `Assets/Managers/UIManager.cs`

Gestiona toda la interfaz de usuario del juego.

### Propiedades Públicas (Serializables)

#### Combat UI (No usado en MVP, heredado del juego anterior)
- `GameObject combatPanel`: Panel de combate
- `TextMeshProUGUI playerHealthText`: Texto de salud del jugador
- `TextMeshProUGUI enemyHealthText`: Texto de salud del enemigo
- `Button attackButton`: Botón de ataque
- `Button defendButton`: Botón de defensa

#### Dialogue UI
- `GameObject dialoguePanel`: Panel de diálogo
- `TextMeshProUGUI dialogueText`: Texto del diálogo
- `GameObject dialogueOptionsPanel`: Panel de opciones
- `Button[] dialogueOptionButtons`: Array de botones de opciones

#### Time UI (No usado en MVP de 1 día)
- `GameObject timePanel`: Panel de tiempo
- `TextMeshProUGUI encountersText`: Texto de encuentros
- `TextMeshProUGUI daysText`: Texto de días

#### Game Over UI
- `GameObject gameOverPanel`: Panel de Game Over
- `TextMeshProUGUI gameOverText`: Texto de Game Over

#### Victory UI
- `GameObject victoryPanel`: Panel de victoria
- `TextMeshProUGUI victoryText`: Texto de victoria

#### Menu UI
- `GameObject menuPanel`: Panel de menú
- `Button startButton`: Botón de empezar
- `Button quitButton`: Botón de salir

### Propiedades

- `static UIManager Instance`: Instancia singleton del UIManager

### Métodos Públicos

#### Gestión de Paneles

```csharp
public void ShowCombatUI()
```
Muestra la UI de combate (sin implementar, no usado en MVP).

```csharp
public void HideCombatUI()
```
Oculta la UI de combate (sin implementar, no usado en MVP).

```csharp
public void ShowDialogueUI()
```
Muestra la UI de diálogo (sin implementar).

**Ejemplo de Implementación**:
```csharp
public void ShowDialogueUI()
{
    if (dialoguePanel != null)
    {
        dialoguePanel.SetActive(true);
    }
}
```

```csharp
public void HideDialogueUI()
```
Oculta la UI de diálogo (sin implementar).

**Ejemplo de Implementación**:
```csharp
public void HideDialogueUI()
{
    if (dialoguePanel != null)
    {
        dialoguePanel.SetActive(false);
    }
}
```

```csharp
public void ShowTimeUI()
```
Muestra la UI de tiempo (sin implementar, no usado en MVP).

```csharp
public void HideTimeUI()
```
Oculta la UI de tiempo (sin implementar, no usado en MVP).

```csharp
public void ShowGameOverUI(string message)
```
Muestra la UI de Game Over (sin implementar).

**Parámetros**:
- `message`: Mensaje a mostrar

**Ejemplo de Implementación**:
```csharp
public void ShowGameOverUI(string message)
{
    if (gameOverPanel != null)
    {
        gameOverPanel.SetActive(true);
    }
    if (gameOverText != null)
    {
        gameOverText.text = message;
    }
}
```

```csharp
public void HideGameOverUI()
```
Oculta la UI de Game Over (sin implementar).

```csharp
public void ShowVictoryUI(string message)
```
Muestra la UI de victoria (sin implementar).

**Parámetros**:
- `message`: Mensaje a mostrar

**Ejemplo de Implementación**:
```csharp
public void ShowVictoryUI(string message)
{
    if (victoryPanel != null)
    {
        victoryPanel.SetActive(true);
    }
    if (victoryText != null)
    {
        victoryText.text = message;
    }
}
```

```csharp
public void HideVictoryUI()
```
Oculta la UI de victoria (sin implementar).

```csharp
public void ShowMenuUI()
```
Muestra la UI del menú (sin implementar).

```csharp
public void HideMenuUI()
```
Oculta la UI del menú (sin implementar).

#### Actualización de Valores

```csharp
public void UpdatePlayerHealth(float health)
```
Actualiza la salud del jugador (sin implementar, no usado en MVP).

```csharp
public void UpdateEnemyHealth(float health)
```
Actualiza la salud del enemigo (sin implementar, no usado en MVP).

```csharp
public void UpdateEncounters(int current, int max)
```
Actualiza los encuentros (sin implementar, no usado en MVP).

```csharp
public void UpdateDays(int days)
```
Actualiza los días (sin implementar, no usado en MVP).

### Métodos Necesarios para MVP (No implementados)

Los siguientes métodos deberían añadirse para el MVP:

```csharp
// Actualizar barra de ira
public void UpdateAngerBar(float currentAnger, float maxAnger)
{
    // Actualizar barra de ira con colores según nivel
}

// Actualizar dinero
public void UpdateMoney(float currentMoney)
{
    // Actualizar texto de dinero actual
}

// Actualizar ahorros
public void UpdateSavings(float totalSavings, float goal)
{
    // Actualizar texto de ahorros y progreso
}

// Mostrar cliente actual
public void ShowCurrentCustomer(string customerName, CustomerType type)
{
    // Mostrar información del cliente actual
}

// Mostrar pedido
public void ShowOrder(string orderDescription)
{
    // Mostrar descripción del pedido
}

// Mostrar menú de selección de pedido
public void ShowOrderMenu(List<string> options)
{
    // Mostrar opciones de componentes del pedido
}

// Feedback de éxito/error
public void ShowOrderFeedback(bool isCorrect)
{
    // Mostrar ✓ verde o ✗ rojo
}

// Mostrar pantalla de fin de turno
public void ShowEndOfDayUI(float income, float expenses, float savings)
{
    // Mostrar resumen del día
}
```

### Inicialización

El UIManager se inicializa en `Awake()`:
- Verifica si ya existe una instancia (Singleton)
- Si no existe, se establece como instancia
- Si ya existe, se destruye el GameObject duplicado
- Llama a `Initialize()` (vacío actualmente)

### Ejemplo de Uso Completo

```csharp
// Mostrar UI de diálogo
UIManager.Instance.ShowDialogueUI();

// Actualizar texto de diálogo
UIManager.Instance.dialogueText.text = "Hola, ¿qué deseas?";

// Mostrar Game Over
UIManager.Instance.ShowGameOverUI("Explotaste de ira");

// Mostrar Victoria
UIManager.Instance.ShowVictoryUI("¡Lograste tu meta!");

// Ocultar UI
UIManager.Instance.HideDialogueUI();
```

---

## Integración entre Managers

### Flujo de Comunicación

```
GameManager (Estados)
    ↓
DialogueManager (Diálogos)
    ↓
UIManager (Visualización)
```

### Ejemplo de Integración

```csharp
// En GameManager.StartGame()
public void StartGame()
{
    ChangeState(GameState.Playing);
    UIManager.Instance.HideMenuUI();
    UIManager.Instance.UpdateMoney(0f);
    UIManager.Instance.UpdateAngerBar(0f, 100f);
}

// En DialogueManager.StartDialogue()
public void StartDialogue(Entity entity)
{
    GameManager.Instance.ChangeState(GameState.Dialogue);
    UIManager.Instance.ShowDialogueUI();
    // ... resto de lógica
}

// En GameManager.GameOver()
public void GameOver()
{
    ChangeState(GameState.GameOver);
    UIManager.Instance.ShowGameOverUI("Explotaste de ira y fuiste despedido");
}
```

---

## Ejemplos de Uso

### Iniciar el Juego

```csharp
// En el botón "Empezar Día" del menú
public void OnStartButtonClicked()
{
    GameManager.Instance.StartGame();
    UIManager.Instance.HideMenuUI();
}
```

### Procesar Diálogo con Cliente

```csharp
// Cuando un cliente llega
Entity customer = customerSystem.SpawnCustomer();
DialogueManager.Instance.StartDialogue(customer);

// Cuando el jugador selecciona una opción
public void OnDialogueOptionSelected(int optionIndex)
{
    DialogueManager.Instance.SelectDialogueOption(optionIndex);
    
    // Aplicar efectos según la opción
    if (optionIndex == 0) // Opción profesional
    {
        angerSystem.DecreaseAnger(5f);
    }
    else // Opción agresiva
    {
        angerSystem.IncreaseAnger(10f);
    }
}
```

### Actualizar UI Durante el Juego

```csharp
// En Update() o cuando cambian valores
void Update()
{
    // Actualizar barra de ira
    float currentAnger = angerSystem.GetCurrentAnger();
    UIManager.Instance.UpdateAngerBar(currentAnger, 100f);
    
    // Actualizar dinero
    float currentMoney = economySystem.GetCurrentMoney();
    UIManager.Instance.UpdateMoney(currentMoney);
    
    // Actualizar ahorros
    float savings = economySystem.GetTotalSavings();
    UIManager.Instance.UpdateSavings(savings, 5000f);
}
```

### Manejar Game Over

```csharp
// En AngerSystem cuando la ira llega a 100%
if (anger.HasExploded)
{
    GameManager.Instance.GameOver();
}
```

### Manejar Victoria

```csharp
// Al final del turno
economySystem.ProcessDailyExpenses();
if (economySystem.HasReachedGoal())
{
    GameManager.Instance.Victory();
}
```

---

## Guía de Implementación

### Paso 1: Implementar GameManager

1. **StartGame()**:
   - Cambiar estado a `Playing`
   - Resetear valores del jugador
   - Generar primer cliente
   - Ocultar menú, mostrar UI de juego

2. **GameOver()**:
   - Cambiar estado a `GameOver`
   - Mostrar UI de Game Over
   - Detener sistemas

3. **Victory()**:
   - Cambiar estado a `Victory`
   - Mostrar UI de victoria
   - Detener sistemas

### Paso 2: Implementar DialogueManager

1. **StartDialogue()**:
   - Obtener DialogueComponent
   - Establecer como actual
   - Cambiar estado a `Dialogue`
   - Mostrar UI de diálogo

2. **SelectDialogueOption()**:
   - Procesar selección
   - Aplicar efectos (ira, dinero)
   - Avanzar o terminar diálogo

3. **EndDialogue()**:
   - Resetear diálogo actual
   - Cambiar estado a `Playing`
   - Ocultar UI de diálogo

### Paso 3: Implementar UIManager

1. **Métodos Show/Hide**:
   - Activar/desactivar paneles
   - Actualizar textos

2. **Métodos Update**:
   - `UpdateAngerBar()`: Actualizar barra con colores
   - `UpdateMoney()`: Actualizar texto de dinero
   - `UpdateSavings()`: Actualizar texto de ahorros
   - `ShowOrderFeedback()`: Mostrar ✓ o ✗

3. **UI de Fin de Turno**:
   - Mostrar ingresos, gastos, ahorros
   - Botón de continuar/reintentar

### Paso 4: Conectar con Sistemas ECS

1. **Obtener referencias**:
   - Obtener World del ECSManager
   - Obtener sistemas ECS
   - Obtener entidad del jugador

2. **Actualizar UI**:
   - En Update(), leer valores de componentes
   - Actualizar UI con valores actuales

3. **Reaccionar a eventos**:
   - Cuando cambia la ira → actualizar barra
   - Cuando cambia el dinero → actualizar texto
   - Cuando explota → Game Over
   - Cuando alcanza meta → Victoria

---

## Notas de Desarrollo

### Estado Actual

- ✅ Estructura de Managers completa
- ✅ Patrón Singleton implementado
- ✅ Métodos definidos
- ⚠️ Lógica de métodos sin implementar (TODO)
- ⚠️ Métodos adicionales necesarios para MVP

### Métodos Necesarios para MVP

**UIManager** necesita estos métodos adicionales:
- `UpdateAngerBar(float current, float max)`
- `UpdateMoney(float money)`
- `UpdateSavings(float savings, float goal)`
- `ShowCurrentCustomer(string name, CustomerType type)`
- `ShowOrder(string description)`
- `ShowOrderMenu(List<string> options)`
- `ShowOrderFeedback(bool isCorrect)`
- `ShowEndOfDayUI(float income, float expenses, float savings)`

### Próximos Pasos

1. Implementar lógica de GameManager
2. Implementar lógica de DialogueManager
3. Implementar métodos adicionales de UIManager
4. Conectar Managers con sistemas ECS
5. Testing y pulido

---

## Referencias

- **GDD MVP**: `Assets/Documetation/GDD_MVP_1Semana.txt`
- **Documentación ECS**: `Assets/Documetation/ECS_Documentation.md`
- **Código Managers**: `Assets/Managers/`

---

**Última actualización**: Estructura inicial para MVP de 1 semana

