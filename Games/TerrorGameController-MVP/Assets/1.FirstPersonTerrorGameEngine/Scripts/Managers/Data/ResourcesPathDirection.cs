using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "PathPrefab", menuName = "HorrorEngine/ResourcesPathDirection", order = 1)]
public class ResourcesPathDirection : ScriptableObject
{
    [Header("Configuración de Prefabs")]
    [SerializeField]
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    
    [Header("Configuración de Rutas (Legacy - Solo para compatibilidad)")]
    [SerializeField]
    private List<string> resourcePaths = new List<string>();
    
    [Header("Configuración")]
    [SerializeField]
    private bool usePrefabReferences = true;
    
    [SerializeField]
    private bool validateOnLoad = true;

    #region Properties
    /// <summary>
    /// Lista de prefabs de enemigos configurados
    /// </summary>
    public List<GameObject> EnemyPrefabs
    {
        get
        {
            if (usePrefabReferences)
            {
                return enemyPrefabs.Where(prefab => prefab != null).ToList();
            }
            return new List<GameObject>();
        }
    }
    
    /// <summary>
    /// Lista de rutas de recursos (legacy)
    /// </summary>
    public List<string> ResourcePaths
    {
        get
        {
            if (!usePrefabReferences)
            {
                return resourcePaths.Where(path => !string.IsNullOrEmpty(path)).ToList();
            }
            return new List<string>();
        }
    }
    
    /// <summary>
    /// Número total de enemigos configurados
    /// </summary>
    public int EnemyCount
    {
        get
        {
            if (usePrefabReferences)
            {
                return enemyPrefabs.Count(prefab => prefab != null);
            }
            return resourcePaths.Count(path => !string.IsNullOrEmpty(path));
        }
    }
    
    /// <summary>
    /// Indica si hay enemigos configurados
    /// </summary>
    public bool HasEnemies
    {
        get { return EnemyCount > 0; }
    }
    #endregion

    #region Unity Lifecycle
    private void OnValidate()
    {
        if (validateOnLoad)
        {
            ValidateConfiguration();
        }
    }
    
    private void OnEnable()
    {
        if (validateOnLoad)
        {
            ValidateConfiguration();
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Obtiene un prefab de enemigo por índice
    /// </summary>
    /// <param name="index">Índice del enemigo</param>
    /// <returns>Prefab del enemigo o null si no existe</returns>
    public GameObject GetEnemyPrefab(int index)
    {
        if (usePrefabReferences)
        {
            if (index >= 0 && index < enemyPrefabs.Count)
            {
                return enemyPrefabs[index];
            }
        }
        else
        {
            if (index >= 0 && index < resourcePaths.Count)
            {
                string path = resourcePaths[index];
                if (!string.IsNullOrEmpty(path))
                {
                    return Resources.Load<GameObject>(path);
                }
            }
        }
        
        Debug.LogWarning($"Índice de enemigo inválido: {index}. Total de enemigos: {EnemyCount}");
        return null;
    }
    
    /// <summary>
    /// Obtiene un prefab de enemigo aleatorio
    /// </summary>
    /// <returns>Prefab aleatorio o null si no hay enemigos</returns>
    public GameObject GetRandomEnemyPrefab()
    {
        if (!HasEnemies)
        {
            Debug.LogWarning("No hay enemigos configurados");
            return null;
        }
        
        int randomIndex = Random.Range(0, EnemyCount);
        return GetEnemyPrefab(randomIndex);
    }
    
    /// <summary>
    /// Obtiene todos los prefabs de enemigos válidos
    /// </summary>
    /// <returns>Lista de prefabs válidos</returns>
    public List<GameObject> GetAllValidEnemyPrefabs()
    {
        if (usePrefabReferences)
        {
            return enemyPrefabs.Where(prefab => prefab != null).ToList();
        }
        else
        {
            List<GameObject> validPrefabs = new List<GameObject>();
            foreach (string path in resourcePaths)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    GameObject prefab = Resources.Load<GameObject>(path);
                    if (prefab != null)
                    {
                        validPrefabs.Add(prefab);
                    }
                    else
                    {
                        Debug.LogWarning($"No se pudo cargar el prefab desde la ruta: {path}");
                    }
                }
            }
            return validPrefabs;
        }
    }
    
    /// <summary>
    /// Agrega un prefab de enemigo a la lista
    /// </summary>
    /// <param name="enemyPrefab">Prefab del enemigo</param>
    public void AddEnemyPrefab(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("No se puede agregar un prefab null");
            return;
        }
        
        if (usePrefabReferences)
        {
            if (!enemyPrefabs.Contains(enemyPrefab))
            {
                enemyPrefabs.Add(enemyPrefab);
                Debug.Log($"Enemigo agregado: {enemyPrefab.name}");
            }
            else
            {
                Debug.LogWarning($"El enemigo {enemyPrefab.name} ya está en la lista");
            }
        }
        else
        {
            Debug.LogWarning("No se pueden agregar prefabs cuando usePrefabReferences está desactivado");
        }
    }
    
    /// <summary>
    /// Remueve un prefab de enemigo de la lista
    /// </summary>
    /// <param name="enemyPrefab">Prefab del enemigo a remover</param>
    public void RemoveEnemyPrefab(GameObject enemyPrefab)
    {
        if (usePrefabReferences)
        {
            if (enemyPrefabs.Remove(enemyPrefab))
            {
                Debug.Log($"Enemigo removido: {enemyPrefab.name}");
            }
            else
            {
                Debug.LogWarning($"El enemigo {enemyPrefab.name} no estaba en la lista");
            }
        }
        else
        {
            Debug.LogWarning("No se pueden remover prefabs cuando usePrefabReferences está desactivado");
        }
    }
    
    /// <summary>
    /// Limpia todos los prefabs de enemigos
    /// </summary>
    public void ClearAllEnemyPrefabs()
    {
        if (usePrefabReferences)
        {
            enemyPrefabs.Clear();
            Debug.Log("Todos los enemigos han sido removidos");
        }
        else
        {
            resourcePaths.Clear();
            Debug.Log("Todas las rutas han sido removidas");
        }
    }
    
    /// <summary>
    /// Valida la configuración actual
    /// </summary>
    /// <returns>True si la configuración es válida</returns>
    public bool ValidateConfiguration()
    {
        bool isValid = true;
        
        if (usePrefabReferences)
        {
            // Validar prefabs
            for (int i = 0; i < enemyPrefabs.Count; i++)
            {
                if (enemyPrefabs[i] == null)
                {
                    Debug.LogError($"Prefab de enemigo en índice {i} es null");
                    isValid = false;
                }
                else
                {
                    // Verificar que sea un prefab válido
                    if (enemyPrefabs[i].scene.name != null)
                    {
                        Debug.LogWarning($"El objeto {enemyPrefabs[i].name} en índice {i} no es un prefab (está en escena)");
                    }
                }
            }
        }
        else
        {
            // Validar rutas
            for (int i = 0; i < resourcePaths.Count; i++)
            {
                if (string.IsNullOrEmpty(resourcePaths[i]))
                {
                    Debug.LogError($"Ruta de recurso en índice {i} está vacía");
                    isValid = false;
                }
                else
                {
                    // Verificar que la ruta sea válida
                    GameObject testPrefab = Resources.Load<GameObject>(resourcePaths[i]);
                    if (testPrefab == null)
                    {
                        Debug.LogError($"No se puede cargar el prefab desde la ruta: {resourcePaths[i]}");
                        isValid = false;
                    }
                }
            }
        }
        
        if (EnemyCount == 0)
        {
            Debug.LogWarning("No hay enemigos configurados en ResourcesPathDirection");
        }
        
        return isValid;
    }
    
    /// <summary>
    /// Obtiene información de debug sobre la configuración
    /// </summary>
    /// <returns>String con información de debug</returns>
    public string GetDebugInfo()
    {
        string info = $"ResourcesPathDirection Debug Info:\n";
        info += $"Use Prefab References: {usePrefabReferences}\n";
        info += $"Total Enemies: {EnemyCount}\n";
        info += $"Has Enemies: {HasEnemies}\n";
        
        if (usePrefabReferences)
        {
            info += $"Prefab References:\n";
            for (int i = 0; i < enemyPrefabs.Count; i++)
            {
                info += $"  [{i}] {(enemyPrefabs[i] != null ? enemyPrefabs[i].name : "NULL")}\n";
            }
        }
        else
        {
            info += $"Resource Paths:\n";
            for (int i = 0; i < resourcePaths.Count; i++)
            {
                info += $"  [{i}] {resourcePaths[i]}\n";
            }
        }
        
        return info;
    }
    #endregion

    #region Legacy Support
    /// <summary>
    /// Convierte las rutas de recursos a prefabs (para compatibilidad)
    /// </summary>
    public void ConvertPathsToPrefabs()
    {
        if (!usePrefabReferences)
        {
            Debug.LogWarning("No se puede convertir cuando usePrefabReferences está desactivado");
            return;
        }
        
        enemyPrefabs.Clear();
        
        foreach (string path in resourcePaths)
        {
            if (!string.IsNullOrEmpty(path))
            {
                GameObject prefab = Resources.Load<GameObject>(path);
                if (prefab != null)
                {
                    enemyPrefabs.Add(prefab);
                    Debug.Log($"Convertido: {path} → {prefab.name}");
                }
                else
                {
                    Debug.LogError($"No se pudo cargar el prefab desde: {path}");
                }
            }
        }
        
        Debug.Log($"Conversión completada. {enemyPrefabs.Count} prefabs cargados.");
    }
    
    /// <summary>
    /// Convierte los prefabs a rutas de recursos (para compatibilidad)
    /// </summary>
    public void ConvertPrefabsToPaths()
    {
        if (usePrefabReferences)
        {
            Debug.LogWarning("No se puede convertir cuando usePrefabReferences está activado");
            return;
        }
        
        resourcePaths.Clear();
        
        foreach (GameObject prefab in enemyPrefabs)
        {
            if (prefab != null)
            {
                string path = GetResourcePath(prefab);
                if (!string.IsNullOrEmpty(path))
                {
                    resourcePaths.Add(path);
                    Debug.Log($"Convertido: {prefab.name} → {path}");
                }
                else
                {
                    Debug.LogError($"No se pudo obtener la ruta para: {prefab.name}");
                }
            }
        }
        
        Debug.Log($"Conversión completada. {resourcePaths.Count} rutas generadas.");
    }
    
    /// <summary>
    /// Obtiene la ruta de recursos de un prefab
    /// </summary>
    /// <param name="prefab">Prefab del cual obtener la ruta</param>
    /// <returns>Ruta del recurso o string vacío si no se puede obtener</returns>
    private string GetResourcePath(GameObject prefab)
    {
        if (prefab == null) return string.Empty;
        
        string assetPath = UnityEditor.AssetDatabase.GetAssetPath(prefab);
        
        // Extraer la parte después de "Resources/"
        int resourcesIndex = assetPath.IndexOf("Resources/");
        if (resourcesIndex >= 0)
        {
            string resourcePath = assetPath.Substring(resourcesIndex + 10); // "Resources/" tiene 10 caracteres
            return resourcePath.Replace(".prefab", ""); // Remover extensión
        }
        
        return string.Empty;
    }
    #endregion
}
