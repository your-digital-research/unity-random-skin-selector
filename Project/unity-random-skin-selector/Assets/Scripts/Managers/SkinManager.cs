using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    #region SINGLETON

    private static SkinManager instance;

    public static SkinManager Instance
    {
        get
        {
            if (instance == null) Debug.LogError("\"SkinManager\" NOT INITIALIZED");
            return instance;
        }

        private set => instance = value;
    }

    #endregion

    #region SERIALIZED_VARIABLES

    [Header("Controllers")]
    [SerializeField] private SkinController skinController;

    [Header("References")]
    [SerializeField] private SkinView skinViewPrefab;
    [SerializeField] private Transform skinGridContainer;

    [Header("Collections")]
    [SerializeField] private List<SkinView> skinViews;

    #endregion

    #region PUBLIC_FUNCTIONS

    public void Init()
    {
        InitSkinViews();
        InitSkinController();
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    private void OnSkinDestroy(int id, SkinView skinView)
    {
        RemoveListeners(skinView);
        skinViews.Remove(skinView);
    }

    private void InitSkinViews()
    {
        int skinsCount = skinController.Skins.Count;

        for (int i = 0; i < skinsCount; i++)
        {
            SkinModel skinModel = GetSkinModelForIndex(i);
            if (skinModel != null) CreateSkinView(skinModel);
        }
    }

    private void InitSkinController()
    {
        skinController.Init(skinViews);
    }

    private SkinModel GetSkinModelForIndex(int index)
    {
        return index >= 0 && index < skinController.Skins.Count ? skinController.Skins[index] : null;
    }

    private void CreateSkinView(SkinModel skinModel)
    {
        SkinView skinView = Instantiate(skinViewPrefab, skinGridContainer);
        skinView.Init(skinModel);

        AddListeners(skinView);
        skinViews.Add(skinView);
    }

    private void AddListeners(SkinView skinView)
    {
        skinView.OnSkinDestroy += OnSkinDestroy;
    }

    private void RemoveListeners(SkinView skinView)
    {
        skinView.OnSkinDestroy -= OnSkinDestroy;
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion
}