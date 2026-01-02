# Sistemas Reutilizables

## Descripción General

Este documento describe los sistemas reutilizables implementados en el proyecto, que pueden ser utilizados en diferentes contextos y escenas para mantener consistencia y reducir la duplicación de código.

## Sistemas Principales

### 1. Sistema de Eventos (CGameEventManager)

#### Descripción
Sistema de eventos genérico que permite comunicación desacoplada entre componentes del juego.

#### Características
- **Eventos tipados** - Soporte para diferentes tipos de datos
- **Suscripción dinámica** - Agregar/quitar listeners en tiempo de ejecución
- **Eventos genéricos** - Flexibilidad para diferentes tipos de datos
- **Persistencia** - Los eventos se mantienen entre escenas

#### Implementación
```csharp
// Evento simple
CGameEventManager.Publish("EnemyKilled");

// Evento con datos
CGameEventManager.Publish("PlayerHealthChanged", 75);

// Suscripción
CGameEventManager.Subscribe("EnemyKilled", OnEnemyKilled);
CGameEventManager.Subscribe<int>("PlayerHealthChanged", OnHealthChanged);
```

#### Casos de Uso
- Notificaciones de eventos del juego
- Comunicación entre sistemas
- UI updates
- Logging de eventos

### 2. Sistema de Persistencia de Datos

#### Descripción
Sistema para guardar y cargar datos del juego de forma persistente.

#### Componentes
- **FileDataHandler** - Manejo de archivos
- **DataPersistenceManager** - Gestión centralizada
- **GameData** - Estructura de datos del juego

#### Implementación
```csharp
// Guardar datos
DataPersistenceManager.Instance.SaveGame();

// Cargar datos
DataPersistenceManager.Instance.LoadGame();

// Nuevo juego
DataPersistenceManager.Instance.NewGame();
```

#### Características
- **Encriptación opcional** - Seguridad de datos
- **Backup automático** - Prevención de pérdida de datos
- **Múltiples perfiles** - Soporte para varios jugadores
- **Auto-save** - Guardado automático configurable

### 3. Sistema de Gestión de Niveles (CLevelManager)

#### Descripción
Gestiona la carga y transición entre escenas/niveles del juego.

#### Características
- **Carga asíncrona** - No bloquea el juego
- **Eventos de progreso** - Notificaciones de estado
- **Múltiples modos** - Carga simple y aditiva
- **Persistencia** - Mantiene estado entre escenas

#### Implementación
```csharp
// Cargar escena
CLevelManager.Inst.LoadScene("Level1");

// Carga asíncrona
CLevelManager.Inst.LoadSceneAsync("Level1");

// Carga aditiva
CLevelManager.Inst.LoadSceneAsyncAdditive("UI");

// Verificar estado
if (CLevelManager.Inst.IsLoadingScene())
{
    // Mostrar loading screen
}
```

### 4. Sistema de Audio (CManagerSFX)

#### Descripción
Gestión centralizada de efectos de sonido y música.

#### Características
- **Pooling de AudioSources** - Optimización de rendimiento
- **Múltiples canales** - Separación de música y SFX
- **Control de volumen** - Ajustes independientes
- **Eventos de audio** - Integración con sistema de eventos

#### Implementación
```csharp
// Reproducir sonido
CManagerSFX.Inst.PlaySound(soundId);

// Detener todos los sonidos
CManagerSFX.Inst.StopSFX();

// Agregar nuevo AudioSource
CManagerSFX.Inst.AddSound();
```

### 5. Sistema de Input (InputManager)

#### Descripción
Gestión centralizada de entrada usando el nuevo Input System de Unity.

#### Características
- **Input System moderno** - Mejor rendimiento y flexibilidad
- **Mapeo de controles** - Configuración flexible
- **Eventos de input** - Integración con sistema de eventos
- **Soporte multiplataforma** - PC, móvil, consola

#### Implementación
```csharp
// Obtener dirección de movimiento
Vector2 moveDirection = InputManager.instance.GetMoveDirection();

// Verificar si se presionó salir
if (InputManager.instance.GetExitPressed())
{
    // Salir del juego
}
```

## Patrones de Diseño Implementados

### 1. Singleton Pattern
Utilizado en managers principales para acceso global:
- CGameManager
- CLevelManager
- CManagerSFX
- DataPersistenceManager

### 2. Observer Pattern
Implementado en el sistema de eventos:
- Suscripción/desuscripción dinámica
- Notificaciones automáticas
- Desacoplamiento de componentes

### 3. Factory Pattern
Implementado en el sistema de enemigos:
- Creación de objetos
- Object pooling
- Gestión de instancias

### 4. State Pattern
Preparado para gestión de estados del juego:
- Estados de juego (Playing, Paused, Menu)
- Transiciones entre estados
- Lógica específica por estado

## Configuración de Sistemas

### 1. Configuración Inicial
```csharp
// En GameManager o similar
void Awake()
{
    // Inicializar sistemas
    CGameEventManager.RegisterStaticEvents();
    
    // Configurar managers
    levelManager = CLevelManager.Inst;
    musicManager = CManagerMusic.Inst;
    audioManager = CManagerSFX.Inst;
}
```

### 2. Configuración de Eventos
```csharp
// Registrar eventos estáticos
public static void RegisterStaticEvents()
{
    // Eventos del juego
    Subscribe("GameStart", OnGameStart);
    Subscribe("GameOver", OnGameOver);
    Subscribe("LevelComplete", OnLevelComplete);
}
```

### 3. Configuración de Persistencia
```csharp
// En DataPersistenceManager
[SerializeField] private bool useEncryption = false;
[SerializeField] private string fileName = "gamedata.json";
[SerializeField] private bool autoSave = true;
[SerializeField] private float autoSaveInterval = 30f;
```

## Integración de Sistemas

### Flujo Típico de Juego
```
1. Inicio → CGameManager inicializa sistemas
2. Menú → CLevelManager carga escena de menú
3. Juego → CLevelManager carga nivel
4. Eventos → CGameEventManager maneja comunicación
5. Audio → CManagerSFX reproduce sonidos
6. Guardado → DataPersistenceManager guarda progreso
```

### Comunicación Entre Sistemas
```csharp
// Ejemplo: Enemigo muere
public void OnEnemyDeath()
{
    // Notificar evento
    CGameEventManager.Publish("EnemyKilled");
    
    // Reproducir sonido
    CManagerSFX.Inst.PlaySound(deathSoundId);
    
    // Actualizar UI
    CGameEventManager.Publish("ScoreUpdated", currentScore);
}
```

## Mejores Prácticas

### 1. Inicialización
- Inicializar sistemas en orden correcto
- Verificar dependencias antes de usar
- Usar try-catch para manejo de errores

### 2. Eventos
- Usar nombres descriptivos para eventos
- Documentar el propósito de cada evento
- Evitar eventos excesivamente frecuentes

### 3. Persistencia
- Validar datos antes de guardar
- Implementar backup y recuperación
- Considerar tamaño de archivos de guardado

### 4. Audio
- Usar pooling para AudioSources
- Implementar fade in/out
- Considerar diferentes dispositivos de audio

### 5. Input
- Proporcionar configuración de controles
- Implementar soporte para múltiples dispositivos
- Considerar accesibilidad

## Troubleshooting

### Problema: Eventos no funcionan
- Verificar que se registren los eventos
- Confirmar que los listeners estén activos
- Revisar nombres de eventos

### Problema: Datos no se guardan
- Verificar permisos de escritura
- Confirmar que el directorio existe
- Revisar formato de datos

### Problema: Audio no se reproduce
- Verificar configuración de AudioMixer
- Confirmar que los clips estén asignados
- Revisar volumen y mute

### Problema: Input no responde
- Verificar configuración del Input System
- Confirmar que los Action Maps estén activos
- Revisar bindings de controles

## Extensiones Futuras

### 1. Sistema de Logging
```csharp
public static class GameLogger
{
    public static void Log(string message, LogLevel level = LogLevel.Info);
    public static void LogError(string error);
    public static void LogWarning(string warning);
}
```

### 2. Sistema de Analytics
```csharp
public static class GameAnalytics
{
    public static void TrackEvent(string eventName, Dictionary<string, object> parameters);
    public static void TrackLevelStart(int levelId);
    public static void TrackLevelComplete(int levelId, float time);
}
```

### 3. Sistema de Localización
```csharp
public static class LocalizationManager
{
    public static string GetText(string key);
    public static void SetLanguage(string languageCode);
    public static string GetCurrentLanguage();
}
```

## Referencias

- [Unity Design Patterns](https://unity.com/how-to/unity-design-patterns)
- [Unity Best Practices](https://docs.unity3d.com/Manual/BestPracticeGuides.html)
- [Unity Performance Optimization](https://docs.unity3d.com/Manual/PerformanceOptimization.html) 