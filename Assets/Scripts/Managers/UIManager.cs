using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region SINGLETON

    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null) Debug.LogError("\"UIManager\" NOT INITIALIZED");
            return instance;
        }

        private set => instance = value;
    }

    #endregion

    #region SERIALIZED_VARIABLES

    [Header("Views")]
    [SerializeField] private GameView gameView;

    #endregion

    #region PUBLIC_FUNCTIONS

    public void ShowGameView()
    {
        gameView.Show();
    }

    public void HideGameView()
    {
        gameView.Hide();
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    private void Awake()
    {
        instance = this;
    }

    #endregion
}