[System.Serializable]
public class GameData
{
    #region PUBLIC_VARIABLES

    // Define variables to save
    public SerializableDictionary<int, SkinData> skinData;

    #endregion

    #region PUBLIC_FUNCTIONS

    // The values defined in this constructor will be default values
    // the game starts with when there's no data to load
    public GameData()
    {
        // Assign default values
        skinData = new SerializableDictionary<int, SkinData>();
    }

    #endregion
}