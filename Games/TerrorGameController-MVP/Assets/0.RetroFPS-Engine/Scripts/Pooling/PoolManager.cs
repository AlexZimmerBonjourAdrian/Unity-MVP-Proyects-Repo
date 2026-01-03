using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace RetroFPS
{
    /// <summary>
    /// Pool Manager - Singleton centralizado para gestión de todos los object pools.
    /// Facilita la creación, acceso y gestión de pools de objetos en todo el juego.
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        // Diccionario de pools por nombre
        private readonly Dictionary<string, object> pools = new Dictionary<string, object>();

        // Configuración por defecto
        [Header("Default Pool Settings")]
        [SerializeField] private int defaultInitialSize = 10;
        [SerializeField] private int defaultMaxSize = 100;
        [SerializeField] private bool defaultAutoExpand = true;

        // Transform padre para todos los pools
        private Transform poolParent;

        #region Singleton Lifecycle

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Crear transform padre para pools
            poolParent = new GameObject("PoolManager_Parent").transform;
            poolParent.SetParent(transform);

            LogDebug("PoolManager initialized");
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                ClearAllPools();
                Instance = null;
            }
        }

        #endregion

        #region Pool Creation

        /// <summary>
        /// Crea un pool de objetos con configuración personalizada
        /// </summary>
        /// <typeparam name="T">Tipo del componente</typeparam>
        /// <param name="poolName">Nombre único del pool</param>
        /// <param name="prefab">Prefab del objeto</param>
        /// <param name="initialSize">Tamaño inicial</param>
        /// <param name="maxSize">Tamaño máximo (0 = ilimitado)</param>
        /// <param name="autoExpand">Si puede expandirse automáticamente</param>
        /// <returns>True si se creó exitosamente</returns>
        public bool CreatePool<T>(string poolName, T prefab, int initialSize = -1, int maxSize = -1, bool autoExpand = true)
            where T : Component
        {
            if (string.IsNullOrEmpty(poolName))
            {
                LogDebug("Cannot create pool: pool name is null or empty");
                return false;
            }

            if (pools.ContainsKey(poolName))
            {
                LogDebug($"Pool '{poolName}' already exists");
                return false;
            }

            if (prefab == null)
            {
                LogDebug($"Cannot create pool '{poolName}': prefab is null");
                return false;
            }

            // Usar valores por defecto si no se especifican
            if (initialSize < 0) initialSize = defaultInitialSize;
            if (maxSize < 0) maxSize = defaultMaxSize;

            // Crear transform específico para este pool
            Transform poolTransform = new GameObject($"Pool_{poolName}").transform;
            poolTransform.SetParent(poolParent);

            // Crear el pool
            var pool = new ObjectPool<T>(prefab, initialSize, poolTransform, maxSize, autoExpand);
            pools[poolName] = pool;

            LogDebug($"Created pool '{poolName}' with {initialSize} initial objects");
            return true;
        }

        /// <summary>
        /// Crea un pool usando Addressables
        /// </summary>
        /// <typeparam name="T">Tipo del componente</typeparam>
        /// <param name="poolName">Nombre único del pool</param>
        /// <param name="address">Dirección del prefab en Addressables</param>
        /// <param name="initialSize">Tamaño inicial</param>
        /// <param name="maxSize">Tamaño máximo</param>
        /// <param name="autoExpand">Si puede expandirse automáticamente</param>
        public async void CreatePoolFromAddressables<T>(
            string poolName,
            string address,
            int initialSize = -1,
            int maxSize = -1,
            bool autoExpand = true) where T : Component
        {
            if (string.IsNullOrEmpty(address))
            {
                LogDebug($"Cannot create pool '{poolName}': address is null or empty");
                return;
            }

            try
            {
                // Cargar el prefab usando Addressables
                var handle = Addressables.LoadAssetAsync<GameObject>(address);
                await handle.Task;

                if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    T prefab = handle.Result.GetComponent<T>();
                    if (prefab != null)
                    {
                        CreatePool(poolName, prefab, initialSize, maxSize, autoExpand);

                        // Liberar el handle (el pool mantiene su propia instancia)
                        Addressables.Release(handle);
                    }
                    else
                    {
                        LogDebug($"Failed to get component {typeof(T).Name} from addressable '{address}'");
                    }
                }
                else
                {
                    LogDebug($"Failed to load addressable '{address}': {handle.Status}");
                }
            }
            catch (System.Exception ex)
            {
                LogDebug($"Exception loading addressable '{address}': {ex.Message}");
            }
        }

        #endregion

        #region Pool Access

        /// <summary>
        /// Obtiene un objeto de un pool específico
        /// </summary>
        /// <typeparam name="T">Tipo del componente</typeparam>
        /// <param name="poolName">Nombre del pool</param>
        /// <returns>Objeto del pool o null si no existe el pool o no hay objetos disponibles</returns>
        public T Get<T>(string poolName) where T : Component
        {
            if (string.IsNullOrEmpty(poolName))
            {
                LogDebug("Cannot get from pool: pool name is null or empty");
                return null;
            }

            if (!pools.TryGetValue(poolName, out object poolObj))
            {
                LogDebug($"Pool '{poolName}' does not exist");
                return null;
            }

            if (poolObj is ObjectPool<T> pool)
            {
                T obj = pool.Get();
                if (obj == null)
                {
                    LogDebug($"No available objects in pool '{poolName}'");
                }
                return obj;
            }
            else
            {
                LogDebug($"Pool '{poolName}' is not of type {typeof(T).Name}");
                return null;
            }
        }

        /// <summary>
        /// Devuelve un objeto a su pool correspondiente
        /// </summary>
        /// <typeparam name="T">Tipo del componente</typeparam>
        /// <param name="poolName">Nombre del pool</param>
        /// <param name="obj">Objeto a devolver</param>
        public void Return<T>(string poolName, T obj) where T : Component
        {
            if (string.IsNullOrEmpty(poolName))
            {
                LogDebug("Cannot return to pool: pool name is null or empty");
                return;
            }

            if (obj == null)
            {
                LogDebug($"Cannot return null object to pool '{poolName}'");
                return;
            }

            if (!pools.TryGetValue(poolName, out object poolObj))
            {
                LogDebug($"Pool '{poolName}' does not exist");
                return;
            }

            if (poolObj is ObjectPool<T> pool)
            {
                pool.Return(obj);
            }
            else
            {
                LogDebug($"Pool '{poolName}' is not of type {typeof(T).Name}");
            }
        }

        /// <summary>
        /// Obtiene un objeto y lo configura para devolverlo automáticamente al pool
        /// </summary>
        /// <typeparam name="T">Tipo del componente</typeparam>
        /// <param name="poolName">Nombre del pool</param>
        /// <param name="autoReturnOnDisable">Si debe devolverse automáticamente al desactivarse</param>
        /// <returns>Objeto del pool</returns>
        public T GetWithAutoReturn<T>(string poolName, bool autoReturnOnDisable = true) where T : Component
        {
            T obj = Get<T>(poolName);
            if (obj != null && autoReturnOnDisable)
            {
                if (pools.TryGetValue(poolName, out object poolObj) && poolObj is ObjectPool<T> pool)
                {
                    obj.ReturnToPoolOnDisable(pool);
                }
            }
            return obj;
        }

        /// <summary>
        /// Devuelve un objeto al pool después de un delay
        /// </summary>
        /// <typeparam name="T">Tipo del componente</typeparam>
        /// <param name="poolName">Nombre del pool</param>
        /// <param name="obj">Objeto a devolver</param>
        /// <param name="delay">Tiempo de delay en segundos</param>
        public void ReturnWithDelay<T>(string poolName, T obj, float delay) where T : Component
        {
            if (pools.TryGetValue(poolName, out object poolObj) && poolObj is ObjectPool<T> pool)
            {
                obj.ReturnToPoolAfterDelay(pool, delay);
            }
            else
            {
                // Devolver inmediatamente si hay algún problema
                Return(poolName, obj);
            }
        }

        #endregion

        #region Pool Management

        /// <summary>
        /// Verifica si existe un pool específico
        /// </summary>
        /// <param name="poolName">Nombre del pool</param>
        /// <returns>True si el pool existe</returns>
        public bool HasPool(string poolName)
        {
            return pools.ContainsKey(poolName);
        }

        /// <summary>
        /// Obtiene todos los nombres de pools existentes
        /// </summary>
        /// <returns>Array con los nombres de los pools</returns>
        public string[] GetPoolNames()
        {
            string[] names = new string[pools.Count];
            pools.Keys.CopyTo(names, 0);
            return names;
        }

        /// <summary>
        /// Precarga objetos adicionales en un pool
        /// </summary>
        /// <param name="poolName">Nombre del pool</param>
        /// <param name="count">Número de objetos a precargar</param>
        public void PreloadPool(string poolName, int count)
        {
            if (pools.TryGetValue(poolName, out object poolObj))
            {
                // Usar reflexión para llamar al método Preload
                var preloadMethod = poolObj.GetType().GetMethod("Preload");
                if (preloadMethod != null)
                {
                    preloadMethod.Invoke(poolObj, new object[] { count });
                }
            }
        }

        /// <summary>
        /// Reduce el tamaño de un pool
        /// </summary>
        /// <param name="poolName">Nombre del pool</param>
        /// <param name="targetSize">Tamaño objetivo</param>
        public void ShrinkPool(string poolName, int targetSize)
        {
            if (pools.TryGetValue(poolName, out object poolObj))
            {
                // Usar reflexión para llamar al método Shrink
                var shrinkMethod = poolObj.GetType().GetMethod("Shrink");
                if (shrinkMethod != null)
                {
                    shrinkMethod.Invoke(poolObj, new object[] { targetSize });
                }
            }
        }

        /// <summary>
        /// Elimina un pool específico y destruye todos sus objetos
        /// </summary>
        /// <param name="poolName">Nombre del pool a eliminar</param>
        public void RemovePool(string poolName)
        {
            if (pools.TryGetValue(poolName, out object poolObj))
            {
                // Limpiar el pool usando reflexión
                var clearMethod = poolObj.GetType().GetMethod("Clear");
                if (clearMethod != null)
                {
                    clearMethod.Invoke(poolObj, null);
                }

                pools.Remove(poolName);
                LogDebug($"Removed pool '{poolName}'");
            }
        }

        /// <summary>
        /// Limpia todos los pools
        /// </summary>
        public void ClearAllPools()
        {
            LogDebug($"Clearing {pools.Count} pools");

            foreach (var kvp in pools)
            {
                // Limpiar cada pool usando reflexión
                var clearMethod = kvp.Value.GetType().GetMethod("Clear");
                if (clearMethod != null)
                {
                    clearMethod.Invoke(kvp.Value, null);
                }
            }

            pools.Clear();

            // Destruir el transform padre
            if (poolParent != null)
            {
                Destroy(poolParent.gameObject);
                poolParent = null;
            }

            LogDebug("All pools cleared");
        }

        #endregion

        #region Statistics and Debug

        /// <summary>
        /// Obtiene estadísticas de un pool específico
        /// </summary>
        /// <param name="poolName">Nombre del pool</param>
        /// <returns>Estadísticas del pool o null si no existe</returns>
        public string GetPoolStatistics(string poolName)
        {
            if (pools.TryGetValue(poolName, out object poolObj))
            {
                // Obtener estadísticas usando reflexión
                var getStatsMethod = poolObj.GetType().GetMethod("GetStatistics");
                if (getStatsMethod != null)
                {
                    var stats = getStatsMethod.Invoke(poolObj, null);
                    return $"{poolName}: {stats}";
                }
            }
            return $"{poolName}: Pool not found";
        }

        /// <summary>
        /// Obtiene estadísticas de todos los pools
        /// </summary>
        /// <returns>Estadísticas de todos los pools</returns>
        public string GetAllPoolStatistics()
        {
            string stats = $"PoolManager Statistics ({pools.Count} pools):\n";

            foreach (var poolName in pools.Keys)
            {
                stats += $"- {GetPoolStatistics(poolName)}\n";
            }

            return stats;
        }

        /// <summary>
        /// Obtiene información de debug completa
        /// </summary>
        public string GetDebugInfo()
        {
            string info = "=== POOL MANAGER DEBUG ===\n\n";
            info += $"Total Pools: {pools.Count}\n";
            info += $"Default Initial Size: {defaultInitialSize}\n";
            info += $"Default Max Size: {defaultMaxSize}\n";
            info += $"Default Auto Expand: {defaultAutoExpand}\n\n";

            info += "POOL DETAILS:\n";
            foreach (var kvp in pools)
            {
                // Obtener debug info usando reflexión
                var debugMethod = kvp.Value.GetType().GetMethod("GetDebugInfo");
                if (debugMethod != null)
                {
                    var debugInfo = debugMethod.Invoke(kvp.Value, null);
                    info += $"{debugInfo}\n\n";
                }
            }

            return info;
        }

        /// <summary>
        /// Valida el estado de todos los pools
        /// </summary>
        /// <returns>True si todos los pools son válidos</returns>
        public bool ValidateAllPools()
        {
            bool allValid = true;

            foreach (var kvp in pools)
            {
                // Validar cada pool usando reflexión
                var validateMethod = kvp.Value.GetType().GetMethod("ValidatePool");
                if (validateMethod != null)
                {
                    bool isValid = (bool)validateMethod.Invoke(kvp.Value, null);
                    if (!isValid)
                    {
                        LogDebug($"Pool '{kvp.Key}' validation failed");
                        allValid = false;
                    }
                }
            }

            LogDebug($"Pool validation: {(allValid ? "PASSED" : "FAILED")}");
            return allValid;
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Crea pools comunes para juegos FPS
        /// </summary>
        public void CreateCommonPools()
        {
            LogDebug("Creating common FPS game pools...");

            // Pool de proyectiles (si existe el prefab)
            // CreatePool("Projectile", projectilePrefab, 20, 100);

            // Pool de efectos de impacto
            // CreatePool("ImpactEffect", impactEffectPrefab, 10, 50);

            // Pool de casquillos
            // CreatePool("BulletCasing", bulletCasingPrefab, 15, 75);

            // Pool de enemigos (se crearía dinámicamente por tipo)
            // CreatePool("BasicEnemy", basicEnemyPrefab, 5, 30);

            LogDebug("Common pools creation attempted (requires prefabs to be assigned)");
        }

        /// <summary>
        /// Optimiza todos los pools reduciendo objetos no utilizados
        /// </summary>
        public void OptimizePools()
        {
            LogDebug("Optimizing pools...");

            foreach (var poolName in pools.Keys)
            {
                ShrinkPool(poolName, defaultInitialSize / 2);
            }

            LogDebug("Pool optimization completed");
        }

        #endregion

        private void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[PoolManager] {message}");
#endif
        }
    }
}
