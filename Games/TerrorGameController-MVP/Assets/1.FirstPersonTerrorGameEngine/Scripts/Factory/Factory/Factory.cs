using UnityEngine;
using System.Collections.Generic;

namespace HorrorEngine
{
    public class Factory
{
    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();
    private readonly object poolLock = new object();

    public GameObject GetOrCreateGameObject(GameObject prefab = null, Vector3? position = null, Quaternion? rotation = null)
    {
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
            // Check if a pool exists for this prefab
            if (objectPools.ContainsKey(key) && objectPools[key].Count > 0)
            {
                GameObject pooledObject = objectPools[key].Dequeue();
                pooledObject.transform.position = spawnPosition;
                pooledObject.transform.rotation = spawnRotation;
                pooledObject.SetActive(true);
                return pooledObject;
            }

            // If no object is available, create a new one
            GameObject newObject = Object.Instantiate(prefab, spawnPosition, spawnRotation);
            newObject.name = key; // Ensure the name matches the key
            return newObject;
        }
    }

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
            // Ensure the pool exists
            if (!objectPools.ContainsKey(key))
            {
                objectPools[key] = new Queue<GameObject>();
            }

            // Deactivate and return the object to the pool
            gameObject.SetActive(false);
            objectPools[key].Enqueue(gameObject);
        }
    }

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
    }
}
