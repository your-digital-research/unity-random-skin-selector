using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    #region SINGLETON

    private static DataPersistenceManager instance;

    public static DataPersistenceManager Instance
    {
        get
        {
            if (instance == null) Debug.LogError("\"DataPersistenceManager\" NOT INITIALIZED");
            return instance;
        }

        private set => instance = value;
    }

    #endregion

    #region EVENTS

    //

    #endregion

    #region PROPERTIES

    public GameData GameData => gameData;

    #endregion

    #region PUBLIC_VARIABLES

    //

    #endregion

    #region PRIVATE_VARIABLES

    private GameData gameData;
    private FileDataHandler fileDataHandler;
    private List<IDataPersistence> dataPersistenceObjects;

    #endregion

    #region SERIALIZED_VARIABLES

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    [SerializeField] [Range(1, 10)] private float gameSaveFrequency;

    #endregion

    #region PRIVATE_VARIABLES

    private Coroutine gameSaveCoroutineC;

    #endregion

    #region PUBLIC_FUNCTIONS

    public GameData GetGameData()
    {
        return gameData;
    }

    public void Init()
    {
        InitDataHandler();
        LoadGame();
    }

    public void LoadGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        // Load any saved data from a file using the data handler
        gameData = fileDataHandler.Load();

        // If no data can be loaded, initialize to a new game
        if (gameData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults");

            NewGame();
        }

        // Push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        dataPersistenceObjects = FindAllDataPersistenceObjects();

        // Pass the data to other scripts so they can update it
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(gameData);
        }

        // Save that data to a file using the data handler
        fileDataHandler.Save(gameData);
    }

    public void ResetData()
    {
        fileDataHandler.Reset();
        ResetDataHandler();
    }

    public void StartGameSave()
    {
        if (gameSaveCoroutineC != null) StopCoroutine(gameSaveCoroutineC);
        gameSaveCoroutineC = StartCoroutine(GameSaveCoroutine());
    }

    public void StopGameSave()
    {
        if (gameSaveCoroutineC != null) StopCoroutine(gameSaveCoroutineC);
        gameSaveCoroutineC = null;
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    private void NewGame()
    {
        gameData = new GameData();
    }

    private void InitDataHandler()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    private void ResetDataHandler()
    {
        fileDataHandler = null;
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        bool includeInactive = false;
        IEnumerable<IDataPersistence> foundedDataPersistenceObjects =
            FindObjectsOfType<MonoBehaviour>(includeInactive).OfType<IDataPersistence>();

        return new List<IDataPersistence>(foundedDataPersistenceObjects);
    }

    private IEnumerator GameSaveCoroutine()
    {
        while (true)
        {
            SaveGame();

            yield return new WaitForSeconds(gameSaveFrequency);
        }
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion
}