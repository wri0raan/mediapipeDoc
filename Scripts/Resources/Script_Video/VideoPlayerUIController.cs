using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Video;
using System.Collections;

public class VideoPlayerUIController : MonoBehaviour, IPointerClickHandler
{
    [Header("Video Components")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage videoRawImage;

    [Header("UI References")]
    [SerializeField] private GameObject thumbnailObject;
    [SerializeField] private Button initialPlayButton;
    [SerializeField] private Button overlayPauseButton;
    [SerializeField] private Button overlayPlayButton;

    [Header("Overlay CanvasGroups")]
    [SerializeField] private CanvasGroup overlayPauseGroup;
    [SerializeField] private CanvasGroup overlayPlayGroup;

    [Header("Timing")]
    [SerializeField] private float overlayVisibleTime = 2f;
    [SerializeField] private float overlayFadeDuration = 0.3f;

    private bool hasStarted = false;
    private Coroutine currentOverlayRoutine = null;

    private void Awake()
    {

        ResetUI();
    }

    private void ResetUI()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.time = 0;

            videoPlayer.Prepare();
        }

        if (thumbnailObject != null)
            thumbnailObject.SetActive(true);

        if (initialPlayButton != null)
            initialPlayButton.gameObject.SetActive(true);

        HideOverlayInstant(overlayPauseGroup);
        HideOverlayInstant(overlayPlayGroup);

        hasStarted = false;
    }

    private void OnEnable()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnDisable()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= OnVideoFinished;
    }

    public void OnClickInitialPlay()
    {
        if (videoPlayer == null || videoRawImage == null)
            return;

        if (thumbnailObject != null)
            thumbnailObject.SetActive(false);

        videoRawImage.enabled = true;

        if (initialPlayButton != null)
            initialPlayButton.gameObject.SetActive(false);

        videoPlayer.time = 0;
        videoPlayer.Play();

        hasStarted = true;

        ShowOverlay(overlayPauseGroup);
    }

    public void OnClickPause()
    {
        PauseVideo();
    }

    public void OnClickResume()
    {
        ResumeVideo();
    }

    private void PauseVideo()
    {
        if (videoPlayer == null) return;

        videoPlayer.Pause();

        HideOverlayInstant(overlayPauseGroup);
        ShowOverlay(overlayPlayGroup);
    }

    private void ResumeVideo()
    {
        if (videoPlayer == null || videoRawImage == null)
            return;

        if (thumbnailObject != null)
            thumbnailObject.SetActive(false);

        videoRawImage.enabled = true;

        if (videoPlayer.length > 0.0 && videoPlayer.time >= videoPlayer.length - 0.05f)
        {
            videoPlayer.time = 0;
        }

        videoPlayer.Play();

        HideOverlayInstant(overlayPlayGroup);
        ShowOverlay(overlayPauseGroup);
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (!hasStarted || videoPlayer == null)
            return;

        if (videoPlayer.isPlaying)
        {
            PauseVideo();
        }
        else
        {
            ResumeVideo();
        }
    }

    public void StopAndReset()
    {
        StopAllCoroutines();
        ResetUI();
    }

    private void OnVideoFinished(VideoPlayer source)
    {

        if (thumbnailObject != null)
            thumbnailObject.SetActive(true);

        HideOverlayInstant(overlayPauseGroup);

        if (overlayPlayGroup != null)
        {
            overlayPlayGroup.gameObject.SetActive(true);
            overlayPlayGroup.alpha = 1f;
        }

    }

    private void ShowOverlay(CanvasGroup targetGroup)
    {
        if (targetGroup == null) return;

        if (currentOverlayRoutine != null)
            StopCoroutine(currentOverlayRoutine);

        currentOverlayRoutine = StartCoroutine(OverlayRoutine(targetGroup));
    }

    private void HideOverlayInstant(CanvasGroup targetGroup)
    {
        if (targetGroup == null) return;

        targetGroup.alpha = 0f;
        targetGroup.gameObject.SetActive(false);
    }

    private IEnumerator OverlayRoutine(CanvasGroup targetGroup)
    {

        if (targetGroup == overlayPauseGroup && overlayPlayGroup != null)
            HideOverlayInstant(overlayPlayGroup);
        if (targetGroup == overlayPlayGroup && overlayPauseGroup != null)
            HideOverlayInstant(overlayPauseGroup);

        targetGroup.gameObject.SetActive(true);
        targetGroup.alpha = 1f;

        yield return new WaitForSeconds(overlayVisibleTime);

        float t = 0f;
        float startAlpha = targetGroup.alpha;

        while (t < overlayFadeDuration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / overlayFadeDuration);
            targetGroup.alpha = Mathf.Lerp(startAlpha, 0f, lerp);
            yield return null;
        }

        targetGroup.alpha = 0f;
        targetGroup.gameObject.SetActive(false);

        currentOverlayRoutine = null;
    }
}
