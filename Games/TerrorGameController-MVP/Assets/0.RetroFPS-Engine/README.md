# Retro FPS Engine - Motor FPS Retro

## 📖 Descripción General

**Retro FPS Engine** es un motor de juego especializado en juegos de disparos en primera persona (FPS) con estética retro. Está diseñado para ser simple, eficiente y reutilizable, proporcionando una base sólida para crear experiencias FPS clásicas.

## 🎯 Características Principales

### ✅ **Funcionalidades Core**
- **Sistema de Movimiento**: Controladores 2D y 3D optimizados
- **Sistema de Armas**: Gestión básica de armas y munición
- **Audio Retro**: Sistema SFX y música con mixer
- **Gestión de Niveles**: Carga y transición de escenas
- **Interacciones**: Sistema básico de objetos interactuables

### ✅ **Funcionalidades Avanzadas**
- **Addressables Integration**: Carga asíncrona básica de assets ([Guía](Documentation/Addressables_Guide.md))
- **Sistema de Diálogos**: Conversaciones simples para narrativa ([Sistema](Documentation/Dialogue_System.md))
- **Sistema de Eventos**: Comunicación desacoplada entre componentes
- **Scriptable Objects**: Configuración de items y datos
- **Object Pooling**: Gestión eficiente de instancias ([Sistema](Documentation/Object_Pooling.md))

### ✅ **Patrones de Diseño Completos**
- **Template Method Pattern**: Estandarización de managers ([Guía](Documentation/Template_Method.md))
- **Observer Pattern**: Sistema reactivo de notificaciones ([Sistema](Documentation/Observer_Pattern.md))
- **Event Bus Pattern**: Comunicación desacoplada global ([Sistema](Documentation/EventBus_System.md))
- **Command Pattern**: Encapsulación de acciones ejecutables ([Sistema](Documentation/Command_Pattern.md))
- **Decorator Pattern**: Modificación dinámica de items ([Sistema](Documentation/Decorator_Pattern.md))
- **Object Pooling**: Gestión eficiente de recursos ([Sistema](Documentation/Object_Pooling.md))
- **Global Variables**: Estado global del juego con persistencia
- **Singleton Pattern**: Instancias únicas de managers

### ✅ **Arquitectura**
- **Assembly Definition**: `FPSCore` para compilación modular
- **Namespaces Organizados**: `RetroFPS.*` para claridad
- **Template Method**: `BaseManager` para estandarización
- **Observer System**: Comunicación reactiva entre componentes
- **Event Bus**: Arquitectura orientada a eventos
- **Object Pooling**: Optimización de performance
- **Interfaces**: `IDamage`, `Iinteract`, `IItem`, `ICommand` para extensibilidad

## 🏗️ Arquitectura del Motor

```
Retro FPS Engine v2.0
├── Core Architecture
│   ├── BaseManager (Template Method)
│   ├── GlobalVariables (Singleton)
│   └── Assembly Definition (FPSCore)
├── Design Patterns
│   ├── Communication
│   │   ├── EventBus System
│   │   └── Observer Pattern
│   ├── Object Management
│   │   ├── Command Pattern
│   │   ├── Decorator Pattern
│   │   └── Object Pooling
│   └── Behavioral
│       └── Template Method
├── Game Systems
│   ├── Core Systems
│   │   ├── Movement (2D/3D Controllers)
│   │   ├── Weapons (Basic weapon system)
│   │   ├── Audio (SFX + Music with Pooling)
│   │   └── Level Management
│   ├── Advanced Features
│   │   ├── Addressables Integration
│   │   ├── Dialogue System
│   │   ├── Event-Driven Architecture
│   │   └── Reactive UI System
│   └── Items & Inventory
│       ├── Decorator-Based Items
│       └── Command-Based Interactions
└── Utilities
    ├── Scriptable Objects
    ├── Interfaces (IDamage, Iinteract, IItem, ICommand)
    ├── Managers (Template Method based)
    └── Performance (Object Pooling, Global State)
```

## 🎨 **Patrones de Diseño Implementados**

### **📐 Template Method Pattern**
**Propósito**: Define el esqueleto de un algoritmo en `BaseManager`, permitiendo que subclases personalicen pasos específicos.

**Implementaciones**:
- `CManagerSFXTemplate` - Manager de audio con pooling
- `CEnemyManagerTemplate` - Manager de enemigos con spawning
- `BaseManager` - Clase base para todos los managers

**Beneficios**: Consistencia, reutilización, mantenibilidad, testing estructurado.

### **👁️ Observer Pattern**
**Propósito**: Sistema reactivo de notificaciones para comunicación entre componentes sin dependencias directas.

**Componentes**:
- `GameObserver<T>` - Observer genérico type-safe
- `GameObservers` - Observers globales del juego (salud, score, estado, etc.)
- Integración automática con `GlobalVariables`

**Casos de Uso**: UI reactiva, audio dinámico, achievements, analytics.

### **📢 Event Bus Pattern**
**Propósito**: Sistema centralizado de comunicación desacoplada para eventos globales del juego.

**Arquitectura**:
- `EventBus` - Bus centralizado estático
- `IEvent` - Interface base para eventos
- `GameEvents` - Eventos específicos del juego
- Compatibilidad con sistema legacy `CGameEvent`

**Eventos Incluidos**: Player events, enemy events, weapon events, UI events, dialogue events.

### **⚔️ Command Pattern**
**Propósito**: Encapsula acciones ejecutables permitiendo undo/redo, queuing, y parametrización.

**Implementaciones**:
- `ICommand` - Interface base
- `InteractableCommand` - Clase base para comandos interactivos
- `OpenDoorCommand`, `PickupItemCommand`, `UseSwitchCommand` - Comandos específicos

**Características**: Undo/redo, validation, chaining, integration con EventBus.

### **🎨 Decorator Pattern**
**Propósito**: Agrega responsabilidades a objetos dinámicamente sin modificar su estructura base.

**Aplicación**: Sistema de items con modificadores dinámicos.

**Decorators Incluidos**:
- `DamagedItemDecorator` - Items dañados con efectividad reducida
- `EnchantedItemDecorator` - Items encantados con bonificaciones (Fire, Ice, Speed, etc.)
- Soporte para chaining ilimitado

### **🏊 Object Pooling Pattern**
**Propósito**: Gestiona eficientemente instancias de objetos reutilizando objetos existentes.

**Sistema Completo**:
- `ObjectPool<T>` - Pool genérico con auto-expansion
- `PoolManager` - Singleton centralizado
- `PoolReturner<T>` - Auto-retorno al pool
- Integración con Addressables

**Aplicaciones**: Projectiles, enemies, particles, audio sources, UI elements.

### **🌍 Global Variables System**
**Propósito**: Sistema centralizado de estado global con persistencia automática.

**Características**:
- Singleton con observers integrados
- Variables críticas del juego (health, ammo, score, keys, etc.)
- Guardado/carga automático con PlayerPrefs
- Notificaciones reactivas de cambios
- Modo debug y cheats

### **🔗 Integración de Patrones**

Los patrones están completamente integrados:
- **Template Method** proporciona estructura base
- **Observer** habilita comunicación reactiva
- **Event Bus** maneja eventos globales
- **Command** encapsula acciones ejecutables
- **Decorator** modifica items dinámicamente
- **Object Pooling** optimiza performance
- **Global Variables** mantiene estado consistente

**Flujo Típico**: `Input → Command → EventBus → Observer → UI Update → GlobalVariables Save`

## 📋 Requisitos del Sistema

### **Unity Version**
- Unity 2021.3+ (recomendado)
- Compatible con Unity 2020.3+

### **Dependencias**
```json
// Package Manager Requirements
{
  "Addressables": "1.21.0+",
  "Input System": "1.5.0+"
}
```

### **Plataformas Soportadas**
- ✅ Windows
- ✅ MacOS
- ✅ Linux
- ❌ WebGL (limitaciones de Addressables)
- ❌ Consolas (requiere adaptación)

## 🚀 Inicio Rápido

### 1. **Instalación**
```bash
# Clona el repositorio
git clone https://github.com/your-repo/RetroFPSEngine.git

# O copia los archivos a tu proyecto Unity
# Assets/0.RetroFPS-Engine/ → TuProyecto/Assets/
```

### 2. **Configuración Inicial**
```csharp
// En tu escena principal, agrega el GameManager
using RetroFPS.Music;

// Ejemplo básico de uso
void Start()
{
    // Inicializar managers
    var gameManager = CGameManager.Instance;
    var audioManager = CManagerSFX.Inst;
    var levelManager = CLevelManager.Inst;
}
```

### 3. **Configuración de Addressables**
1. Abre `Window → Asset Management → Addressables → Groups`
2. Crea grupo "RetroFPS_Assets"
3. Marca prefabs como Addressable
4. Build Addressables: `Window → Asset Management → Addressables → Build → New Build`

## 🎮 Uso Básico

### **Sistema de Addressables**
```csharp
// Cargar asset de manera asíncrona
GameObject enemy = await CAssetManager.Instance.LoadAssetAsync<GameObject>("Enemy_Basic");
Instantiate(enemy, spawnPosition, Quaternion.identity);

// O cargar e instanciar directamente
GameObject enemy2 = await CAssetManager.Instance.InstantiateAssetAsync("Enemy_Basic", spawnPosition);
```

### **Sistema de Diálogos**
```csharp
// Diálogo simple
var dialogue = new DialogueData("NPC", "¡Hola, aventurero!");
CDialogueManager.Instance.ShowDialogue(dialogue);

// Diálogo con opciones
var dialogueWithChoices = new DialogueData("NPC", "¿Me ayudas?", new string[] { "Sí", "No" });
CDialogueManager.Instance.ShowDialogueWithOptions(dialogueWithChoices, OnChoiceMade);
```

### **Uso Completo**
Ver [Ejemplos de Uso](Documentation/Example_Usage.cs) para implementaciones completas.

## 🎮 Uso Básico

### **Sistema de Movimiento**
```csharp
// Controlador básico de movimiento
public class PlayerController : MonoBehaviour
{
    private CPlayerController controller;

    void Start()
    {
        controller = GetComponent<CPlayerController>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        bool jump = Input.GetButtonDown("Jump");

        // Mover jugador
        controller.Move(moveX, moveY, jump);
    }
}
```

### **Sistema de Armas**
```csharp
// Ejemplo de uso de armas
public class WeaponController : MonoBehaviour
{
    private CWeapon currentWeapon;

    void Start()
    {
        currentWeapon = GetComponent<CWeapon>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            currentWeapon.Shoot();
        }
    }
}
```

### **Sistema de Audio**
```csharp
// Reproducir efectos de sonido
CManagerSFX.Inst.PlaySound(soundId);

// Reproducir música de fondo
CManagerMusic.Inst.PlayMusicBackground(trackId);
```

## 🎨 Sistema de Diálogos

### **Configuración Básica**
```csharp
// Crear diálogo simple
var dialogueData = new DialogueData
{
    speakerName = "NPC",
    message = "¡Hola, aventurero!",
    choices = new string[] { "Hola", "Adiós" }
};

// Mostrar diálogo
DialogueManager.Instance.ShowDialogue(dialogueData);
```

### **Ejemplo Completo**
```csharp
// En un NPC o trigger
public class NPCTalk : MonoBehaviour, Iinteract
{
    public void Oninteract()
    {
        var dialogue = new DialogueData
        {
            speakerName = "Guía",
            message = "¿Necesitas ayuda para navegar este nivel?",
            choices = new string[] {
                "Sí, explícame los controles",
                "No, gracias"
            }
        };

        DialogueManager.Instance.ShowDialogue(dialogue);
    }
}
```

## 📦 Addressables Integration

### **Carga Básica de Assets**
```csharp
// Cargar prefab con Addressables
public async Task<GameObject> LoadEnemyAsync(string enemyKey)
{
    var handle = Addressables.LoadAssetAsync<GameObject>(enemyKey);
    await handle.Task;

    if (handle.Status == AsyncOperationStatus.Succeeded)
    {
        return handle.Result;
    }

    return null;
}

// Uso
var enemyPrefab = await LoadEnemyAsync("Enemy_Basic");
if (enemyPrefab != null)
{
    Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
}
```

### **Gestión de Recursos**
```csharp
// Clase básica de gestión de recursos
public class ResourceManager : MonoBehaviour
{
    public async Task<T> LoadAssetAsync<T>(string key) where T : Object
    {
        var handle = Addressables.LoadAssetAsync<T>(key);
        await handle.Task;
        return handle.Result;
    }

    public void UnloadAsset(AsyncOperationHandle handle)
    {
        Addressables.Release(handle);
    }
}
```

## 🎯 Casos de Uso

### ✅ **Juegos Recomendados**
- **FPS Arcade**: Juegos tipo Doom, Wolfenstein
- **FPS Retro**: Estilo pixel art, low-poly
- **FPS Indie**: Proyectos pequeños con mecánicas clásicas
- **FPS Educativos**: Simuladores con controles simples

### ⚠️ **No Recomendado Para**
- **FPS Modernos**: Requiere más optimización
- **Multiplayer Masivo**: Necesita networking avanzado
- **VR/AR**: Requiere adaptación específica
- **Mobile**: Optimización para dispositivos móviles

## 🔧 Personalización y Extensión

### **Crear Nuevo Manager**
```csharp
using RetroFPS.Core;

namespace RetroFPS.Custom
{
    public class CustomManager : MonoBehaviour
    {
        public static CustomManager Instance { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        // Tu lógica personalizada aquí
    }
}
```

### **Extender Sistema de Armas**
```csharp
using RetroFPS.Weapon;

public class LaserGun : CWeapon
{
    public override void Shoot()
    {
        // Implementación específica de disparo láser
        base.Shoot(); // Llama a la lógica base

        // Efectos específicos del láser
        CreateLaserBeam();
        PlayLaserSound();
    }
}
```

## 🐛 Troubleshooting

### **Problemas Comunes**

#### **Error: "Assembly reference missing"**
```
Solución: Asegúrate de que FPSCore.asmdef esté en el proyecto
Verifica que todas las dependencias estén instaladas
```

#### **Error: "Addressables not initialized"**
```
Solución:
1. Abre Window → Asset Management → Addressables → Groups
2. Crea al menos un grupo
3. Build: Window → Asset Management → Addressables → Build
```

#### **Error: "Dialogue system not working"**
```
Solución:
1. Verifica que DialogueManager esté en la escena
2. Asegúrate de que la UI de diálogo esté configurada
3. Revisa que los datos de diálogo sean válidos
```

### **Debug Tools**
```csharp
// Habilitar debug en managers
CManagerSFX.Inst.enableDebugLogs = true;
CGameManager.enableDebugMode = true;

// Ver estado del sistema
Debug.Log($"Enemies loaded: {enemyManager.GetEnemyCount()}");
Debug.Log($"Current level: {levelManager.GetCurrentLevelName()}");
```

## 📊 Rendimiento

### **Recomendaciones de Optimización**
- **Object Pooling**: Usa para enemigos y proyectiles
- **LOD System**: Implementa para modelos distantes
- **Occlusion Culling**: Para niveles grandes
- **Texture Atlasing**: Reduce draw calls

### **Benchmarks Básicos**
- **Enemigos Simultáneos**: 50-100 (dependiendo de complejidad)
- **FPS Objetivo**: 60+ en PC moderno
- **Tamaño de Build**: ~50-100MB (dependiendo de assets)

## 📚 Documentación Completa

### **🎯 Patrones de Diseño**
- [Design Patterns Overview](Documentation/Design_Patterns.md) - **NUEVO** - Visión completa de todos los patrones implementados
- [Template Method Pattern](Documentation/Template_Method.md) - **NUEVO** - Guía completa del patrón Template Method
- [Observer Pattern](Documentation/Observer_Pattern.md) - **NUEVO** - Sistema reactivo de notificaciones
- [EventBus System](Documentation/EventBus_System.md) - **NUEVO** - Comunicación desacoplada global
- [Command Pattern](Documentation/Command_Pattern.md) - **NUEVO** - Acciones ejecutables con undo/redo
- [Decorator Pattern](Documentation/Decorator_Pattern.md) - **NUEVO** - Modificación dinámica de items
- [Object Pooling](Documentation/Object_Pooling.md) - **NUEVO** - Gestión eficiente de recursos

### **🔧 Sistemas Específicos**
- [Addressables Guide](Documentation/Addressables_Guide.md) - Sistema básico de Addressables
- [Dialogue System](Documentation/Dialogue_System.md) - Sistema de diálogos retro
- [Example Usage](Documentation/Example_Usage.cs) - Ejemplos completos de implementación

### **📋 Archivos de Referencia**
- [ListMechanics.txt](Scripts/0.Documents/ListMechanics.txt) - Lista completa de mecánicas implementadas
- [PatternsIntegration.cs](Scripts/Core/PatternsIntegration.cs) - **NUEVO** - Ejemplo de integración de todos los patrones

### **🏗️ Arquitectura**
- [FPSCore.asmdef](Scripts/FPSCore.asmdef) - Definición de assembly
- [Namespaces](Scripts/) - Estructura organizada por `RetroFPS.*`
- [BaseManager](Scripts/Core/BaseManager.cs) - **NUEVO** - Clase base para managers
- [GlobalVariables](Scripts/Core/GlobalVariables.cs) - **NUEVO** - Sistema de variables globales

## 🤝 Contribución

### **Estructura de Código**
```csharp
// Usar namespaces consistentes
namespace RetroFPS.{Categoria}

// Nombres de clases con prefijo C
public class CManagerName : MonoBehaviour

// Interfaces sin prefijo
public interface IWeapon

// Métodos virtuales para extensión
public virtual void Initialize() { }
```

### **Convenciones de Commit**
```
feat: nueva funcionalidad
fix: corrección de bug
docs: actualización de documentación
refactor: refactorización de código
```

## 📄 Licencia

Este proyecto está bajo licencia MIT. Ver archivo LICENSE para más detalles.

---

**Versión**: 2.0.0 - Design Patterns Edition
**Última actualización**: Enero 2026
**Compatible con**: Unity 2021.3+
**Estado**: Motor completo con arquitectura de patrones de diseño
**Patrones Implementados**: Template Method, Observer, EventBus, Command, Decorator, Object Pooling, Singleton
**Documentación**: Completa con ejemplos y guías detalladas
