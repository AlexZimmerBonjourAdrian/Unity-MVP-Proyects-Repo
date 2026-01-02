# Sistema de Addressables

## Descripción General

El sistema de Addressables en este proyecto proporciona una gestión eficiente y asíncrona de assets (prefabs, texturas, audio, etc.) con las siguientes características:

- **Carga asíncrona** - No bloquea el hilo principal
- **Sistema de caché** - Evita cargas duplicadas
- **Gestión de memoria** - Liberación automática de recursos
- **Manejo de dependencias** - Carga automática de assets relacionados
- **Actualizaciones en tiempo de ejecución** - Posibilidad de actualizar contenido sin parches

## Arquitectura del Sistema

### Separación de Responsabilidades

```
Addressables ←→ CEnemyManager ←→ Factory ←→ Instancias en Escena
     ↓              ↓              ↓              ↓
  Carga de      Coordinación    Object Pooling   Objetos
  Assets        y Lógica        y Spawning       Activos
```

### Componentes Principales

#### 1. AssetCache
```csharp
private Dictionary<string, GameObject> assetCache = new Dictionary<string, GameObject>();
```
- Almacena assets cargados para reutilización
- Evita cargas repetidas del mismo asset
- Mejora significativamente el rendimiento

#### 2. LoadingOperations
```csharp
private Dictionary<string, AsyncOperationHandle<GameObject>> loadingOperations = new Dictionary<string, AsyncOperationHandle<GameObject>>();
```
- Gestiona operaciones de carga asíncronas
- Previene cargas duplicadas simultáneas
- Permite cancelación y liberación de recursos

## Implementación en CEnemyManager

### Inicialización del Sistema

```csharp
private async void InitializeEnemies()
{
    // Validar configuración
    if (!ValidateConfiguration()) return;
    
    // Precargar todos los assets
    foreach (string assetAddress in resourcesPathDirection.resourcePaths)
    {
        await LoadEnemyAssetAsync(assetAddress);
    }
    
    // Spawn inicial
    await SpawnEnemyAsync(resourcesPathDirection.resourcePaths[0]);
}
```

### Carga de Assets

```csharp
private async Task<GameObject> LoadEnemyAssetAsync(string assetAddress)
{
    // Verificar caché
    if (assetCache.ContainsKey(assetAddress))
        return assetCache[assetAddress];
    
    // Verificar carga en progreso
    if (loadingOperations.ContainsKey(assetAddress))
    {
        await loadingOperations[assetAddress].Task;
        return assetCache[assetAddress];
    }
    
    // Cargar con Addressables
    var handle = Addressables.LoadAssetAsync<GameObject>(assetAddress);
    loadingOperations[assetAddress] = handle;
    
    GameObject prefab = await handle.Task;
    assetCache[assetAddress] = prefab;
    
    return prefab;
}
```

### Spawn de Enemigos

```csharp
private async Task<GameObject> SpawnEnemyAsync(string assetAddress = null)
{
    // 1. Addressables carga el asset
    GameObject prefab = await LoadEnemyAssetAsync(assetAddress);
    
    // 2. Factory crea la instancia
    GameObject enemy = factory.GetOrCreateGameObject(prefab, spawnPoint.position, spawnPoint.rotation);
    
    // 3. CEnemyManager gestiona la lógica
    enemyInstances.Add(enemy);
    OnEnemySpawned?.Invoke(enemy);
    
    return enemy;
}
```

## Configuración Requerida

### 1. Package Manager
Asegúrate de tener instalado el paquete Addressables:
```
Window → Package Manager → Unity Registry → Addressables
```

### 2. Configuración de Grupos
1. Abrir `Window → Asset Management → Addressables → Groups`
2. Crear grupos para diferentes tipos de assets
3. Configurar estrategias de carga

### 3. Marcado de Assets
1. Seleccionar el prefab del enemigo
2. En Inspector: `Addressable` → `✓`
3. Asignar una dirección única (ej: "SpiderEnemy")

### 4. Build de Addressables
1. `Window → Asset Management → Addressables → Build → New Build → Default Build Script`
2. Esto genera los archivos necesarios para el runtime

## Ventajas del Sistema

### Rendimiento
- **Carga asíncrona** - No bloquea el juego
- **Caché inteligente** - Evita cargas repetidas
- **Gestión de memoria** - Liberación automática

### Escalabilidad
- **Fácil adición** de nuevos enemigos
- **Configuración centralizada** en ResourcesPathDirection
- **Sistema modular** y reutilizable

### Mantenibilidad
- **Separación clara** de responsabilidades
- **Logging detallado** para debugging
- **Validaciones robustas** en cada paso

## Casos de Uso

### 1. Carga Inicial
```csharp
// Se ejecuta automáticamente en Start()
await InitializeEnemies();
```

### 2. Spawn Manual
```csharp
// Con la tecla T
if (Input.GetKeyDown(KeyCode.T))
{
    await SpawnEnemyAsync();
}
```

### 3. Oleadas de Enemigos
```csharp
// Sistema automático de oleadas
public void StartWaves()
{
    StartCoroutine(SpawnWaveRoutine());
}
```

## Troubleshooting

### Error: "Asset not found"
- Verificar que el asset esté marcado como Addressable
- Confirmar que la dirección sea correcta
- Revisar que se haya hecho el build de Addressables

### Error: "Loading operation failed"
- Verificar conexión a internet (si usa CDN)
- Revisar logs de Addressables en Console
- Confirmar que el asset existe en el build

### Performance Issues
- Revisar tamaño de los assets
- Considerar compresión de texturas
- Optimizar configuración de grupos

## Mejores Prácticas

### 1. Nomenclatura
- Usar nombres descriptivos para las direcciones
- Mantener consistencia en el naming
- Documentar las direcciones utilizadas

### 2. Organización
- Agrupar assets relacionados
- Usar prefijos para categorías
- Mantener estructura lógica

### 3. Optimización
- Precargar assets críticos
- Liberar assets no utilizados
- Monitorear uso de memoria

### 4. Testing
- Probar en diferentes dispositivos
- Verificar comportamiento offline
- Validar tiempos de carga

## Referencias

- [Unity Addressables Documentation](https://docs.unity3d.com/Packages/com.unity.addressables@latest)
- [Addressables Best Practices](https://docs.unity3d.com/Packages/com.unity.addressables@latest/manual/AddressableAssetsBestPractices.html)
- [Memory Management](https://docs.unity3d.com/Packages/com.unity.addressables@latest/manual/MemoryManagement.html)