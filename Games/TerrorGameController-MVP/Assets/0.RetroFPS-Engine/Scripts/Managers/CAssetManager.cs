using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

namespace RetroFPS
{
    /// <summary>
    /// Sistema básico de gestión de assets con Addressables para Retro FPS Engine.
    /// Diseñado para ser simple y eficiente, sin la complejidad del motor de terror.
    /// </summary>
    public class CAssetManager : MonoBehaviour
    {
        public static CAssetManager Instance { get; private set; }

        [Header("Asset Manager Configuration")]
        [Tooltip("Habilitar logs de debug")]
        [SerializeField] private bool enableDebugLogs = true;

        // Cache de assets cargados
        private Dictionary<string, Object> assetCache = new Dictionary<string, Object>();
        private Dictionary<string, AsyncOperationHandle> activeHandles = new Dictionary<string, AsyncOperationHandle>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            // Liberar todos los handles activos
            foreach (var handle in activeHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }
            activeHandles.Clear();
            assetCache.Clear();
        }

        /// <summary>
        /// Carga un asset de manera síncrona (solo para assets críticos)
        /// </summary>
        public T LoadAsset<T>(string address) where T : Object
        {
            if (assetCache.TryGetValue(address, out Object cachedAsset))
            {
                LogDebug($"Asset '{address}' loaded from cache");
                return cachedAsset as T;
            }

            var handle = Addressables.LoadAssetAsync<T>(address);
            handle.WaitForCompletion();

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                assetCache[address] = handle.Result;
                activeHandles[address] = handle;
                LogDebug($"Asset '{address}' loaded synchronously");
                return handle.Result;
            }
            else
            {
                LogDebug($"Failed to load asset '{address}': {handle.Status}");
                return null;
            }
        }

        /// <summary>
        /// Carga un asset de manera asíncrona
        /// </summary>
        public async Task<T> LoadAssetAsync<T>(string address) where T : Object
        {
            if (assetCache.TryGetValue(address, out Object cachedAsset))
            {
                LogDebug($"Asset '{address}' loaded from cache");
                return cachedAsset as T;
            }

            try
            {
                LogDebug($"Loading asset asynchronously: '{address}'");
                var handle = Addressables.LoadAssetAsync<T>(address);
                activeHandles[address] = handle;

                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    assetCache[address] = handle.Result;
                    LogDebug($"Asset '{address}' loaded successfully");
                    return handle.Result;
                }
                else
                {
                    LogDebug($"Failed to load asset '{address}': {handle.Status}");
                    return null;
                }
            }
            catch (System.Exception e)
            {
                LogDebug($"Exception loading asset '{address}': {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Instancia un prefab cargado con Addressables
        /// </summary>
        public async Task<GameObject> InstantiateAssetAsync(string address, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            GameObject prefab = await LoadAssetAsync<GameObject>(address);
            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab, position, rotation, parent);
                LogDebug($"Instantiated asset '{address}' at {position}");
                return instance;
            }
            return null;
        }

        /// <summary>
        /// Carga una escena con Addressables
        /// </summary>
        public async Task LoadSceneAsync(string sceneAddress, LoadSceneMode loadMode = LoadSceneMode.Single)
        {
            try
            {
                LogDebug($"Loading scene: '{sceneAddress}'");
                var handle = Addressables.LoadSceneAsync(sceneAddress, loadMode);
                activeHandles[sceneAddress] = handle;

                await handle.Task;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    LogDebug($"Scene '{sceneAddress}' loaded successfully");
                }
                else
                {
                    LogDebug($"Failed to load scene '{sceneAddress}': {handle.Status}");
                }
            }
            catch (System.Exception e)
            {
                LogDebug($"Exception loading scene '{sceneAddress}': {e.Message}");
            }
        }

        /// <summary>
        /// Descarga un asset específico
        /// </summary>
        public void UnloadAsset(string address)
        {
            if (activeHandles.TryGetValue(address, out AsyncOperationHandle handle))
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
                activeHandles.Remove(address);
            }

            if (assetCache.ContainsKey(address))
            {
                assetCache.Remove(address);
            }

            LogDebug($"Unloaded asset: '{address}'");
        }

        /// <summary>
        /// Limpia la cache de assets (útil para cambios de nivel)
        /// </summary>
        public void ClearCache()
        {
            foreach (var handle in activeHandles.Values)
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
            }

            activeHandles.Clear();
            assetCache.Clear();
            LogDebug("Asset cache cleared");
        }

        /// <summary>
        /// Verifica si un asset está en cache
        /// </summary>
        public bool IsAssetLoaded(string address)
        {
            return assetCache.ContainsKey(address);
        }

        /// <summary>
        /// Obtiene información de debug
        /// </summary>
        public string GetDebugInfo()
        {
            return $"AssetManager Debug Info:\n" +
                   $"- Assets in cache: {assetCache.Count}\n" +
                   $"- Active handles: {activeHandles.Count}\n" +
                   $"- Debug logs: {(enableDebugLogs ? "Enabled" : "Disabled")}";
        }

        private void LogDebug(string message)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[AssetManager] {message}");
            }
        }
    }

    /// <summary>
    /// Extensión para facilitar el uso del AssetManager
    /// </summary>
    public static class AssetManagerExtensions
    {
        /// <summary>
        /// Carga y instancia un prefab en una sola llamada
        /// </summary>
        public static async Task<GameObject> LoadAndInstantiateAsync(
            this CAssetManager manager,
            string address,
            Vector3 position = default,
            Quaternion rotation = default,
            Transform parent = null)
        {
            return await manager.InstantiateAssetAsync(address, position, rotation, parent);
        }
    }
}
