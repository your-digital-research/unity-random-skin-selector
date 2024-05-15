using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region SINGLETON

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null) Debug.LogError("\"GameManager\" NOT INITIALIZED");
            return instance;
        }

        private set => instance = value;
    }

    #endregion

    #region PUBLIC_FUNCTIONS

    public void ResetGame()
    {
        Debug.Log("ResetGame");

        DataPersistenceManager.Instance.ResetData();
    }

    public void ReloadGame()
    {
        Debug.Log("ReloadGame");

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    private void Init()
    {
        Debug.Log("InitGame");

        DOTween.Init();
        DOTween.KillAll();
        DebugManager.Instance.Init();
        Application.targetFrameRate = Constants.TargetFramerate;

        DataPersistenceManager.Instance.Init();
        SkinManager.Instance.Init();
    }

    private void StartGame()
    {
        Debug.Log("StartGame");

        UIManager.Instance.ShowGameView();
    }

    private void Start()
    {
        Init();
        StartGame();
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion
}