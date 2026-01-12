using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    public class CNoteManager : MonoBehaviour
{
    public static CNoteManager Instance { get; private set; }

    [SerializeField] private GameObject notePrefab; // Prefab de la nota
    [SerializeField] private Transform spawnPoint; // Punto de spawn para las notas

    private List<GameObject> activeNotes = new List<GameObject>(); // Lista de notas activas

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Limpiar notas que dejaron de existir
        for (int i = activeNotes.Count - 1; i >= 0; i--)
        {
            if (activeNotes[i] == null)
            {
                activeNotes.RemoveAt(i);
            }
        }
    }

    public GameObject SpawnNote()
    {
        if (notePrefab == null || spawnPoint == null)
        {
            Debug.LogError("Note prefab or spawn point is not assigned!");
            return null;
        }

        GameObject newNote = Instantiate(notePrefab, spawnPoint.position, spawnPoint.rotation);
        activeNotes.Add(newNote);
        return newNote;
    }

    public void RemoveNote(GameObject note)
    {
        if (activeNotes.Contains(note))
        {
            activeNotes.Remove(note);
            Destroy(note); // O usar un sistema de pooling si es necesario
        }
    }
    }
}