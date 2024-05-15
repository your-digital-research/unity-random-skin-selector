using System.Collections;
using UnityEngine;
using TMPro;

public class DebugManager : MonoBehaviour
{
    #region SINGLETON

    private static DebugManager instance;

    public static DebugManager Instance
    {
        get
        {
            if (instance == null) Debug.LogError("\"DebugManager\" NOT INITIALIZED");
            return instance;
        }

        private set => instance = value;
    }

    #endregion

    #region PRIVATE_VARIABLES

    private Coroutine fpsMeasurementCoroutineC;

    #endregion

    #region SERIALIZED_VARIABLES

    [Header("References")]
    [SerializeField] private DebugView debugView;
    [SerializeField] private TextMeshProUGUI fpsCounter;

    #endregion

    #region PUBLIC_FUNCTIONS

    public void Init()
    {
        StartFPSMeasurement();
    }

    public void OpenDebugView()
    {
        debugView.Open();
    }

    public void CloseDebugView()
    {
        debugView.Close();
    }

    #endregion

    #region PRIVATE_FUNCTIONS

    private void StartFPSMeasurement()
    {
        if (fpsMeasurementCoroutineC != null) StopCoroutine(fpsMeasurementCoroutineC);
        fpsMeasurementCoroutineC = StartCoroutine(FPSMeasurementCoroutine());
    }

    private void StopFPSMeasurement()
    {
        if (fpsMeasurementCoroutineC != null) StopCoroutine(fpsMeasurementCoroutineC);
        fpsMeasurementCoroutineC = null;
    }

    private IEnumerator FPSMeasurementCoroutine()
    {
        int frameCounter = 0;
        float timeCounter = 0.0f;

        const float refreshTime = 0.5f;

        while (true)
        {
            if (timeCounter < refreshTime)
            {
                timeCounter += Time.deltaTime;
                frameCounter++;
            }
            else
            {
                // This code will break if you set your refreshTime to 0, which makes no sense.
                float lastFramerate = (float)(frameCounter / timeCounter);

                frameCounter = 0;
                timeCounter = 0.0f;

                fpsCounter.text = $"FPS : {(int)lastFramerate}";
            }

            yield return null;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    #endregion
}