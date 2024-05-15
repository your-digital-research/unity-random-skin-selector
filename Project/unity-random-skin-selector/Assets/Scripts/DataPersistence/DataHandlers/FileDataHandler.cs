using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    #region PRIVATE_VARIABLES

    private readonly bool useEncryption;
    private readonly string dataDirPath;
    private readonly string dataFileName;

    private const string encryptionCodeWord = Constants.EncryptionCodeWord;

    #endregion

    #region PUBLIC_FUNCTIONS

    public FileDataHandler(string newDataDirPath, string newDataFileName, bool newUseEncryption)
    {
        dataDirPath = newDataDirPath;
        dataFileName = newDataFileName;
        useEncryption = newUseEncryption;
    }

    public GameData Load()
    {
        // Use Path.Combine() to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;

        if (!File.Exists(fullPath)) return loadedData;

        try
        {
            // Load serialized data from the file
            string dataToLoad = "";

            using (FileStream stream = new FileStream(fullPath, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    dataToLoad = reader.ReadToEnd();
                }
            }

            // Optionally decrypt game data
            if (useEncryption) dataToLoad = EncryptDecrypt(dataToLoad);

            // Deserialize the data from JSON back into C# object
            loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred when trying to load data from file : " + fullPath + "\n" + e);
        }

        return loadedData;
    }

    public void Save(GameData gameData)
    {
        // Use Path.Combine() to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            // Create the directory, the file will be written to if it doesn't already exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException());

            // Serialize the C# game data into JSON
            string dataToStore = JsonUtility.ToJson(gameData, true);

            // Optionally encrypt game data
            if (useEncryption) dataToStore = EncryptDecrypt(dataToStore);

            // Write the serialized data to the fle
            using FileStream stream = new FileStream(fullPath, FileMode.Create);
            using StreamWriter writer = new StreamWriter(stream);

            writer.Write(dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error occurred when trying to save data to file : " + fullPath + "\n" + e);
        }
    }

    public void Reset()
    {
        // Use Path.Combine() to account for different OS's having different path separators
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if (!File.Exists(fullPath)) return;

        try
        {
            File.Delete(fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Something went wrong when deleting data : " + fullPath + "\n" + e);
        }
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    // Simple implementation of XOR encryption
    private string EncryptDecrypt(string gameData)
    {
        string modifiedGameData = "";

        for (int i = 0; i < gameData.Length; i++)
        {
            modifiedGameData += (char)(gameData[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }

        return modifiedGameData;
    }

    #endregion
}