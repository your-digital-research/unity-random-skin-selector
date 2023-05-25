using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinController : MonoBehaviour, IDataPersistence
{
    #region SERIALIZED_VARIABLES

    [Header("Collections")]
    [SerializeField] private List<SkinModel> skinModels;

    #endregion

    #region PRIVATE_VARIABLES

    private SerializableDictionary<int, SkinData> skinsData;

    #endregion

    #region PROPERTIES

    public List<SkinModel> Skins => skinModels;

    #endregion

    #region PUBLIC_FUNCTIONS

    public void LoadData(GameData gameData)
    {
        LoadSkinsData(gameData);
    }

    public void SaveData(GameData gameData)
    {
        SaveSkinsData(gameData);
    }

    public bool IsLockedSkinExist()
    {
        return skinModels.Find(skin => skin.State == SkinState.Locked);
    }

    public void Init(List<SkinView> skinViews)
    {
        foreach (SkinView skinView in skinViews)
        {
            AddListeners(skinView);
        }
    }

    public void UnlockRandomSkin()
    {
        List<SkinModel> lockedSkins = skinModels.FindAll(skin => skin.State == SkinState.Locked);

        if (lockedSkins.Count > 0)
        {
            int randomIndex = Random.Range(0, lockedSkins.Count);
            lockedSkins[randomIndex].SetState(SkinState.Unlocked);

            DataPersistenceManager.Instance.SaveGame();
        }
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    private void LoadSkinsData(GameData gameData)
    {
        // Load the saved skins data
        skinsData = gameData.skinData;

        // Loop through data
        for (int index = 0; index < skinModels.Count; index += 1)
        {
            SkinData skinData;
            SkinModel skinModel = skinModels[index];

            // Via ID check if the data contains the data of certain skin
            // If so load the data
            if (skinsData.Keys.Contains(skinModel.Id))
            {
                skinData = skinsData[skinModel.Id];
            }
            // If not change the model state from Unknown to Locked
            // then create new SkinData and pass the model
            else
            {
                skinModel.State = SkinState.Locked;
                skinData = new SkinData(skinModel);
            }

            // At the end assign the data to the model
            skinModel.AssignData(skinData);
        }
    }

    private void SaveSkinsData(GameData gameData)
    {
        // Loop through models
        foreach (SkinModel skinModel in skinModels)
        {
            // Create SkinModel instance for saving data
            SkinModel model = ScriptableObject.CreateInstance<SkinModel>();

            // Assign current model that to the instance model
            model.AssignData(skinModel);

            // Via ID check if data contains the model's data
            // If so remove it so we can add the updated data;
            if (gameData.skinData.ContainsKey(model.Id))
            {
                gameData.skinData.Remove(model.Id);
            }

            // Check if skin selected, if so save state as Unlocked
            if (model.State == SkinState.Selected)
            {
                model.State = SkinState.Unlocked;
            }

            // Create new data and pass the model
            SkinData skinData = new SkinData(model);

            // Add the new data
            gameData.skinData.Add(model.Id, skinData);
        }
    }

    private void OnSkinClick(int skinId, SkinView skinView)
    {
        DeselectSelectedSkin();
        SelectSkin(skinId);
    }

    private void OnSkinDestroy(int skinId, SkinView skinView)
    {
        RemoveListeners(skinView);
    }

    private void DeselectSelectedSkin()
    {
        SkinModel selectedSkin = skinModels.Find(skin => skin.State == SkinState.Selected);
        if (selectedSkin) selectedSkin.SetState(SkinState.Unlocked);
    }

    private void SelectSkin(int skinId)
    {
        SkinModel skin = skinModels.Find(skin => skin.Id == skinId);
        if (skin) skin.SetState(SkinState.Selected);
    }

    private void AddListeners(SkinView skinView)
    {
        skinView.OnSkinClick += OnSkinClick;
        skinView.OnSkinDestroy += OnSkinDestroy;
    }

    private void RemoveListeners(SkinView skinView)
    {
        skinView.OnSkinClick -= OnSkinClick;
        skinView.OnSkinDestroy -= OnSkinDestroy;
    }

    #endregion
}