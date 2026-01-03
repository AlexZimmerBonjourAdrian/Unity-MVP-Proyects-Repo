using System.Collections.Generic;
using UnityEngine;

namespace RetroFPS
{
    /// <summary>
    /// Object Pooling Pattern - Pool genérico para gestión eficiente de instancias de objetos.
    /// Reduce la creación/destrucción de objetos reutilizando instancias existentes.
    /// </summary>
    /// <typeparam name="T">Tipo del componente a poolar (debe heredar de Component)</typeparam>
    public class ObjectPool<T> where T : Component
    {
        // Cola para objetos disponibles
        private readonly Queue<T> availableObjects = new Queue<T>();

        // Lista de todos los objetos creados (para limpieza)
        private readonly List<T> allObjects = new List<T>();

        // Prefab original
        private readonly T prefab;

        // Configuración del pool
        private readonly Transform parent;
        private readonly int maxSize;
        private readonly bool autoExpand;

        // Estadísticas
        private int totalCreated = 0;
        private int totalRetrieved = 0;
        private int totalReturned = 0;

        /// <summary>
        /// Constructor del pool
        /// </summary>
        /// <param name="prefab">Prefab del objeto a poolar</param>
        /// <param name="initialSize">Tamaño inicial del pool</param>
        /// <param name="parent">Transform padre para los objetos</param>
        /// <param name="maxSize">Tamaño máximo del pool (0 = ilimitado)</param>
        /// <param name="autoExpand">Si el pool puede crecer automáticamente</param>
        public ObjectPool(T prefab, int initialSize = 10, Transform parent = null, int maxSize = 0, bool autoExpand = true)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.maxSize = maxSize;
            this.autoExpand = autoExpand;

            // Crear objetos iniciales
            for (int i = 0; i < initialSize; i++)
            {
                CreateNewObject();
            }

            LogDebug($"ObjectPool initialized with {initialSize} objects. Max size: {maxSize}, Auto-expand: {autoExpand}");
        }

        /// <summary>
        /// Obtiene un objeto del pool
        /// </summary>
        /// <returns>Objeto disponible o null si no hay ninguno y no puede expandir</returns>
        public T Get()
        {
            T obj;

            if (availableObjects.Count > 0)
            {
                // Reutilizar objeto existente
                obj = availableObjects.Dequeue();
            }
            else if (autoExpand && (maxSize == 0 || allObjects.Count < maxSize))
            {
                // Crear nuevo objeto
                obj = CreateNewObject();
            }
            else
            {
                // No hay objetos disponibles y no puede expandir
                LogDebug("No available objects in pool and cannot expand");
                return null;
            }

            // Activar y preparar el objeto
            if (obj != null)
            {
                obj.gameObject.SetActive(true);
                OnObjectRetrieved(obj);
                totalRetrieved++;
            }

            return obj;
        }

        /// <summary>
        /// Devuelve un objeto al pool
        /// </summary>
        /// <param name="obj">Objeto a devolver</param>
        public void Return(T obj)
        {
            if (obj == null)
            {
                LogDebug("Attempted to return null object");
                return;
            }

            if (!allObjects.Contains(obj))
            {
                LogDebug("Attempted to return object not managed by this pool");
                return;
            }

            // Preparar el objeto para devolver al pool
            OnObjectReturned(obj);

            // Desactivar el objeto
            obj.gameObject.SetActive(false);

            // Agregar a la cola de disponibles
            availableObjects.Enqueue(obj);
            totalReturned++;

            LogDebug($"Object returned to pool. Available: {availableObjects.Count}");
        }

        /// <summary>
        /// Precarga objetos adicionales al pool
        /// </summary>
        /// <param name="count">Número de objetos a precargar</param>
        public void Preload(int count)
        {
            int created = 0;
            for (int i = 0; i < count; i++)
            {
                if (maxSize == 0 || allObjects.Count < maxSize)
                {
                    CreateNewObject();
                    created++;
                }
                else
                {
                    break;
                }
            }

            if (created > 0)
            {
                LogDebug($"Preloaded {created} objects");
            }
        }

        /// <summary>
        /// Limpia el pool, destruyendo todos los objetos
        /// </summary>
        public void Clear()
        {
            LogDebug($"Clearing pool. Destroying {allObjects.Count} objects");

            foreach (T obj in allObjects)
            {
                if (obj != null && obj.gameObject != null)
                {
                    Object.Destroy(obj.gameObject);
                }
            }

            availableObjects.Clear();
            allObjects.Clear();
            totalCreated = 0;
            totalRetrieved = 0;
            totalReturned = 0;
        }

        /// <summary>
        /// Reduce el pool eliminando objetos no utilizados
        /// </summary>
        /// <param name="targetSize">Tamaño objetivo del pool</param>
        public void Shrink(int targetSize)
        {
            if (targetSize < 0) targetSize = 0;

            while (availableObjects.Count > targetSize && availableObjects.Count > 0)
            {
                T obj = availableObjects.Dequeue();
                if (obj != null)
                {
                    allObjects.Remove(obj);
                    Object.Destroy(obj.gameObject);
                    totalCreated--;
                }
            }

            LogDebug($"Pool shrunk to {availableObjects.Count} available objects");
        }

        /// <summary>
        /// Crea un nuevo objeto y lo agrega al pool
        /// </summary>
        private T CreateNewObject()
        {
            if (prefab == null)
            {
                LogDebug("Cannot create object: prefab is null");
                return null;
            }

            // Instanciar el objeto
            T obj = Object.Instantiate(prefab, parent);

            // Desactivar inicialmente
            obj.gameObject.SetActive(false);

            // Agregar a las listas
            allObjects.Add(obj);
            availableObjects.Enqueue(obj);

            totalCreated++;

            LogDebug($"New object created. Total created: {totalCreated}");

            return obj;
        }

        /// <summary>
        /// Método hook llamado cuando se obtiene un objeto del pool
        /// </summary>
        protected virtual void OnObjectRetrieved(T obj)
        {
            // Resetear estado del objeto si es necesario
            // Sobrescribir en subclases para lógica específica
        }

        /// <summary>
        /// Método hook llamado cuando se devuelve un objeto al pool
        /// </summary>
        protected virtual void OnObjectReturned(T obj)
        {
            // Resetear estado del objeto
            // Sobrescribir en subclases para lógica específica
        }

        #region Properties

        /// <summary>
        /// Número de objetos disponibles en el pool
        /// </summary>
        public int AvailableCount => availableObjects.Count;

        /// <summary>
        /// Número total de objetos creados
        /// </summary>
        public int TotalCount => allObjects.Count;

        /// <summary>
        /// Número de objetos activos (en uso)
        /// </summary>
        public int ActiveCount => TotalCount - AvailableCount;

        /// <summary>
        /// Tamaño máximo del pool (0 = ilimitado)
        /// </summary>
        public int MaxSize => maxSize;

        /// <summary>
        /// Si el pool puede expandirse automáticamente
        /// </summary>
        public bool AutoExpand => autoExpand;

        #endregion

        #region Statistics

        /// <summary>
        /// Estadísticas de uso del pool
        /// </summary>
        public PoolStatistics GetStatistics()
        {
            return new PoolStatistics
            {
                TotalCreated = totalCreated,
                TotalRetrieved = totalRetrieved,
                TotalReturned = totalReturned,
                CurrentAvailable = AvailableCount,
                CurrentActive = ActiveCount,
                UtilizationRate = TotalCount > 0 ? (float)ActiveCount / TotalCount : 0f
            };
        }

        /// <summary>
        /// Estructura para estadísticas del pool
        /// </summary>
        public struct PoolStatistics
        {
            public int TotalCreated;
            public int TotalRetrieved;
            public int TotalReturned;
            public int CurrentAvailable;
            public int CurrentActive;
            public float UtilizationRate;

            public override string ToString()
            {
                return $"Created: {TotalCreated}, Retrieved: {TotalRetrieved}, Returned: {TotalReturned}, " +
                       $"Available: {CurrentAvailable}, Active: {CurrentActive}, Utilization: {UtilizationRate:P2}";
            }
        }

        #endregion

        #region Debug

        /// <summary>
        /// Obtiene información de debug del pool
        /// </summary>
        public string GetDebugInfo()
        {
            var stats = GetStatistics();
            return $"ObjectPool<{typeof(T).Name}> Debug Info:\n" +
                   $"- Prefab: {(prefab != null ? prefab.name : "null")}\n" +
                   $"- Parent: {(parent != null ? parent.name : "null")}\n" +
                   $"- Max Size: {maxSize} (0 = unlimited)\n" +
                   $"- Auto Expand: {autoExpand}\n" +
                   $"- Available: {AvailableCount}\n" +
                   $"- Active: {ActiveCount}\n" +
                   $"- Total Created: {totalCreated}\n" +
                   $"- Utilization: {stats.UtilizationRate:P2}";
        }

        /// <summary>
        /// Valida el estado del pool (para debugging)
        /// </summary>
        public bool ValidatePool()
        {
            bool valid = true;

            // Verificar que no hay objetos nulos
            foreach (T obj in allObjects)
            {
                if (obj == null)
                {
                    LogDebug("Found null object in allObjects list");
                    valid = false;
                }
            }

            // Verificar consistencia de conteos
            if (availableObjects.Count + ActiveCount != TotalCount)
            {
                LogDebug($"Count inconsistency: Available({AvailableCount}) + Active({ActiveCount}) != Total({TotalCount})");
                valid = false;
            }

            if (valid)
            {
                LogDebug("Pool validation passed");
            }
            else
            {
                LogDebug("Pool validation failed");
            }

            return valid;
        }

        private void LogDebug(string message)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[ObjectPool<{typeof(T).Name}>] {message}");
#endif
        }

        #endregion
    }

    /// <summary>
    /// Extensión para facilitar el uso de ObjectPool
    /// </summary>
    public static class ObjectPoolExtensions
    {
        /// <summary>
        /// Devuelve un objeto al pool automáticamente cuando se desactiva
        /// </summary>
        public static void ReturnToPoolOnDisable<T>(this T obj, ObjectPool<T> pool) where T : Component
        {
            var returner = obj.gameObject.AddComponent<PoolReturner<T>>();
            returner.Pool = pool;
        }

        /// <summary>
        /// Devuelve un objeto al pool automáticamente después de un tiempo
        /// </summary>
        public static void ReturnToPoolAfterDelay<T>(this T obj, ObjectPool<T> pool, float delay) where T : Component
        {
            MonoBehaviour monoBehaviour = obj as MonoBehaviour;
            if (monoBehaviour != null)
            {
                monoBehaviour.StartCoroutine(DelayedReturn(obj, pool, delay));
            }
            else
            {
                // Si no es MonoBehaviour, usar un componente helper
                var helper = obj.gameObject.AddComponent<PoolReturnerHelper>();
                helper.StartCoroutine(DelayedReturn(obj, pool, delay));
            }
        }

        private static System.Collections.IEnumerator DelayedReturn<T>(T obj, ObjectPool<T> pool, float delay) where T : Component
        {
            yield return new WaitForSeconds(delay);
            pool.Return(obj);
        }
    }

    /// <summary>
    /// Componente helper que devuelve objetos al pool cuando se desactivan
    /// </summary>
    public class PoolReturner<T> : MonoBehaviour where T : Component
    {
        public ObjectPool<T> Pool { get; set; }

        private void OnDisable()
        {
            if (Pool != null)
            {
                Pool.Return(GetComponent<T>());
            }
        }
    }

    /// <summary>
    /// Componente helper simple para ejecutar coroutines cuando el componente no es MonoBehaviour
    /// </summary>
    internal class PoolReturnerHelper : MonoBehaviour
    {
        // Este componente solo existe para poder ejecutar coroutines
        // Se destruye automáticamente cuando la coroutine termina
    }
}
