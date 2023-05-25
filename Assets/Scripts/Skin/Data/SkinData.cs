// Serialized class to store skin data into JSON

[System.Serializable]
public class SkinData
{
    #region PUBLIC_VARIABLES

    public int id;
    public string skinName;
    public SkinState state;

    #endregion

    #region CONSTRUCTORS

    public SkinData()
    {
        id = -1;
        skinName = "Unknown";
        state = SkinState.Unknown;
    }

    public SkinData(SkinModel skinModel)
    {
        id = skinModel.Id;
        skinName = skinModel.SkinName;
        state = skinModel.State;
    }

    #endregion
}