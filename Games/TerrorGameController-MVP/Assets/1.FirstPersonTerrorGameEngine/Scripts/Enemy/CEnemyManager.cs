using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement;
using System.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HorrorEngine
{
    public class CEnemyManager : MonoBehaviour
{
    public static CEnemyManager Instance { get; private set; }

    [SerializeField]
    private Transform spawnPoint;

    private Factory factory;
    private List<GameObject> enemyInstances = new List<GameObject>();

    #region Event Enemies
    public delegate void EnemyEvent(GameObject enemy);
    public event EnemyEvent OnEnemySpawned;
    public event EnemyEvent OnEnemyDestroyed;
    #endregion

    #region Asset Configuration (AUTOMÁTICO)
    [Header("Asset Configuration")]
    [SerializeField] private bool autoLoadEnemies = true;
    [SerializeField] private string enemyFolderPath = "Assets/1.FirstPersonTerrorGameEngine/Prefab/Enemy/";
    [SerializeField] private bool useAddressables = true;
    [SerializeField] private bool fallbackToResources = true;
    [SerializeField] private bool handleAddressableExceptions = true;
    
    [Header("Manual Configuration (Solo si autoLoadEnemies = false)")]
    [SerializeField] private List<string> enemyAddresses = new List<string>();
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    #endregion

    #region Testing & Debug Variables
    [Header("Testing & Debug")]
    [SerializeField] private bool enableDebugLogs = true;
    [SerializeField] private bool autoSpawnOnStart = false;
    [SerializeField] private KeyCode spawnTestKey = KeyCode.T;
    [SerializeField] private KeyCode randomSpawnKey = KeyCode.R;
    [SerializeField] private KeyCode clearAllKey = KeyCode.C;
    [SerializeField] private KeyCode debugInfoKey = KeyCode.I;
    [SerializeField] private KeyCode reloadEnemiesKey = KeyCode.L;
    
    // Variables de estado para testing
    private bool isTestMode = false;
    private int totalSpawnAttempts = 0;
    private int successfulSpawns = 0;
    private int failedSpawns = 0;
    private float lastSpawnTime = 0f;
    #endregion

    #region Asset Management (HÍBRIDO)
    private Dictionary<string, GameObject> assetCache = new Dictionary<string, GameObject>();
    private Dictionary<string, AsyncOperationHandle<GameObject>> loadingOperations = new Dictionary<string, AsyncOperationHandle<GameObject>>();
    private List<GameObject> availableEnemies = new List<GameObject>();
    private bool isInitialized = false;
    private bool isInitializing = false;
    #endregion

    #region Unity Lifecycle
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Configurar Exception Handler para Addressables (SOLUCIÓN OFICIAL)
        if (handleAddressableExceptions)
        {
            SetupAddressableExceptionHandler();
        }
        
        factory = new Factory();
        
        // Inicializar Addressables antes de cargar enemigos
        StartCoroutine(InitializeAddressablesAndEnemies());
        
        // Auto spawn para testing
        if (autoSpawnOnStart)
        {
            StartCoroutine(AutoSpawnTestRoutine());
        }
    }
    
    /// <summary>
    /// Inicializa Addressables y luego carga enemigos
    /// </summary>
    private IEnumerator InitializeAddressablesAndEnemies()
    {
        if (useAddressables)
        {
            LogDebug("🚀 Inicializando Addressables...");
            
            // Verificar si Addressables ya está inicializado
            if (Addressables.RuntimePath != null)
            {
                LogDebug("✅ Addressables ya está inicializado");
            }
            else
            {
                // Inicializar Addressables
                var initOperation = Addressables.InitializeAsync();
                yield return initOperation;
                
                // Verificar si el handle es válido antes de acceder a su Status
                if (initOperation.IsValid())
                {
                    if (initOperation.Status == AsyncOperationStatus.Succeeded)
                    {
                        LogDebug("✅ Addressables inicializado correctamente");
                    }
                    else
                    {
                        LogDebug($"❌ Error inicializando Addressables: {initOperation.Status}");
                    }
                    
                    // Liberar el handle de inicialización
                    Addressables.Release(initOperation);
                }
                else
                {
                    LogDebug("⚠️ Handle de inicialización de Addressables no válido");
                }
            }
        }
        
        // Cargar enemigos después de inicializar Addressables
        InitializeEnemies();
    }

    void Update()
    {
        // Limpiar instancias nulas
        for (int i = enemyInstances.Count - 1; i >= 0; i--)
        {
            if (enemyInstances[i] == null)
            {
                enemyInstances.RemoveAt(i);
            }
        }

        // Testing controls
        HandleTestInputs();
    }
    #endregion

    #region Addressable Exception Handler (SOLUCIÓN OFICIAL)
    /// <summary>
    /// Configura el Exception Handler personalizado para Addressables
    /// Basado en la documentación oficial de Unity
    /// </summary>
    private void SetupAddressableExceptionHandler()
    {
        try
        {
            ResourceManager.ExceptionHandler = CustomExceptionHandler;
            LogDebug("✅ Exception Handler configurado para Addressables");
        }
        catch (Exception e)
        {
            LogDebug($"❌ Error configurando Exception Handler: {e.Message}");
        }
    }

    /// <summary>
    /// Exception Handler personalizado para Addressables
    /// Se llama para cada escenario de error durante una operación
    /// Basado en la documentación oficial de Unity
    /// </summary>
    /// <param name="handle">Handle de la operación</param>
    /// <param name="exception">Excepción que ocurrió</param>
    private void CustomExceptionHandler(AsyncOperationHandle handle, Exception exception)
    {
        // Manejar InvalidKeyException de forma silenciosa (como recomienda Unity)
        if (exception.GetType() == typeof(InvalidKeyException))
        {
            LogDebug($"⚠️ InvalidKeyException manejada: {exception.Message}");
            // No hacer nada más, fallback automático a Resources
        }
        else
        {
            // Para otras excepciones, usar el logging oficial de Unity
            LogDebug($"❌ Error de Addressables: {exception.Message}");
            Addressables.LogException(handle, exception);
        }
    }
    #endregion

    #region Testing & Debug Methods (MODIFICADO)
    private void HandleTestInputs()
    {
        if (Input.GetKeyDown(spawnTestKey))
        {
            TestSpawnEnemy();
        }
        
        if (Input.GetKeyDown(randomSpawnKey))
        {
            TestSpawnRandomEnemy();
        }
        
        if (Input.GetKeyDown(clearAllKey))
        {
            TestClearAllEnemies();
        }
        
        if (Input.GetKeyDown(debugInfoKey))
        {
            TestShowDebugInfo();
        }
        
        if (Input.GetKeyDown(reloadEnemiesKey))
        {
            TestReloadEnemies();
        }
    }
    
    /// <summary>
    /// Test: Recargar enemigos
    /// </summary>
    public void TestReloadEnemies()
    {
        LogDebug("🔄 Recargando enemigos...");
        isInitialized = false;
        availableEnemies.Clear();
        assetCache.Clear();
        InitializeEnemies();
    }
    
    /// <summary>
    /// Test: Spawn de enemigo normal
    /// </summary>
    public void TestSpawnEnemy()
    {
        if (!isInitialized)
        {
            LogDebug("❌ Sistema no inicializado para spawn test");
            return;
        }
        
        totalSpawnAttempts++;
        lastSpawnTime = Time.time;
        
        LogDebug("🧪 Iniciando test de spawn...");
        StartCoroutine(TestSpawnCoroutine());
    }
    
    /// <summary>
    /// Test: Spawn de enemigo aleatorio
    /// </summary>
    public void TestSpawnRandomEnemy()
    {
        if (!isInitialized)
        {
            LogDebug("❌ Sistema no inicializado para spawn aleatorio test");
            return;
        }
        
        totalSpawnAttempts++;
        lastSpawnTime = Time.time;
        
        LogDebug("🎲 Iniciando test de spawn aleatorio...");
        StartCoroutine(TestRandomSpawnCoroutine());
    }
    
    /// <summary>
    /// Test: Limpiar todos los enemigos
    /// </summary>
    public void TestClearAllEnemies()
    {
        LogDebug("🧹 Limpiando todos los enemigos...");
        ClearAllEnemies();
        LogDebug($"✅ Enemigos limpiados. Total actual: {GetEnemyCount()}");
    }
    
    /// <summary>
    /// Test: Mostrar información de debug
    /// </summary>
    public void TestShowDebugInfo()
    {
        string info = GetDetailedDebugInfo();
        LogDebug(info);
    }
    
    /// <summary>
    /// Test: Verificar carga de recursos
    /// </summary>
    public void TestResourceLoading()
    {
        LogDebug("🔍 Verificando carga de recursos...");
        
        if (availableEnemies.Count == 0)
        {
            LogDebug("❌ No hay enemigos disponibles");
            return;
        }
        
        LogDebug($"📁 Total de enemigos disponibles: {availableEnemies.Count}");
        
        for (int i = 0; i < availableEnemies.Count; i++)
        {
            GameObject enemy = availableEnemies[i];
            string status = enemy != null ? "✅ Disponible" : "❌ Null";
            LogDebug($"  {i + 1}. {enemy?.name}: {status}");
        }
    }
    
    /// <summary>
    /// Test: Auto spawn para testing continuo
    /// </summary>
    private IEnumerator AutoSpawnTestRoutine()
    {
        LogDebug("🤖 Iniciando auto spawn test...");
        isTestMode = true;
        
        yield return new WaitForSeconds(2f); // Esperar inicialización
        
        while (isTestMode)
        {
            if (isInitialized)
            {
                TestSpawnRandomEnemy();
                yield return new WaitForSeconds(5f); // Spawn cada 5 segundos
            }
            else
            {
                yield return new WaitForSeconds(1f);
            }
        }
    }
    
    /// <summary>
    /// Corrutina para test de spawn
    /// </summary>
    private IEnumerator TestSpawnCoroutine()
    {
        var spawnTask = SpawnEnemyAsync();
        
        while (!spawnTask.IsCompleted)
        {
            yield return null;
        }
        
        GameObject enemy = spawnTask.Result;
        if (enemy != null)
        {
            successfulSpawns++;
            LogDebug($"✅ Test spawn exitoso: {enemy.name} (Total: {successfulSpawns}/{totalSpawnAttempts})");
        }
        else
        {
            failedSpawns++;
            LogDebug($"❌ Test spawn fallido (Total: {failedSpawns}/{totalSpawnAttempts})");
        }
    }
    
    /// <summary>
    /// Corrutina para test de spawn aleatorio
    /// </summary>
    private IEnumerator TestRandomSpawnCoroutine()
    {
        if (availableEnemies.Count == 0)
        {
            LogDebug("❌ No hay enemigos disponibles para spawn aleatorio");
            yield break;
        }
        
        // Seleccionar enemigo aleatorio
        int randomIndex = UnityEngine.Random.Range(0, availableEnemies.Count);
        GameObject randomEnemy = availableEnemies[randomIndex];
        
        var spawnTask = SpawnEnemyAsync(randomEnemy);
        
        while (!spawnTask.IsCompleted)
        {
            yield return null;
        }
        
        GameObject enemy = spawnTask.Result;
        if (enemy != null)
        {
            successfulSpawns++;
            LogDebug($"🎲 Test spawn aleatorio exitoso: {enemy.name} (Total: {successfulSpawns}/{totalSpawnAttempts})");
        }
        else
        {
            failedSpawns++;
            LogDebug($"❌ Test spawn aleatorio fallido (Total: {failedSpawns}/{totalSpawnAttempts})");
        }
    }
    
    /// <summary>
    /// Log con control de debug
    /// </summary>
    private void LogDebug(string message)
    {
        if (enableDebugLogs)
        {
            Debug.Log($"[CEnemyManager Test] {message}");
        }
    }
    
    /// <summary>
    /// Información detallada de debug
    /// </summary>
    public string GetDetailedDebugInfo()
    {
        string info = "=== CEnemyManager Test Info ===\n";
        info += $"🔄 Estado del Sistema:\n";
        info += $"  - Inicializado: {(isInitialized ? "✅" : "❌")}\n";
        info += $"  - Inicializando: {(isInitializing ? "⏳" : "✅")}\n";
        info += $"  - Modo Test: {(isTestMode ? "🤖" : "🎮")}\n";
        info += $"  - Carga Automática: {(autoLoadEnemies ? "✅" : "❌")}\n";
        info += $"  - Usar Addressables: {(useAddressables ? "✅" : "❌")}\n";
        info += $"  - Fallback a Resources: {(fallbackToResources ? "✅" : "❌")}\n";
        info += $"  - Exception Handler: {(handleAddressableExceptions ? "✅" : "❌")}\n";
        
        info += $"\n📊 Estadísticas de Spawn:\n";
        info += $"  - Intentos totales: {totalSpawnAttempts}\n";
        info += $"  - Spawns exitosos: {successfulSpawns}\n";
        info += $"  - Spawns fallidos: {failedSpawns}\n";
        info += $"  - Tasa de éxito: {(totalSpawnAttempts > 0 ? (successfulSpawns * 100f / totalSpawnAttempts).ToString("F1") : "0")}%\n";
        info += $"  - Último spawn: {(lastSpawnTime > 0 ? (Time.time - lastSpawnTime).ToString("F1") + "s atrás" : "Nunca")}\n";
        
        info += $"\n🎯 Enemigos Activos:\n";
        info += $"  - Total: {GetEnemyCount()}\n";
        
        info += $"\n💾 Gestión de Assets:\n";
        info += $"  - Enemigos disponibles: {availableEnemies.Count}\n";
        info += $"  - Assets en caché: {assetCache.Count}\n";
        info += $"  - Operaciones de carga: {loadingOperations.Count}\n";
        
        if (availableEnemies.Count > 0)
        {
            info += $"\n📁 Enemigos Disponibles:\n";
            for (int i = 0; i < availableEnemies.Count; i++)
            {
                GameObject enemy = availableEnemies[i];
                string status = enemy != null ? "✅" : "❌";
                info += $"  {i + 1}. {status} {enemy?.name}\n";
            }
        }
        else
        {
            info += $"  - Enemigos disponibles: ❌ Ninguno\n";
        }
        
        info += $"\n🎮 Controles de Test:\n";
        info += $"  - Spawn normal: {spawnTestKey}\n";
        info += $"  - Spawn aleatorio: {randomSpawnKey}\n";
        info += $"  - Limpiar todos: {clearAllKey}\n";
        info += $"  - Info debug: {debugInfoKey}\n";
        info += $"  - Recargar enemigos: {reloadEnemiesKey}\n";
        
        return info;
    }
    #endregion

    #region Asset Loading (AUTOMÁTICO)
    private async void InitializeEnemies()
    {
        if (isInitializing || isInitialized)
        {
            LogDebug("⚠️ Inicialización ya en progreso o completada");
            return;
        }

        isInitializing = true;

        try
        {
            LogDebug("🚀 Iniciando carga automática de enemigos...");

            // Cargar enemigos automáticamente
            if (autoLoadEnemies)
            {
                await LoadEnemiesAutomatically();
            }
            else
            {
                await LoadEnemiesManually();
            }

            // Validar configuración
            if (!ValidateConfiguration())
            {
                LogDebug("❌ Configuración inválida");
                return;
            }

            // Spawn inicial
            if (availableEnemies.Count > 0)
            {
                LogDebug($" Spawn inicial con: {availableEnemies[0].name}");
                var spawnTask = SpawnEnemyAsync(availableEnemies[0]);
                await spawnTask;
            }

            isInitialized = true;
            LogDebug($"✅ Inicialización completada. {availableEnemies.Count} enemigos disponibles");
            
            // Mostrar resumen
            TestResourceLoading();
        }
        catch (Exception e)
        {
            LogDebug($"❌ Error durante la inicialización: {e.Message}");
        }
        finally
        {
            isInitializing = false;
        }
    }

    private async Task LoadEnemiesAutomatically()
    {
        LogDebug("🔍 Cargando enemigos automáticamente...");
        
        // Limpiar lista anterior
        availableEnemies.Clear();
        
        // Buscar enemigos en la carpeta
        List<GameObject> foundEnemies = FindEnemiesInFolder();
        
        if (foundEnemies.Count == 0)
        {
            LogDebug("❌ No se encontraron enemigos en la carpeta");
            return;
        }

        LogDebug($" Encontrados {foundEnemies.Count} enemigos en la carpeta");

        // Cargar cada enemigo
        foreach (GameObject enemyPrefab in foundEnemies)
        {
            if (enemyPrefab != null)
            {
                string enemyKey = enemyPrefab.name;
                
                if (useAddressables)
                {
                    // Intentar cargar con Addressables
                    GameObject loadedEnemy = await LoadEnemyWithAddressables(enemyKey, enemyPrefab);
                    if (loadedEnemy != null)
                    {
                        availableEnemies.Add(loadedEnemy);
                        LogDebug($"✅ Cargado con Addressables: {enemyKey}");
                    }
                    else if (fallbackToResources)
                    {
                        // Fallback a Resources
                        GameObject resourceEnemy = LoadEnemyWithResources(enemyPrefab);
                        if (resourceEnemy != null)
                        {
                            availableEnemies.Add(resourceEnemy);
                            LogDebug($"📦 Cargado con Resources (fallback): {enemyKey}");
                        }
                    }
                }
                else
                {
                    // Cargar directamente con Resources
                    GameObject resourceEnemy = LoadEnemyWithResources(enemyPrefab);
                    if (resourceEnemy != null)
                    {
                        availableEnemies.Add(resourceEnemy);
                        LogDebug($"📦 Cargado con Resources: {enemyKey}");
                    }
                }
            }
        }

        LogDebug($"✅ Carga automática completada: {availableEnemies.Count} enemigos disponibles");
    }

    private List<GameObject> FindEnemiesInFolder()
    {
        List<GameObject> enemies = new List<GameObject>();

#if UNITY_EDITOR
        try
        {
            // Buscar todos los prefabs en la carpeta
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { enemyFolderPath });
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.Contains("Enemy_") && path.EndsWith(".prefab"))
                {
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab != null)
                    {
                        enemies.Add(prefab);
                        LogDebug($"📁 Encontrado: {prefab.name} en {path}");
                    }
                }
            }
        }
        catch (Exception e)
        {
            LogDebug($"❌ Error buscando enemigos: {e.Message}");
        }
#else
        // En build, usar Resources
        try
        {
            string resourcesPath = "Prefab/Enemy/";
            GameObject[] prefabs = Resources.LoadAll<GameObject>(resourcesPath);
            
            foreach (GameObject prefab in prefabs)
            {
                if (prefab.name.StartsWith("Enemy_"))
                {
                    enemies.Add(prefab);
                    LogDebug($"📦 Encontrado en Resources: {prefab.name}");
                }
            }
        }
        catch (Exception e)
        {
            LogDebug($"❌ Error cargando desde Resources: {e.Message}");
        }
#endif

        return enemies;
    }

    private async Task<GameObject> LoadEnemyWithAddressables(string enemyKey, GameObject fallbackPrefab)
    {
        AsyncOperationHandle<GameObject> handle = default;
        
        try
        {
            LogDebug($"🔍 Intentando cargar con Addressables: {enemyKey}");
            
            // Intentar cargar con Addressables
            handle = Addressables.LoadAssetAsync<GameObject>(enemyKey);
            loadingOperations[enemyKey] = handle;
            
            // Esperar a que termine (con Exception Handler activo)
            GameObject prefab = await handle.Task;
            
            // Verificar si el handle sigue siendo válido
            if (handle.IsValid())
            {
                if (prefab != null)
                {
                    assetCache[enemyKey] = prefab;
                    LogDebug($"✅ Addressables exitoso: {enemyKey}");
                    return prefab;
                }
                else
                {
                    LogDebug($"⚠️ Addressables retornó null para: {enemyKey}");
                    return null;
                }
            }
            else
            {
                LogDebug($"⚠️ Handle inválido para: {enemyKey}");
                return null;
            }
        }
        catch (InvalidKeyException)
        {
            // Esta excepción será manejada por el Exception Handler
            LogDebug($"⚠️ InvalidKeyException para: {enemyKey} - Usando fallback");
            return null;
        }
        catch (Exception e)
        {
            LogDebug($"❌ Error inesperado con Addressables para {enemyKey}: {e.Message}");
            return null;
        }
        finally
        {
            // Limpiar el handle del diccionario
            loadingOperations.Remove(enemyKey);
            
            // Liberar el handle si es válido
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
    }

    private GameObject LoadEnemyWithResources(GameObject prefab)
    {
        try
        {
            // Usar el prefab directamente
            assetCache[prefab.name] = prefab;
            LogDebug($"📦 Cargado con Resources: {prefab.name}");
            return prefab;
        }
        catch (Exception e)
        {
            LogDebug($"❌ Error cargando con Resources: {e.Message}");
            return null;
        }
    }

    private async Task LoadEnemiesManually()
    {
        LogDebug("🔧 Cargando enemigos manualmente...");
        
        // Usar configuración manual
        if (enemyAddresses.Count > 0)
        {
            foreach (string address in enemyAddresses)
            {
                await LoadEnemyAssetAsync(address);
            }
        }
        
        if (enemyPrefabs.Count > 0)
        {
            foreach (GameObject prefab in enemyPrefabs)
            {
                if (prefab != null)
                {
                    availableEnemies.Add(prefab);
                    assetCache[prefab.name] = prefab;
                }
            }
        }
    }

    private bool ValidateConfiguration()
    {
        // Validar spawnPoint
        if (spawnPoint == null)
        {
            LogDebug("❌ SpawnPoint no está asignado en el inspector");
            return false;
        }

        // Validar que haya enemigos disponibles
        if (availableEnemies.Count == 0)
        {
            LogDebug("❌ No hay enemigos disponibles");
            return false;
        }

        // Validar factory
        if (factory == null)
        {
            LogDebug("❌ Factory no está inicializado");
            return false;
        }

        return true;
    }

    private async Task<GameObject> LoadEnemyAssetAsync(string assetAddress)
    {
        if (string.IsNullOrEmpty(assetAddress))
        {
            LogDebug("❌ Dirección de asset vacía");
            return null;
        }

        // Verificar caché
        if (assetCache.ContainsKey(assetAddress))
        {
            LogDebug($" Asset ya en caché: {assetAddress}");
            return assetCache[assetAddress];
        }

        // Verificar si ya se está cargando
        if (loadingOperations.ContainsKey(assetAddress))
        {
            LogDebug($"⏳ Asset ya se está cargando: {assetAddress}");
            await loadingOperations[assetAddress].Task;
            return assetCache.ContainsKey(assetAddress) ? assetCache[assetAddress] : null;
        }

        try
        {
            LogDebug($" Iniciando carga de asset: {assetAddress}");
            
            // Cargar con Addressables
            var handle = Addressables.LoadAssetAsync<GameObject>(assetAddress);
            loadingOperations[assetAddress] = handle;
            
            // Esperar a que termine la carga
            GameObject prefab = await handle.Task;
            
            if (prefab != null)
            {
                assetCache[assetAddress] = prefab;
                availableEnemies.Add(prefab);
                LogDebug($"✅ Asset cargado exitosamente: {assetAddress}");
            }
            else
            {
                LogDebug($"❌ Error al cargar asset: {assetAddress} - El asset es null");
            }

            return prefab;
        }
        catch (Exception e)
        {
            LogDebug($"💥 Excepción al cargar {assetAddress}: {e.Message}");
            return null;
        }
        finally
        {
            loadingOperations.Remove(assetAddress);
        }
    }

    private bool IsAssetLoaded(string assetKey)
    {
        return assetCache.ContainsKey(assetKey) && assetCache[assetKey] != null;
    }

    private bool IsAssetLoading(string assetKey)
    {
        return loadingOperations.ContainsKey(assetKey);
    }
    
    /// <summary>
    /// Obtiene el enemigo por defecto
    /// </summary>
    private GameObject GetDefaultEnemy()
    {
        if (availableEnemies.Count > 0)
        {
            return availableEnemies[0];
        }
        return null;
    }
    #endregion

    #region Enemy Spawning (MODIFICADO)
    private async Task<GameObject> SpawnEnemyAsync(GameObject enemyPrefab = null)
    {
        // Validar que el sistema esté inicializado
        if (!isInitialized && !isInitializing)
        {
            LogDebug("⚠️ Sistema de enemigos no inicializado. Iniciando...");
            await Task.Delay(100);
        }

        // Validar configuración
        if (!ValidateConfiguration())
        {
            return null;
        }

        GameObject prefabToUse = enemyPrefab ?? GetDefaultEnemy();
        
        if (prefabToUse == null)
        {
            LogDebug("❌ No hay enemigo disponible para spawn");
            return null;
        }

        LogDebug($" Spawneando enemigo: {prefabToUse.name}");

        // Factory crea la instancia
        GameObject enemy = factory.GetOrCreateGameObject(prefabToUse, spawnPoint.position, spawnPoint.rotation);
        
        if (enemy != null)
        {
            // CEnemyManager gestiona la lógica de juego
            enemyInstances.Add(enemy);
            OnEnemySpawned?.Invoke(enemy);
            LogDebug($"✅ Enemigo spawnado exitosamente: {enemy.name}");
            return enemy;
        }
        else
        {
            LogDebug($"❌ Factory no pudo crear la instancia del enemigo");
            return null;
        }
    }
    #endregion



    #region Enemy Management (SIN CAMBIOS)
    public void RemoveEnemy(GameObject enemy)
    {
        if (enemy == null)
        {
            Debug.LogWarning("Intento de remover enemigo null");
            return;
        }

        if (enemyInstances.Contains(enemy))
        {
            enemyInstances.Remove(enemy);
            factory.ReturnToPool(enemy);
            OnEnemyDestroyed?.Invoke(enemy);
            Debug.Log($"Enemigo removido: {enemy.name}");
        }
        else
        {
            Debug.LogWarning($"Enemigo no encontrado en la lista: {enemy.name}");
        }
    }

    public void ClearAllEnemies()
    {
        Debug.Log($"Limpiando {enemyInstances.Count} enemigos");
        
        for (int i = enemyInstances.Count - 1; i >= 0; i--)
        {
            if (enemyInstances[i] != null)
            {
                factory.ReturnToPool(enemyInstances[i]);
            }
        }
        enemyInstances.Clear();
    }

    public int GetEnemyCount()
    {
        return enemyInstances.Count;
    }


    
    /// <summary>
    /// Obtiene información básica de debug sobre el estado del sistema
    /// </summary>
    /// <returns>String con información de debug</returns>
    public string GetDebugInfo()
    {
        return GetDetailedDebugInfo();
    }
    
    /// <summary>
    /// Spawna un enemigo aleatorio
    /// </summary>
    /// <returns>Enemigo spawnado o null si falla</returns>
    public async Task<GameObject> SpawnRandomEnemyAsync()
    {
        if (availableEnemies.Count == 0)
        {
            LogDebug("❌ No hay enemigos disponibles para spawn aleatorio");
            return null;
        }
        
        // Obtener enemigo aleatorio
        int randomIndex = UnityEngine.Random.Range(0, availableEnemies.Count);
        GameObject randomEnemy = availableEnemies[randomIndex];
        
        if (randomEnemy == null)
        {
            LogDebug("❌ Enemigo aleatorio es null");
            return null;
        }
        
        return await SpawnEnemyAsync(randomEnemy);
    }
    #endregion

    #region Cleanup (NUEVO)
    private void OnDestroy()
    {
        LogDebug("🧹 Limpiando recursos de CEnemyManager...");
        
        // Detener modo test
        isTestMode = false;
        
        // Liberar operaciones de Addressables
        foreach (var operation in loadingOperations.Values)
        {
            if (operation.IsValid())
            {
                Addressables.Release(operation);
            }
        }
        loadingOperations.Clear();
        
        // Liberar assets del caché
        assetCache.Clear();
        
        LogDebug("✅ Recursos de CEnemyManager liberados");
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Debug.Log("Aplicación pausada - CEnemyManager en pausa");
        }
        else
        {
            Debug.Log("Aplicación resumida - CEnemyManager activo");
        }
    }
    #endregion
    }
}

