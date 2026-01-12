using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace HorrorEngine
{
    public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false; // Desactiva la persistencia de datos para pruebas.
    [SerializeField] private bool initializeDataIfNull = false; // Inicializa datos si no se encuentran.
    [SerializeField] private bool overrideSelectedProfileId = false; // Sobrescribe el ID del perfil seleccionado.
    [SerializeField] private string testSelectedProfileId = "test"; // ID de perfil para pruebas.

    [Header("File Storage Config")]
    [SerializeField] private string fileName; // Nombre del archivo de datos.
    [SerializeField] private bool useEncryption; // Indica si se debe usar encriptación.

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTimeSeconds = 60f; // Intervalo de tiempo para el guardado automático.

    private GameData gameData; // Contenedor de los datos del juego.
    private List<IDataPersistence> dataPersistenceObjects; // Lista de objetos que implementan IDataPersistence.
    private FileDataHandler dataHandler; // Manejador de archivos para guardar y cargar datos.

    private string selectedProfileId = ""; // ID del perfil seleccionado.

    private Coroutine autoSaveCoroutine; // Corrutina para el guardado automático.

    public static DataPersistenceManager instance { get; private set; } // Instancia singleton.

    private void Awake() 
    {
        // Garantiza que solo haya una instancia de DataPersistenceManager.
        if (instance != null) 
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject); // Evita que el objeto se destruya al cambiar de escena.

        if (disableDataPersistence) 
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        // Inicializa el manejador de archivos con la ruta de almacenamiento persistente.
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileId(); // Inicializa el ID del perfil seleccionado.
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Se suscribe al evento de carga de escena.
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Se desuscribe del evento de carga de escena.
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects(); // Encuentra todos los objetos que implementan IDataPersistence.
        LoadGame(); // Carga los datos del juego.

        // Inicia la corrutina de guardado automático.
        if (autoSaveCoroutine != null) 
        {
            StopCoroutine(autoSaveCoroutine);
        }
        autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    public void ChangeSelectedProfileId(string newProfileId) 
    {
        this.selectedProfileId = newProfileId; // Cambia el ID del perfil seleccionado.
        LoadGame(); // Carga los datos del nuevo perfil.
    }

    public void DeleteProfileData(string profileId) 
    {
        dataHandler.Delete(profileId); // Elimina los datos del perfil especificado.
        InitializeSelectedProfileId(); // Reestablece el ID del perfil seleccionado.
        LoadGame(); // Recarga los datos del juego.
    }

    private void InitializeSelectedProfileId() 
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId(); // Obtiene el perfil más reciente.
        if (overrideSelectedProfileId) 
        {
            this.selectedProfileId = testSelectedProfileId; // Sobrescribe el ID del perfil para pruebas.
            Debug.LogWarning("Overrode selected profile id with test id: " + testSelectedProfileId);
        }
    }

    public void NewGame() 
    {
        this.gameData = new GameData(); // Crea nuevos datos del juego.
    }

    public void LoadGame()
    {
        if (disableDataPersistence) 
        {
            return; // No carga datos si la persistencia está desactivada.
        }

        this.gameData = dataHandler.Load(selectedProfileId); // Carga los datos del perfil seleccionado.

        if (this.gameData == null && initializeDataIfNull) 
        {
            NewGame(); // Inicializa nuevos datos si no se encuentran y está habilitado.
        }

        if (this.gameData == null) 
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return; // No continúa si no hay datos.
        }

        // Actualiza los datos en todos los objetos que implementan IDataPersistence.
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (disableDataPersistence) 
        {
            return; // No guarda datos si la persistencia está desactivada.
        }

        if (this.gameData == null) 
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return; // No guarda si no hay datos.
        }

        // Actualiza los datos desde todos los objetos que implementan IDataPersistence.
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.SaveData(gameData);
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary(); // Marca de tiempo de la última actualización.

        dataHandler.Save(gameData, selectedProfileId); // Guarda los datos en un archivo.
    }

    private void OnApplicationQuit() 
    {
        SaveGame(); // Guarda los datos al cerrar la aplicación.
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects() 
    {
        // Encuentra todos los objetos que implementan IDataPersistence, incluyendo los inactivos.
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData() 
    {
        return gameData != null; // Devuelve true si hay datos del juego.
    }

    public Dictionary<string, GameData> GetAllProfilesGameData() 
    {
        return dataHandler.LoadAllProfiles(); // Carga los datos de todos los perfiles.
    }

    private IEnumerator AutoSave() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(autoSaveTimeSeconds); // Espera el tiempo configurado.
            SaveGame(); // Guarda los datos automáticamente.
            Debug.Log("Auto Saved Game");
        }
    }
    }
}
