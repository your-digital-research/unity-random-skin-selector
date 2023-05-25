using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Skin", menuName = "Skin", order = 0)]
public class SkinModel : ScriptableObject
{
    #region SERIALIZED_VARIABLES

    [Header("Properties")]
    [SerializeField] private int id;
    [SerializeField] private string skinName;
    [SerializeField] private SkinState state;

    #endregion

    #region EVENTS

    public UnityAction<SkinState, SkinState> OnStateChanged;

    #endregion

    #region PROPERTIES

    public int Id
    {
        get => id;
        set => id = value;
    }

    public string SkinName
    {
        get => skinName;
        set => skinName = value;
    }

    public SkinState State
    {
        get => state;
        set => state = value;
    }

    #endregion

    #region CONSTRUCTORS

    public SkinModel()
    {
        id = -1;
        skinName = "Unknown";
        state = SkinState.Unknown;
    }

    public SkinModel(int newId, string newSkinSkinName)
    {
        id = newId;
        skinName = newSkinSkinName;
        state = SkinState.Unknown;
    }

    #endregion

    #region PUBLIC_FUNCTIONS

    public void AssignData(SkinModel skinModel)
    {
        id = skinModel.Id;
        skinName = skinModel.SkinName;
        state = skinModel.State;
    }

    public void AssignData(SkinData skinData)
    {
        id = skinData.id;
        skinName = skinData.skinName;
        state = skinData.state;
    }

    public void SetState(SkinState newState)
    {
        SkinState previousState = state;
        state = newState;

        OnStateChanged?.Invoke(previousState, state);
    }

    #endregion
}