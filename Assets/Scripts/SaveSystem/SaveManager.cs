using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName = "pfg.json";
    [SerializeField] private bool useEncryption = false;

    private GameData gameData;
    private FileDataHandler dataHandler;
    private string selectedProfileId;


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
    }

    public void LoadGame()
    {
        gameData = dataHandler.LoadData(selectedProfileId);

        if (gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        foreach (var saveable in FindISaveables())
            saveable.LoadData(gameData);
    }

    public void SaveGame()
    {
        if (gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        foreach (var saveable in FindISaveables())
            saveable.SaveData(ref gameData);

        dataHandler.SavaData(gameData, selectedProfileId);

    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveable> FindISaveables()
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
