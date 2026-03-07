using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HorrorEngine
{
    [System.Serializable] // Permite que esta clase sea serializable por Unity.
    public class GameData
{
    public long lastUpdated; // Marca de tiempo de la última actualización de los datos.
    public int deathCount; // Número de muertes del jugador.
    public Vector3 playerPosition; // Posición del jugador en el mundo.
    public SerializableDictionary<string, bool> FlagCollected; // Diccionario para rastrear banderas recolectadas.
    public SerializableDictionary<string, bool> EventsCollected; // Diccionario para rastrear eventos completados.
    public AttributesData playerAttributesData; // Datos relacionados con los atributos del jugador.
    public TasksData tasksData; // Datos relacionados con las tareas del juego.
    public InventorySaveData inventoryData; // Datos relacionados con el inventario del jugador.

    // Constructor que define los valores predeterminados cuando no hay datos para cargar.
    public GameData() 
    {
        this.deathCount = 0; // Inicializa el contador de muertes en 0.
        playerPosition = Vector3.zero; // Inicializa la posición del jugador en el origen (0, 0, 0).
        FlagCollected = new SerializableDictionary<string, bool>(); // Inicializa el diccionario de banderas.
        EventsCollected = new SerializableDictionary<string, bool>(); // Inicializa el diccionario de eventos.
        playerAttributesData = new AttributesData(); // Inicializa los atributos del jugador con valores predeterminados.
        tasksData = new TasksData(); // Inicializa los datos de tareas.
        inventoryData = new InventorySaveData(); // Inicializa los datos del inventario.
    }
    }
}
