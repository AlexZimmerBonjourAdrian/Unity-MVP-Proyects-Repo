# Sistema Factory

## Descripción General

El sistema Factory implementa un patrón de **Object Pooling** que optimiza la creación y reutilización de GameObjects, mejorando significativamente el rendimiento al evitar la creación y destrucción constante de objetos.

## Características Principales

- **Object Pooling** - Reutilización eficiente de objetos
- **Thread-safe** - Operaciones seguras en entornos multihilo
- **Gestión automática** - Activación/desactivación automática
- **Flexibilidad** - Soporte para diferentes tipos de prefabs
- **Rendimiento optimizado** - Reduce fragmentación de memoria

## Arquitectura del Sistema

### Diagrama de Flujo

```
GetOrCreateGameObject()
    ↓
¿Existe pool para este prefab?
    ↓
¿Hay objetos disponibles en el pool?
    ↓
SÍ → Reutilizar objeto existente
NO → Crear nuevo objeto
    ↓
Configurar posición y rotación
    ↓
Activar objeto
    ↓
Retornar objeto
```

### Componentes Principales

#### 1. ObjectPools
```csharp
private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();
```
- Almacena colas de objetos por tipo de prefab
- Clave: nombre del prefab
- Valor: cola de objetos disponibles

#### 2. PoolLock
```csharp
private readonly object poolLock = new object();
```
- Sincronización para operaciones thread-safe
- Previene condiciones de carrera
- Garantiza consistencia de datos

## Implementación Detallada

### Clase Factory

```csharp
public class Factory
{
    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();
    private readonly object poolLock = new object();
}
```

### Método Principal: GetOrCreateGameObject

```csharp
public GameObject GetOrCreateGameObject(GameObject prefab = null, Vector3? position = null, Quaternion? rotation = null)
{
    // Validación de entrada
    if (prefab == null)
    {
        Debug.LogError("Prefab is required to create or retrieve a GameObject.");
        return null;
    }

    string key = prefab.name;
    Vector3 spawnPosition = position ?? Vector3.zero;
    Quaternion spawnRotation = rotation ?? Quaternion.identity;

    lock (poolLock)
    {
        // Verificar si existe un pool y hay objetos disponibles
        if (objectPools.ContainsKey(key) && objectPools[key].Count > 0)
        {
            // Reutilizar objeto del pool
            GameObject pooledObject = objectPools[key].Dequeue();
            pooledObject.transform.position = spawnPosition;
            pooledObject.transform.rotation = spawnRotation;
            pooledObject.SetActive(true);
            return pooledObject;
        }

        // Crear nuevo objeto si no hay disponibles
        GameObject newObject = Object.Instantiate(prefab, spawnPosition, spawnRotation);
        newObject.name = key; // Mantener consistencia con la clave
        return newObject;
    }
}
```

### Retorno al Pool: ReturnToPool

```csharp
public void ReturnToPool(GameObject gameObject)
{
    if (gameObject == null)
    {
        Debug.LogError("Cannot return a null GameObject to the pool.");
        return;
    }

    string key = gameObject.name;

    lock (poolLock)
    {
        // Crear pool si no existe
        if (!objectPools.ContainsKey(key))
        {
            objectPools[key] = new Queue<GameObject>();
        }

        // Desactivar y agregar al pool
        gameObject.SetActive(false);
        objectPools[key].Enqueue(gameObject);
    }
}
```

### Limpieza de Pools

#### Limpiar Pool Específico
```csharp
public void ClearPool(string key)
{
    lock (poolLock)
    {
        if (objectPools.ContainsKey(key))
        {
            while (objectPools[key].Count > 0)
            {
                GameObject pooledObject = objectPools[key].Dequeue();
                Object.Destroy(pooledObject);
            }
            objectPools.Remove(key);
        }
    }
}
```

#### Limpiar Todos los Pools
```csharp
public void ClearAllPools()
{
    lock (poolLock)
    {
        foreach (var pool in objectPools)
        {
            while (pool.Value.Count > 0)
            {
                GameObject pooledObject = pool.Value.Dequeue();
                Object.Destroy(pooledObject);
            }
        }
        objectPools.Clear();
    }
}
```

## Integración con CEnemyManager

### Uso Típico

```csharp
// En CEnemyManager
private Factory factory;

void Start()
{
    factory = new Factory();
}

private async Task<GameObject> SpawnEnemyAsync(string assetAddress = null)
{
    // 1. Cargar prefab con Addressables
    GameObject prefab = await LoadEnemyAssetAsync(assetAddress);
    
    // 2. Factory crea la instancia (con pooling)
    GameObject enemy = factory.GetOrCreateGameObject(prefab, spawnPoint.position, spawnPoint.rotation);
    
    return enemy;
}

public void RemoveEnemy(GameObject enemy)
{
    // Retornar al pool en lugar de destruir
    factory.ReturnToPool(enemy);
}
```

## Ventajas del Sistema

### Rendimiento
- **Reducción de GC** - Menos presión en el garbage collector
- **Menos fragmentación** - Memoria más eficiente
- **Inicialización rápida** - Objetos ya configurados

### Escalabilidad
- **Manejo de muchos objetos** - Ideal para enemigos, proyectiles, efectos
- **Configuración flexible** - Diferentes pools para diferentes tipos
- **Thread-safe** - Seguro para operaciones concurrentes

### Mantenibilidad
- **Código limpio** - Separación clara de responsabilidades
- **Fácil debugging** - Logs informativos
- **API simple** - Fácil de usar e integrar

## Casos de Uso

### 1. Spawn de Enemigos
```csharp
// Crear enemigo
GameObject enemy = factory.GetOrCreateGameObject(enemyPrefab, spawnPosition, spawnRotation);

// Cuando muere el enemigo
factory.ReturnToPool(enemy);
```

### 2. Sistema de Proyectiles
```csharp
// Disparar proyectil
GameObject projectile = factory.GetOrCreateGameObject(projectilePrefab, gunPosition, gunRotation);

// Cuando el proyectil impacta
factory.ReturnToPool(projectile);
```

### 3. Efectos Visuales
```csharp
// Crear efecto de explosión
GameObject explosion = factory.GetOrCreateGameObject(explosionPrefab, hitPosition, Quaternion.identity);

// Cuando termina la animación
factory.ReturnToPool(explosion);
```

## Configuración y Optimización

### Tamaño de Pool
Para optimizar, considera pre-poblar los pools:

```csharp
public void PrePopulatePool(string key, GameObject prefab, int count)
{
    lock (poolLock)
    {
        if (!objectPools.ContainsKey(key))
        {
            objectPools[key] = new Queue<GameObject>();
        }

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Object.Instantiate(prefab);
            obj.name = key;
            obj.SetActive(false);
            objectPools[key].Enqueue(obj);
        }
    }
}
```

### Monitoreo de Pools
```csharp
public int GetPoolSize(string key)
{
    lock (poolLock)
    {
        return objectPools.ContainsKey(key) ? objectPools[key].Count : 0;
    }
}

public Dictionary<string, int> GetAllPoolSizes()
{
    lock (poolLock)
    {
        var sizes = new Dictionary<string, int>();
        foreach (var pool in objectPools)
        {
            sizes[pool.Key] = pool.Value.Count;
        }
        return sizes;
    }
}
```

## Mejores Prácticas

### 1. Nomenclatura
- Usar nombres consistentes para los prefabs
- El nombre del prefab debe coincidir con la clave del pool
- Documentar las convenciones de naming

### 2. Gestión de Memoria
- Limpiar pools no utilizados
- Monitorear el tamaño de los pools
- Evitar pools excesivamente grandes

### 3. Integración
- Usar junto con Addressables para carga eficiente
- Implementar en sistemas que crean muchos objetos
- Considerar para efectos temporales

### 4. Testing
- Probar con diferentes cargas de objetos
- Verificar comportamiento en situaciones extremas
- Validar liberación correcta de memoria

## Troubleshooting

### Problema: Objetos no se reutilizan
- Verificar que el nombre del prefab sea consistente
- Confirmar que se llame a `ReturnToPool`
- Revisar que no se destruyan objetos manualmente

### Problema: Memory Leaks
- Asegurar que se llame a `ClearAllPools` al finalizar
- Verificar que no haya referencias circulares
- Monitorear el tamaño de los pools

### Problema: Thread Safety
- Todas las operaciones deben usar el lock
- Evitar modificaciones directas a los pools
- Usar métodos thread-safe para consultas

## Extensiones Posibles

### 1. Pool con Configuración
```csharp
public class PoolConfig
{
    public int initialSize;
    public int maxSize;
    public bool expandable;
}
```

### 2. Pool con Callbacks
```csharp
public event Action<GameObject> OnObjectSpawned;
public event Action<GameObject> OnObjectReturned;
```

### 3. Pool con Categorías
```csharp
public enum PoolCategory
{
    Enemies,
    Projectiles,
    Effects,
    UI
}
```

## Referencias

- [Object Pooling Pattern](https://en.wikipedia.org/wiki/Object_pool_pattern)
- [Unity Performance Best Practices](https://docs.unity3d.com/Manual/PerformanceOptimization.html)
- [Memory Management in Unity](https://docs.unity3d.com/Manual/PerformanceOptimization.html) 