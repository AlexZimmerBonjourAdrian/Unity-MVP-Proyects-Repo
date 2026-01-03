# Plan de Migración Futura a ECS (Entity Component System)

## Estado Actual

**IMPORTANTE**: Este proyecto **NO utiliza arquitectura ECS** actualmente. La arquitectura implementada es **modular basada en MonoBehaviour** con patrones de diseño como Singleton, Factory, Observer, etc.

Este documento describe un plan de migración futuro a ECS si fuera necesario para optimización de rendimiento o escalabilidad.

## ¿Cuándo Considerar Migrar a ECS?

### Indicadores para Migración

1. **Rendimiento**
   - Más de 1000 entidades activas simultáneamente
   - Problemas de framerate con muchas entidades
   - Necesidad de procesamiento paralelo (multithreading)

2. **Escalabilidad**
   - Juegos con miles de enemigos/proyectiles
   - Simulaciones complejas (física, IA masiva)
   - Necesidad de optimización de memoria

3. **Networking Avanzado**
   - Sincronización masiva de entidades
   - Autoridad del servidor compleja
   - Replicación eficiente de estado

### Cuándo NO Migrar

- Proyectos pequeños o medianos (< 500 entidades)
- Arquitectura actual funciona bien
- Tiempo de desarrollo limitado
- Equipo sin experiencia en ECS

## Arquitectura Actual vs ECS

### Arquitectura Actual (Modular)

```
GameObject (MonoBehaviour)
├── CEnemy (lógica + datos)
├── CEnemyController (control)
└── CBodyPart (componentes físicos)
```

**Ventajas:**
- Fácil de entender
- Compatible con Unity estándar
- Buen rendimiento para proyectos medianos
- Fácil de mantener

**Desventajas:**
- Menos eficiente con muchas entidades
- Difícil paralelizar
- Acoplamiento entre lógica y datos

### Arquitectura ECS (Futura)

```
Entity (ID único)
├── Component (datos puros - structs)
│   ├── HealthComponent
│   ├── MovementComponent
│   └── PositionComponent
└── System (lógica pura - procesa componentes)
    ├── MovementSystem
    ├── HealthSystem
    └── RenderSystem
```

**Ventajas:**
- Excelente rendimiento
- Fácil paralelizar
- Separación clara datos/lógica
- Escalable a miles de entidades

**Desventajas:**
- Curva de aprendizaje
- Refactorización completa
- Más complejo de mantener

## Plan de Migración (Fase por Fase)

### Fase 0: Preparación (Actual)

**Estado**: ✅ Completado

- Arquitectura modular funcional
- Sistemas reutilizables implementados
- Documentación actualizada

### Fase 1: Análisis y Diseño (2-3 semanas)

#### 1.1 Identificar Entidades Actuales

**Entidades a Migrar:**
- `CEnemy` → Entity con componentes
- `Player` → Entity con componentes
- Objetos interactuables → Entities
- Proyectiles → Entities

**Componentes Necesarios:**
```csharp
// Componentes de datos (structs)
public struct HealthComponent
{
    public float currentHealth;
    public float maxHealth;
}

public struct MovementComponent
{
    public Vector3 velocity;
    public float speed;
    public bool isGrounded;
}

public struct PositionComponent
{
    public Vector3 position;
    public Quaternion rotation;
}

public struct EnemyComponent
{
    public EnemyType type;
    public float attackCooldown;
}
```

#### 1.2 Diseñar Sistemas ECS

**Sistemas Necesarios:**
```csharp
// Sistemas de procesamiento
public class MovementSystem : ISystem
{
    public void Update(EntityManager entityManager, float deltaTime)
    {
        // Procesar todas las entidades con MovementComponent
    }
}

public class HealthSystem : ISystem
{
    public void Update(EntityManager entityManager, float deltaTime)
    {
        // Procesar todas las entidades con HealthComponent
    }
}

public class EnemyAISystem : ISystem
{
    public void Update(EntityManager entityManager, float deltaTime)
    {
        // Procesar todas las entidades con EnemyComponent
    }
}
```

#### 1.3 Elegir Framework ECS

**Opciones:**

1. **Unity DOTS (Data-Oriented Technology Stack)**
   - Oficial de Unity
   - Entities, Components, Systems
   - Job System para paralelización
   - **Recomendado para proyectos nuevos**

2. **Entitas-CSharp**
   - Framework ECS popular
   - Code generation
   - Buena documentación
   - **Recomendado para proyectos existentes**

3. **Custom ECS**
   - Control total
   - Más trabajo
   - **Solo si tienes experiencia**

**Recomendación**: Unity DOTS para proyectos nuevos, Entitas-CSharp para migración gradual.

### Fase 2: Implementación Base (4-6 semanas)

#### 2.1 Crear Core ECS

**Estructura Base:**
```
Assets/
└── Core/
    └── ECS/
        ├── Entity.cs
        ├── Component.cs
        ├── System.cs
        ├── EntityManager.cs
        └── World.cs
```

**Implementación Mínima:**
```csharp
// Entity.cs
public struct Entity
{
    public int id;
    public static Entity None => new Entity { id = -1 };
}

// Component.cs
public interface IComponent { }

// EntityManager.cs
public class EntityManager
{
    private Dictionary<int, Dictionary<Type, IComponent>> entities;
    private int nextEntityId = 0;
    
    public Entity CreateEntity()
    {
        // Crear nueva entidad
    }
    
    public void AddComponent<T>(Entity entity, T component) where T : IComponent
    {
        // Agregar componente a entidad
    }
    
    public T GetComponent<T>(Entity entity) where T : IComponent
    {
        // Obtener componente de entidad
    }
}
```

#### 2.2 Migrar Sistema Más Simple Primero

**Estrategia**: Empezar con el sistema más simple (ej: HealthSystem)

1. Crear `HealthComponent` (struct)
2. Crear `HealthSystem` (procesa componentes)
3. Migrar `CEnemy.health` → `HealthComponent`
4. Probar y validar
5. Migrar siguiente sistema

#### 2.3 Integración Híbrida

**Durante la migración**, mantener ambos sistemas:

```csharp
// Sistema híbrido temporal
public class HybridEnemy : MonoBehaviour
{
    private Entity ecsEntity;
    private HealthComponent healthComponent;
    
    void Start()
    {
        // Crear entidad ECS
        ecsEntity = EntityManager.Instance.CreateEntity();
        healthComponent = new HealthComponent { currentHealth = 100, maxHealth = 100 };
        EntityManager.Instance.AddComponent(ecsEntity, healthComponent);
    }
    
    void Update()
    {
        // Sincronizar ECS → MonoBehaviour (temporal)
        healthComponent = EntityManager.Instance.GetComponent<HealthComponent>(ecsEntity);
        // Usar healthComponent.currentHealth
    }
}
```

### Fase 3: Migración Completa (8-12 semanas)

#### 3.1 Migrar Todos los Sistemas

**Orden Recomendado:**
1. Health System (más simple)
2. Position/Movement System
3. Enemy AI System
4. Interaction System
5. Rendering System (último)

#### 3.2 Eliminar Código Legacy

**Solo después de validar que todo funciona:**
- Eliminar MonoBehaviour legacy
- Eliminar código híbrido
- Limpiar referencias

#### 3.3 Optimización

- Implementar Job System para paralelización
- Optimizar queries de componentes
- Profiling y ajustes

### Fase 4: Networking con ECS (Opcional)

#### 4.1 Serialización de Componentes

```csharp
// Componentes deben ser serializables
[Serializable]
public struct HealthComponent : IComponent, INetworkSerializable
{
    public float currentHealth;
    public float maxHealth;
    
    public void Serialize(NetworkWriter writer)
    {
        writer.Write(currentHealth);
        writer.Write(maxHealth);
    }
    
    public void Deserialize(NetworkReader reader)
    {
        currentHealth = reader.ReadFloat();
        maxHealth = reader.ReadFloat();
    }
}
```

#### 4.2 Replicación de Estado

```csharp
// Sistema de replicación
public class NetworkReplicationSystem : ISystem
{
    public void Update(EntityManager entityManager, float deltaTime)
    {
        // Replicar componentes a clientes
        var entities = entityManager.GetEntitiesWithComponent<NetworkComponent>();
        foreach (var entity in entities)
        {
            var networkComp = entityManager.GetComponent<NetworkComponent>(entity);
            if (networkComp.isDirty)
            {
                ReplicateToClients(entity);
                networkComp.isDirty = false;
            }
        }
    }
}
```

## Ejemplo de Migración: Sistema de Enemigos

### Antes (Arquitectura Actual)

```csharp
public class CEnemy : MonoBehaviour
{
    private float currentHealth = 100;
    private float maxHealth = 100;
    private Vector3 position;
    
    void Update()
    {
        // Lógica de movimiento
        position += velocity * Time.deltaTime;
        transform.position = position;
        
        // Lógica de salud
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
```

### Después (Arquitectura ECS)

```csharp
// Componentes (datos)
public struct HealthComponent : IComponent
{
    public float currentHealth;
    public float maxHealth;
}

public struct PositionComponent : IComponent
{
    public Vector3 position;
    public Quaternion rotation;
}

public struct MovementComponent : IComponent
{
    public Vector3 velocity;
    public float speed;
}

public struct EnemyComponent : IComponent
{
    public EnemyType type;
}

// Sistemas (lógica)
public class MovementSystem : ISystem
{
    public void Update(EntityManager entityManager, float deltaTime)
    {
        var entities = entityManager.GetEntitiesWithComponents<PositionComponent, MovementComponent>();
        
        foreach (var entity in entities)
        {
            var position = entityManager.GetComponent<PositionComponent>(entity);
            var movement = entityManager.GetComponent<MovementComponent>(entity);
            
            position.position += movement.velocity * deltaTime;
            
            entityManager.SetComponent(entity, position);
        }
    }
}

public class HealthSystem : ISystem
{
    public void Update(EntityManager entityManager, float deltaTime)
    {
        var entities = entityManager.GetEntitiesWithComponent<HealthComponent>();
        
        foreach (var entity in entities)
        {
            var health = entityManager.GetComponent<HealthComponent>(entity);
            
            if (health.currentHealth <= 0)
            {
                // Eliminar entidad o marcar para eliminación
                EntityManager.Instance.DestroyEntity(entity);
            }
        }
    }
}
```

## Checklist de Migración

### Pre-Migración
- [ ] Analizar rendimiento actual
- [ ] Identificar cuellos de botella
- [ ] Decidir si ECS es necesario
- [ ] Elegir framework ECS
- [ ] Capacitar equipo en ECS

### Durante Migración
- [ ] Crear core ECS
- [ ] Migrar sistema más simple
- [ ] Validar funcionamiento
- [ ] Migrar sistemas restantes
- [ ] Mantener compatibilidad temporal

### Post-Migración
- [ ] Eliminar código legacy
- [ ] Optimizar rendimiento
- [ ] Documentar cambios
- [ ] Actualizar tests
- [ ] Validar con usuarios

## Consideraciones Especiales

### Networking

**Con Arquitectura Modular Actual:**
- Más fácil de implementar
- Unity Netcode funciona bien
- Sincronización directa de GameObjects

**Con ECS:**
- Requiere serialización de componentes
- Más eficiente para muchas entidades
- Mejor para autoridad del servidor

**Recomendación**: Si el networking es crítico y tienes muchas entidades, ECS puede ayudar. Si no, la arquitectura modular es suficiente.

### Compatibilidad con Unity

**Arquitectura Modular:**
- 100% compatible con Unity estándar
- Funciona con todos los sistemas de Unity
- Fácil integración con assets

**ECS:**
- Requiere DOTS o framework externo
- Algunos sistemas de Unity no compatibles
- Necesita adaptación de assets

### Tiempo de Desarrollo

**Arquitectura Modular:**
- Desarrollo rápido
- Fácil de mantener
- Bueno para MVP

**ECS:**
- Desarrollo más lento inicialmente
- Curva de aprendizaje
- Mejor a largo plazo para proyectos grandes

## Recursos y Referencias

### Documentación Oficial
- [Unity DOTS Documentation](https://docs.unity3d.com/Packages/com.unity.entities@latest)
- [Entitas-CSharp Documentation](https://github.com/sschmid/Entitas-CSharp)

### Tutoriales
- Unity Learn: DOTS Fundamentals
- Entitas-CSharp Examples
- ECS Best Practices

### Herramientas
- Unity Profiler (para identificar cuellos de botella)
- Unity DOTS Samples
- Entitas-CSharp Visual Debugging

## Conclusión

**Recomendación Final**: 

Mantener la arquitectura modular actual hasta que:
1. Tengas problemas de rendimiento reales
2. Necesites más de 1000 entidades simultáneas
3. Requieras paralelización masiva

**Si decides migrar a ECS:**
- Hazlo gradualmente
- Mantén compatibilidad temporal
- Valida cada paso
- Documenta todo el proceso

**Recordatorio**: ECS es una herramienta poderosa, pero no es necesaria para todos los proyectos. La arquitectura modular actual es perfectamente válida y funcional para la mayoría de casos de uso.

---

**Última actualización**: Diciembre 2024  
**Estado**: Plan de migración futura (no implementado)  
**Prioridad**: Baja (solo si es necesario para rendimiento)

