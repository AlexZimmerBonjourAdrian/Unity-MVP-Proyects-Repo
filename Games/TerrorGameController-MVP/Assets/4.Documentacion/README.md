# Documentación del Proyecto - Terror Game Controller MVP

## Descripción General

Este proyecto implementa un sistema completo de gestión de enemigos y assets para un juego de terror en primera persona, utilizando tecnologías modernas de Unity como Addressables, Object Pooling y sistemas modulares reutilizables.

## Estructura de Documentación

### 📚 Documentación Técnica

#### [Sistema de Addressables](./Addressables_System.md)
- Gestión eficiente de assets con carga asíncrona
- Sistema de caché para optimización de rendimiento
- Configuración y mejores prácticas
- Troubleshooting y casos de uso

#### [Sistema Factory](./Factory_System.md)
- Implementación de Object Pooling
- Gestión de instancias de GameObjects
- Thread-safe operations
- Integración con otros sistemas

#### [Sistemas Reutilizables](./Reusable_Systems.md)
- Sistema de eventos (CGameEventManager)
- Persistencia de datos
- Gestión de niveles (CLevelManager)
- Sistema de audio (CManagerSFX)
- Sistema de input (InputManager)

## Arquitectura del Proyecto

### Diagrama de Sistema
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Addressables  │    │  CEnemyManager  │    │     Factory     │
│                 │    │                 │    │                 │
│ • Asset Loading │◄──►│ • Coordination  │◄──►│ • Object Pooling│
│ • Caching       │    │ • Wave System   │    │ • Instance Mgmt │
│ • Memory Mgmt   │    │ • Event System  │    │ • Thread Safety │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Asset Cache   │    │  Enemy Instances│    │  Pooled Objects │
│                 │    │                 │    │                 │
│ • Prefabs       │    │ • Active Enemies│    │ • Reusable Objs │
│ • Textures      │    │ • Wave Tracking │    │ • Performance   │
│ • Audio         │    │ • Event Triggers│    │ • Memory Opt    │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

### Flujo de Datos
1. **Addressables** carga assets desde disco/CDN
2. **CEnemyManager** coordina la lógica de juego
3. **Factory** crea y gestiona instancias con Object Pooling
4. **Sistemas de soporte** manejan eventos, audio, input, etc.

## Características Principales

### 🚀 Rendimiento
- **Carga asíncrona** de assets
- **Object Pooling** para optimización
- **Caché inteligente** de recursos
- **Thread-safe** operations

### 🔧 Modularidad
- **Separación de responsabilidades** clara
- **Sistemas reutilizables** entre escenas
- **Configuración centralizada**
- **API consistente**

### 🛡️ Robustez
- **Validaciones completas** en cada paso
- **Manejo de errores** gracioso
- **Logging detallado** para debugging
- **Recuperación automática** de fallos

## Configuración Rápida

### 1. Instalación de Dependencias
```bash
# Unity Package Manager
- Addressables (1.21.0+)
- Input System (1.5.0+)
- TextMeshPro (3.0.0+)
```

### 2. Configuración de Addressables
1. Abrir `Window → Asset Management → Addressables → Groups`
2. Crear grupos para enemigos, efectos, audio
3. Marcar prefabs como Addressable
4. Build de Addressables

### 3. Configuración del Sistema
1. Asignar `ResourcesPathDirection` en CEnemyManager
2. Configurar `spawnPoint` para enemigos
3. Ajustar parámetros de oleadas
4. Configurar eventos del sistema

## Uso Básico

### Spawn de Enemigos
```csharp
// Spawn manual
await CEnemyManager.Instance.SpawnEnemyAsync();

// Sistema de oleadas
CEnemyManager.Instance.StartWaves();

// Spawn con tecla T (configurado)
if (Input.GetKeyDown(KeyCode.T))
{
    await CEnemyManager.Instance.SpawnEnemyAsync();
}
```

### Gestión de Eventos
```csharp
// Suscribirse a eventos
CEnemyManager.Instance.OnEnemySpawned += OnEnemySpawned;
CEnemyManager.Instance.OnEnemyDestroyed += OnEnemyDestroyed;

// Publicar eventos
CGameEventManager.Publish("EnemyKilled");
CGameEventManager.Publish("WaveComplete", currentWave);
```

### Persistencia de Datos
```csharp
// Guardar progreso
DataPersistenceManager.Instance.SaveGame();

// Cargar progreso
DataPersistenceManager.Instance.LoadGame();

// Nuevo juego
DataPersistenceManager.Instance.NewGame();
```

## Casos de Uso

### 🎮 Juego de Terror
- Spawn dinámico de enemigos
- Oleadas configurables
- Efectos de sonido atmosféricos
- Sistema de progresión

### 🏗️ Prototipado Rápido
- Configuración rápida de enemigos
- Sistema modular reutilizable
- Debugging avanzado
- Iteración rápida

### 📱 Multiplataforma
- Optimización de memoria
- Carga eficiente de assets
- Sistema de input flexible
- Escalabilidad

## Troubleshooting

### Problemas Comunes

#### Error: "Asset not found"
- Verificar configuración de Addressables
- Confirmar build de Addressables
- Revisar rutas en ResourcesPathDirection

#### Error: "SpawnPoint not assigned"
- Asignar Transform en inspector
- Verificar que el GameObject esté activo
- Confirmar configuración en CEnemyManager

#### Performance Issues
- Revisar tamaño de assets
- Optimizar configuración de pools
- Monitorear uso de memoria

### Logs de Debug
El sistema incluye logging detallado:
```
[INFO] Iniciando carga de assets de enemigos...
[INFO] Asset de enemigo cargado exitosamente: SpiderEnemy
[INFO] Enemigo spawnado exitosamente: SpiderEnemy
[INFO] Iniciando oleada 1 con 5 enemigos
```

## Mejores Prácticas

### 🎯 Desarrollo
- Usar nombres descriptivos para assets
- Documentar eventos del sistema
- Implementar validaciones robustas
- Seguir patrones de diseño establecidos

### 🔧 Optimización
- Precargar assets críticos
- Configurar pools apropiados
- Monitorear rendimiento
- Liberar recursos no utilizados

### 🧪 Testing
- Probar en diferentes dispositivos
- Validar comportamiento offline
- Verificar manejo de errores
- Testear casos extremos

## Contribución

### Estructura de Código
- Seguir convenciones de naming
- Documentar métodos públicos
- Implementar validaciones
- Mantener separación de responsabilidades

### Testing
- Probar nuevas funcionalidades
- Validar integración con sistemas existentes
- Verificar rendimiento
- Documentar cambios

## Referencias

### Documentación Unity
- [Addressables Documentation](https://docs.unity3d.com/Packages/com.unity.addressables@latest)
- [Input System Documentation](https://docs.unity3d.com/Packages/com.unity.inputsystem@latest)
- [Performance Optimization](https://docs.unity3d.com/Manual/PerformanceOptimization.html)

### Patrones de Diseño
- [Object Pooling Pattern](https://en.wikipedia.org/wiki/Object_pool_pattern)
- [Factory Pattern](https://en.wikipedia.org/wiki/Factory_method_pattern)
- [Observer Pattern](https://en.wikipedia.org/wiki/Observer_pattern)

### Recursos Adicionales
- [Unity Best Practices](https://docs.unity3d.com/Manual/BestPracticeGuides.html)
- [Unity Performance Best Practices](https://docs.unity3d.com/Manual/PerformanceOptimization.html)
- [Unity Design Patterns](https://unity.com/how-to/unity-design-patterns)

---

**Versión:** 1.0.0  
**Última actualización:** Diciembre 2024  
**Autor:** Equipo de Desarrollo  
**Licencia:** Propietaria 