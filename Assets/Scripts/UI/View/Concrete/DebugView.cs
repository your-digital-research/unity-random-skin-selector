using UnityEngine;
using UnityEngine.UI;

public class DebugView : AbstractView
{
    #region SERIALIZED_VARIABLES

    [Header("Debug Buttons")]
    [SerializeField] private GameObject buttons;

    [Header("Open/Close Buttons")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button settingsButton;
    #endregion

    #region PUBLIC_FUNCTIONS

    public override void Show(bool force = true, float delay = 0, float duration = 1)
    {
        gameObject.SetActive(true);
    }

    public override void Hide(bool force = true, float delay = 0, float duration = 1)
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        buttons.gameObject.SetActive(true);
        background.gameObject.SetActive(true);
        closeButton.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(false);
    }

    public void Close()
    {
        buttons.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);
        settingsButton.gameObject.SetActive(true);
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    private void Start()
    {
        Close();
    }

    #endregion
}