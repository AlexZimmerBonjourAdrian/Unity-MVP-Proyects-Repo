# Guía de Integración - FirstPersonTerrorGameEngine

## Tabla de Contenidos
1. [Requisitos Previos](#requisitos-previos)
2. [Instalación](#instalación)
3. [Configuración del Assembly Definition](#configuración-del-assembly-definition)
4. [Sistemas Principales](#sistemas-principales)
5. [Configuración Inicial](#configuración-inicial)
6. [Uso Básico](#uso-básico)
7. [Ejemplos de Integración](#ejemplos-de-integración)
8. [Troubleshooting](#troubleshooting)

---

## Requisitos Previos

### Dependencias de Unity
El motor requiere los siguientes paquetes de Unity:

- **Unity.TextMeshPro** - Para UI y texto
- **YarnSpinner.Unity** - Sistema de diálogos
- **Unity.InputSystem** - Sistema de input (opcional, puede usar Input legacy)
- **Unity.Addressables** - Gestión de assets
- **Unity.ResourceManager** - Gestión de recursos

### Dependencias Externas
- **FPSCore** - Motor FPS base (debe estar en el proyecto)

### Versión de Unity
Recomendado: Unity 2021.3 LTS o superior

---

## Instalación

### Paso 1: Copiar el Motor
1. Copia la carpeta `Assets/1.FirstPersonTerrorGameEngine` a tu proyecto Unity
2. Asegúrate de mantener la estructura de carpetas intacta

### Paso 2: Verificar Dependencias
1. Abre Unity Package Manager (`Window > Package Manager`)
2. Instala los paquetes requeridos si no están instalados:
   - TextMeshPro
   - Yarn Spinner
   - Input System (opcional)
   - Addressables

### Paso 3: Configurar Assembly Definition
Si tu proyecto usa Assembly Definitions, configura la referencia:

```json
{
    "name": "TuProyectoCore",
    "references": [
        "GUID:8ee9e806686c7844786ccd111c1015ef"
    ]
}
```

El GUID `8ee9e806686c7844786ccd111c1015ef` corresponde a `HorrorCore.asmdef`.

---

## Configuración del Assembly Definition

### Referenciar HorrorCore en tu Proyecto

Si tu proyecto tiene un Assembly Definition, agrega la referencia:

**Opción 1: Por GUID (Recomendado)**
```json
{
    "name": "MiProyecto",
    "references": [
        "GUID:8ee9e806686c7844786ccd111c1015ef"
    ]
}
```

**Opción 2: Por Nombre**
Si ambos assemblies están en el mismo proyecto, puedes usar:
```json
{
    "name": "MiProyecto",
    "references": [
        "HorrorCore"
    ]
}
```

### Namespace
Todas las clases del motor están en el namespace `HorrorEngine`:

```csharp
using HorrorEngine;
```

---

## Sistemas Principales

### 1. Sistema de Jugador

#### Player
Clase singleton que gestiona la salud y estado del jugador.

```csharp
using HorrorEngine;

// Acceder a la instancia
Player player = Player.Instance;

// Obtener posición
Vector3 playerPos = Player.Instance.transform.position;

// Dañar al jugador
Player.Instance.TakeDamage(10);

// Curar al jugador
Player.Instance.Heal(5);
```

#### CHorrorController
Controlador de movimiento en primera persona.

**Componentes Requeridos:**
- `CharacterController`
- `CInteractRayCast`
- `Player`

**Configuración:**
```csharp
// En el Inspector:
// - moveSpeed: Velocidad de movimiento
// - jumpHeight: Altura del salto
// - mouseSensitivity: Sensibilidad del mouse
// - sprintSpeedMultiplier: Multiplicador de velocidad al correr
```

### 2. Sistema de Interacciones

#### CInteractRayCast
Sistema de raycast para detectar objetos interactuables.

**Uso:**
```csharp
using HorrorEngine;

// Obtener el componente
CInteractRayCast interactSystem = GetComponent<CInteractRayCast>();

// Habilitar/deshabilitar interacciones
interactSystem.SetInteractionsEnabled(true);
```

#### Iinteract Interface
Interface para crear objetos interactuables:

```csharp
using HorrorEngine;

public class MiInteractuable : MonoBehaviour, Iinteract
{
    public void Oninteract()
    {
        Debug.Log("¡Interactuado!");
    }
}
```

**Ejemplos de Interactuables Incluidos:**
- `KeyPickUp` - Recoger llaves
- `LightSwitch` - Interruptor de luz
- `LockedDoor` - Puerta con llave
- `CPhoneInteract` - Interacción con teléfono
- `StartYarnOnInteract` - Iniciar diálogo Yarn

### 3. Sistema de Enemigos

#### CEnemyManager
Gestor de enemigos con soporte para Addressables y Resources.

**Configuración en Inspector:**
- `autoLoadEnemies`: Carga automática de enemigos
- `enemyFolderPath`: Ruta de la carpeta de enemigos
- `useAddressables`: Usar Addressables para cargar
- `fallbackToResources`: Fallback a Resources si falla Addressables
- `spawnPoint`: Transform donde spawnear enemigos

**Uso:**
```csharp
using HorrorEngine;

// Obtener instancia
CEnemyManager enemyManager = CEnemyManager.Instance;

// Spawnear enemigo aleatorio
await enemyManager.SpawnRandomEnemyAsync();

// Obtener cantidad de enemigos
int count = enemyManager.GetEnemyCount();

// Limpiar todos los enemigos
enemyManager.ClearAllEnemies();
```

**Eventos:**
```csharp
// Suscribirse a eventos
CEnemyManager.Instance.OnEnemySpawned += OnEnemySpawned;
CEnemyManager.Instance.OnEnemyDestroyed += OnEnemyDestroyed;

void OnEnemySpawned(GameObject enemy)
{
    Debug.Log($"Enemigo spawnado: {enemy.name}");
}
```

### 4. Sistema de Eventos

#### CGameEventManager
Sistema de eventos desacoplado.

**Uso:**
```csharp
using HorrorEngine;

// Suscribirse a evento
CGameEventManager.Subscribe("MiEvento", OnMiEvento);

// Publicar evento
CGameEventManager.Publish("MiEvento");

// Desuscribirse
CGameEventManager.Unsubscribe("MiEvento", OnMiEvento);

void OnMiEvento()
{
    Debug.Log("Evento disparado!");
}
```

#### CGameEvents
Eventos predefinidos del sistema:

```csharp
using HorrorEngine;

// Eventos de diálogo
CGameEvents.OnDialogueStart.Subscribe(OnDialogueStart);
CGameEvents.OnDialogueEnd.Subscribe(OnDialogueEnd);

// Eventos de UI
CGameEvents.OnShowPauseMenu.Subscribe(OnShowPauseMenu);
CGameEvents.OnShowInventory.Subscribe(OnShowInventory);

// Eventos de sonido
CGameEvents.OnPlaySound.Subscribe(OnPlaySound);
```

### 5. Sistema de Sonido

#### CManagerSFX
Gestor de efectos de sonido.

**Configuración:**
```csharp
using HorrorEngine;

// Acceder a instancia
CManagerSFX sfxManager = CManagerSFX.Inst;

// Reproducir sonido por ID
sfxManager.PlaySound(0);

// Reproducir sonido de personaje
sfxManager.PlayCharacterSound(characterIndex);

// Reproducir sonido de reacción
sfxManager.PlayReactionSound("reactionType");

// Detener todos los sonidos
sfxManager.StopSFX();
```

**Configuración en Inspector:**
- `ListSFX`: Lista de AudioClips
- `audioMixer`: AudioMixer para control de volumen

### 6. Sistema de Niveles

#### CLevelManager
Gestor de carga de escenas.

**Uso:**
```csharp
using HorrorEngine;

// Obtener instancia
CLevelManager levelManager = CLevelManager.Inst;

// Cargar escena por nombre
levelManager.LoadScene("MiEscena");

// Cargar escena por índice
levelManager.LoadScene(1);

// Cargar escena asíncrona
levelManager.LoadSceneAsync("MiEscena");

// Cargar escena con eventos
levelManager.LoadSceneWithEvents("MiEscena");

// Obtener ID de escena actual
int currentScene = levelManager.GetCurrentSceneID();

// Recargar escena actual
levelManager.ReloadCurrentScene();
```

**Eventos:**
```csharp
// Suscribirse a eventos de carga
CLevelManager.OnSceneLoadStarted += OnSceneLoadStarted;
CLevelManager.OnSceneLoadCompleted += OnSceneLoadCompleted;
```

### 7. Sistema de Diálogos (Yarn)

#### DialogueController
Controlador de diálogos Yarn Spinner.

**Configuración:**
- Requiere un `YarnProject` configurado
- Requiere `DialogueRunner` de Yarn Spinner

**Uso:**
```csharp
using HorrorEngine;

// Obtener controlador
DialogueController dialogueController = GetComponent<DialogueController>();

// Iniciar diálogo
dialogueController.StartDialogue("MiNodo");

// El sistema se integra automáticamente con Yarn Spinner
```

### 8. Sistema de Items

#### Item Decorator Pattern
Sistema de items usando el patrón Decorator.

**Uso:**
```csharp
using HorrorEngine;

// Crear item base
BaseItem item = new BaseItem("Espada", 10);

// Decorar con boost de daño
IItem decoratedItem = new DamageBoostItemDecorator(item, 5);

// Usar item
int damage = decoratedItem.GetDamage(); // Retorna 15
```

### 9. Sistema de Flags

#### CFlagManager
Gestor de flags para progreso del juego.

**Uso:**
```csharp
using HorrorEngine;

// Obtener instancia
CFlagManager flagManager = CFlagManager.Instance;

// Establecer flag
flagManager.SetFlag("llaveRecogida", true);

// Obtener flag
bool tieneLlave = flagManager.GetFlag("llaveRecogida");

// Verificar si existe flag
bool existe = flagManager.HasFlag("llaveRecogida");
```

### 10. Sistema de Guardado

El motor incluye un sistema de guardado en `SaveSystem/`. Consulta los archivos en esa carpeta para más detalles.

---

## Configuración Inicial

### Paso 1: Configurar el Jugador

1. Crea un GameObject vacío llamado "Player"
2. Agrega los siguientes componentes:
   - `CharacterController`
   - `Player` (HorrorEngine)
   - `CHorrorController` (HorrorEngine)
   - `CInteractRayCast` (HorrorEngine)

3. Configura la cámara:
   - Crea una cámara como hijo del Player
   - Asigna la cámara a `CHorrorController.CameraTransform`
   - Asigna la cámara a `Player.playerCamera`

4. Configura el tag:
   - Asigna el tag "Player" al GameObject

### Paso 2: Configurar Managers

#### CManagerSFX
1. Crea un GameObject vacío "ManagerSFX"
2. Agrega el componente `CManagerSFX`
3. Configura la lista de AudioClips en el Inspector
4. (Opcional) Asigna un AudioMixer

#### CLevelManager
1. Crea un GameObject vacío "LevelManager"
2. Agrega el componente `CLevelManager`
3. Se inicializa automáticamente como Singleton

#### CEnemyManager
1. Crea un GameObject vacío "EnemyManager"
2. Agrega el componente `CEnemyManager`
3. Crea un GameObject vacío "SpawnPoint" y asigna su Transform
4. Configura las opciones en el Inspector:
   - `autoLoadEnemies`: true
   - `enemyFolderPath`: Ruta a tus prefabs de enemigos
   - `useAddressables`: true/false según tu configuración

### Paso 3: Configurar Prefabs

#### Enemigos
1. Crea prefabs de enemigos con el componente `CEnemy`
2. Nombra los prefabs con el prefijo "Enemy_" (ej: "Enemy_Zombie")
3. Colócalos en la carpeta configurada en `CEnemyManager`

#### Interactuables
1. Crea prefabs de objetos interactuables
2. Implementa `Iinteract` en tus scripts
3. Asegúrate de que estén en el Layer correcto para el raycast

---

## Uso Básico

### Ejemplo: Crear un Objeto Interactuable

```csharp
using UnityEngine;
using HorrorEngine;

public class MiObjetoInteractuable : MonoBehaviour, Iinteract
{
    public void Oninteract()
    {
        Debug.Log("¡Has interactuado conmigo!");
        // Tu lógica aquí
    }
}
```

### Ejemplo: Usar el Sistema de Eventos

```csharp
using UnityEngine;
using HorrorEngine;

public class MiScript : MonoBehaviour
{
    void Start()
    {
        // Suscribirse a eventos
        CGameEvents.OnDialogueStart.Subscribe(OnDialogueStart);
        CGameEvents.OnPlayerDeath.Subscribe(OnPlayerDeath);
    }

    void OnDestroy()
    {
        // Desuscribirse
        CGameEvents.OnDialogueStart.Unsubscribe(OnDialogueStart);
        CGameEvents.OnPlayerDeath.Unsubscribe(OnPlayerDeath);
    }

    void OnDialogueStart()
    {
        Debug.Log("Diálogo iniciado!");
    }

    void OnPlayerDeath(string cause)
    {
        Debug.Log($"Jugador murió por: {cause}");
    }
}
```

### Ejemplo: Spawnear Enemigos

```csharp
using UnityEngine;
using HorrorEngine;
using System.Threading.Tasks;

public class MiSpawner : MonoBehaviour
{
    async void Start()
    {
        // Esperar a que el sistema esté inicializado
        await Task.Delay(1000);

        // Spawnear enemigo aleatorio
        GameObject enemy = await CEnemyManager.Instance.SpawnRandomEnemyAsync();
        
        if (enemy != null)
        {
            Debug.Log($"Enemigo spawnado: {enemy.name}");
        }
    }
}
```

### Ejemplo: Cambiar de Escena

```csharp
using UnityEngine;
using HorrorEngine;

public class MiCambioEscena : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Cargar nueva escena
            CLevelManager.Inst.LoadScene("NuevaEscena");
        }
    }
}
```

---

## Ejemplos de Integración

### Integración con DeliriumArchne

El proyecto `DeliriumArchne` muestra cómo integrar el motor:

1. **Assembly Definition**: Referencia a `HorrorCore` mediante GUID
2. **Uso de Sistemas**: Los modos de juego usan los sistemas del motor
3. **Namespace**: Importa `using HorrorEngine;`

**Ejemplo de código:**
```csharp
using HorrorEngine;

namespace DeliriumArchne
{
    public class HorrorMode : GameModeStrategyBase
    {
        private CHorrorController horrorController;
        private CEnemyManager enemyManager;
        private CManagerSFX sfxManager;

        private void InitializeHorrorSystems()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                horrorController = player.GetComponent<CHorrorController>();
                if (horrorController == null)
                {
                    horrorController = player.AddComponent<CHorrorController>();
                }
            }

            enemyManager = CEnemyManager.Instance;
            sfxManager = CManagerSFX.Inst;
        }
    }
}
```

---

## Troubleshooting

### Error: "The type or namespace name 'HorrorEngine' could not be found"

**Solución:**
1. Verifica que el Assembly Definition de tu proyecto referencia a `HorrorCore`
2. Asegúrate de tener `using HorrorEngine;` en tus scripts
3. Recompila los assemblies (Assets > Reimport All)

### Error: "CEnemyManager.Instance is null"

**Solución:**
1. Asegúrate de que existe un GameObject con el componente `CEnemyManager` en la escena
2. El sistema se inicializa automáticamente, pero puede tardar un frame
3. Usa `await Task.Delay(100)` antes de acceder a la instancia

### Error: "Player.Instance is null"

**Solución:**
1. Verifica que existe un GameObject con tag "Player" y el componente `Player`
2. El componente debe estar activo en la escena
3. El sistema usa Singleton, asegúrate de que solo hay una instancia

### Los enemigos no se cargan

**Solución:**
1. Verifica la ruta en `CEnemyManager.enemyFolderPath`
2. Asegúrate de que los prefabs tienen el prefijo "Enemy_"
3. Si usas Addressables, verifica que los assets están marcados como Addressable
4. Activa `fallbackToResources` si Addressables falla

### El sistema de interacciones no funciona

**Solución:**
1. Verifica que el Player tiene el componente `CInteractRayCast`
2. Asegúrate de que la cámara está asignada en `CInteractRayCast.mainCamera`
3. Verifica que los objetos interactuables implementan `Iinteract`
4. Comprueba que los objetos están en el Layer correcto

---

## Estructura de Carpetas

```
Assets/1.FirstPersonTerrorGameEngine/
├── Scripts/
│   ├── Character/          # Sistema de personajes
│   ├── Core/              # Núcleo del sistema
│   ├── Dialogue/          # Sistema de diálogos Yarn
│   ├── Enemy/             # Sistema de enemigos
│   ├── Event/             # Sistema de eventos
│   ├── Horror/            # Controladores de terror
│   ├── Interacts/          # Objetos interactuables
│   ├── Items/              # Sistema de items
│   ├── Managers/           # Managers del sistema
│   ├── Patterns/           # Patrones de diseño
│   ├── SaveSystem/         # Sistema de guardado
│   └── ...
├── Prefab/                 # Prefabs del motor
├── Sound/                  # Sonidos
└── HorrorCore.asmdef       # Assembly Definition
```

---

## Recursos Adicionales

### Documentación de Sistemas Específicos
- Consulta los comentarios XML en los scripts para documentación detallada
- Los ejemplos en `Examples/` muestran uso avanzado
- `RolSystemExample.cs` en `3.RolEngine` muestra integración con sistema de stats

### Soporte
- Revisa los logs de Unity para mensajes de debug
- Los sistemas tienen métodos `GetDebugInfo()` para diagnóstico
- Activa `enableDebugLogs` en los managers para más información

---

## Notas Finales

- El motor está diseñado para ser modular y extensible
- Todos los sistemas usan el patrón Singleton para fácil acceso
- Los eventos permiten desacoplamiento entre sistemas
- El namespace `HorrorEngine` agrupa todas las clases del motor
- La arquitectura ECS está presente en algunos sistemas (enemigos, items)

¡Disfruta creando tu juego de terror!

