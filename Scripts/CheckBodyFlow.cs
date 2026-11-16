using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class CheckBodyFlow : MonoBehaviour
{
    [Header("UI Groups")]
    public GameObject loadingGroup;
    public GameObject pointGroup;
    public GameObject popupResult;

    [Header("Tracking Visuals")]
    public GameObject trackingVisualRoot;

    [Header("Timing")]
    public float loadingSeconds = 3f;
    public float autoResetAfterSeconds = 120f;
    public float cameraReadyTimeout = 2f;

    [Header("Hooks")]
    public UnityEvent onBeginTracking;
    public UnityEvent onEndTracking;

    [Header("Scene")]
    public string mainSceneName = "Main";

    private bool ended;
    private bool trackingOn;
    private bool cameraReady;
    private Coroutine flowRoutine;

    void Awake()
    {

        if (popupResult) popupResult.SetActive(false);
        if (loadingGroup) loadingGroup.SetActive(true);
        if (pointGroup) pointGroup.SetActive(false);

        ended = false;
        trackingOn = false;
        cameraReady = false;
    }

    void Start()
    {

        trackingOn = true;
        onBeginTracking?.Invoke();

        flowRoutine = StartCoroutine(WaitCameraThenFlow());
    }

    IEnumerator WaitCameraThenFlow()
    {
        float t = 0f;

        while (!cameraReady && t < cameraReadyTimeout)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        if (loadingGroup) loadingGroup.SetActive(true);

        yield return new WaitForSecondsRealtime(loadingSeconds);

        if (loadingGroup) loadingGroup.SetActive(false);
        if (pointGroup) pointGroup.SetActive(true);

        yield return new WaitForSecondsRealtime(autoResetAfterSeconds);
        EndAndReset();
    }

    public void NotifyCameraReady()
    {
        cameraReady = true;
    }

    public void OnTrackingCompleted()
    {
        if (ended) return;

        if (trackingOn)
        {
            trackingOn = false;
            onEndTracking?.Invoke();
        }

        if (trackingVisualRoot) trackingVisualRoot.SetActive(false);

        if (pointGroup) pointGroup.SetActive(false);
        if (popupResult) popupResult.SetActive(true);
    }

    void OnDisable()
    {
        if (flowRoutine != null)
        {
            StopCoroutine(flowRoutine);
            flowRoutine = null;
        }
    }

    public void BackToPointFromResult()
    {
        if (ended) return;

        if (popupResult) popupResult.SetActive(false);

        if (trackingVisualRoot) trackingVisualRoot.SetActive(true);

        if (pointGroup) pointGroup.SetActive(true);

        if (!trackingOn)
        {
            trackingOn = true;
            onBeginTracking?.Invoke();
        }
    }

    private void EndAndReset()
    {
        if (ended) return;
        ended = true;

        if (trackingOn)
        {
            trackingOn = false;
            onEndTracking?.Invoke();
        }

        if (trackingVisualRoot) trackingVisualRoot.SetActive(false);

        if (!string.IsNullOrEmpty(mainSceneName))
        {

        }
    }
}
