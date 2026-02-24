using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFileName;

    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "secret";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public void SavaData(GameData gameData, string profileId)
    {
        string fullpath = Path.Combine(dataDirPath, profileId, dataFileName);

        try
        {
            // 1. 디렉토리 생성
            Directory.CreateDirectory(Path.GetDirectoryName(fullpath));

            // 2. gameData -> json으로 전환
            string dataToSave = JsonUtility.ToJson(gameData, true);

            // 암호화
            if (useEncryption)
            {
                dataToSave = EncryptDecrypt(dataToSave);
            }

            // 3. 새 파일 열기 (생성)
            using (FileStream stream = new FileStream(fullpath, FileMode.Create))
            {
                // 4. json을 파일에 작성
                using (StreamWriter write = new StreamWriter(stream))
                {
                    write.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error on trying to save data to file: " + fullpath + "\n" + e);
        }
    }

    public GameData LoadData(string profileId)
    {
        string fullpath = Path.Combine(dataDirPath, profileId, dataFileName);

        GameData loadData = null;

        // 1. 저장된 파일이 있는지 확인
        if (File.Exists(fullpath))
        {
            try
            {
                string dataToLoad = "";

                // 2. 파일 열기
                using (FileStream stream = new FileStream(fullpath, FileMode.Open))
                {
                    // 3. 파일 내용 읽기
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // 복호화
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // 4. json -> gameData로 전환
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error on trying to load data from file: " + fullpath + "\n" + e);
            }
        }

        return loadData;
    }

    public Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach(DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullpath = Path.Combine(dataDirPath, profileId, dataFileName);
            if (!File.Exists(fullpath))
                continue;
            
            GameData profileData = LoadData(profileId);

            if (profileData != null)
                profileDictionary.Add(profileId, profileData);
        }

        return profileDictionary;
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
}

