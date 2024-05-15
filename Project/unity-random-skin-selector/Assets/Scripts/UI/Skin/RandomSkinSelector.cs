using UnityEngine;
using UnityEngine.UI;

public class RandomSkinSelector : MonoBehaviour
{
    #region SERIALIZED_VARIABLES

    [Header("Controllers")]
    [SerializeField] private SkinController skinController;

    [Header("Buttons")]
    [SerializeField] private Button unlockRandomSkinButton;

    #endregion

    #region PUBLIC_FUNCTIONS

    public void OnUnlockRandomSkinButtonClicked()
    {
        skinController.UnlockRandomSkin();
        UpdateButtonActiveness();
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    private void UpdateButtonActiveness()
    {
        unlockRandomSkinButton.interactable = skinController.IsLockedSkinExist();
    }

    private void Start()
    {
        UpdateButtonActiveness();
    }

    #endregion
}