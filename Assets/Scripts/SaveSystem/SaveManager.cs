using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName = "pfg.json";
    [SerializeField] private bool useEncryption = false;

    private GameData gameData;
    private FileDataHandler dataHandler;

    [SerializeField] private string selectedProfileId;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        selectedProfileId = newProfileId;
        gameData = dataHandler.LoadData(selectedProfileId);
    }

    public void NewGame()
    {
        gameData = new GameData();
        dataHandler.SavaData(gameData, selectedProfileId);
    }

    public void LoadGame()
    {
        gameData = dataHandler.LoadData(selectedProfileId);

        if (gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        SyncMemoryToScene();
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        UpdateSceneToMemory();
        dataHandler.SavaData(gameData, selectedProfileId);
    }

    // 메모리 데이터를 씬의 객체들에게 주입 (씬 이동 시 사용)
    public void SyncMemoryToScene()
    {
        foreach (var saveable in FindAllSaveables())
        {
            saveable.LoadData(gameData);
        }
    }

    // 씬의 객체 데이터를 메모리에만 업데이트 (씬 이동 직전 사용)
    public void UpdateSceneToMemory()
    {
        if (gameData == null)
            return;

        foreach (var saveable in FindAllSaveables())
        {
            saveable.SaveData(ref gameData);
        }
    }

    private List<ISaveable> FindAllSaveables()
    {
        return
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToList();
    }
    public bool HasGameData()
    {
        return gameData != null;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    internal GameData GetSavedGameData()
    {
        return gameData;
    }
}
