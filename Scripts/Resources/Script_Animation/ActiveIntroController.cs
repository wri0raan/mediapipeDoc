using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class ActiveIntroController : MonoBehaviour
{
    [Header("img_ch 영상 재생 + 페이드")]
    public VideoPlayer imgChVideo;
    public CanvasGroup imgChGroup;

    [Header("말풍선 / 버튼 페이드 대상")]
    public CanvasGroup imgDiaGroup;
    public RectTransform imgDiaRect;
    public CanvasGroup btnPicGroup;

    [Header("타이밍 (초)")]
    public float chFadeDuration = 0.5f;
    public float fadeDuration = 0.5f;
    public float delayDia = 1.0f;
    public float delayBtn = 0.3f;

    [Header("말풍선 뿅 등장(스케일)")]
    public float diaPopDuration = 0.35f;
    public float diaStartScale = 0.05f;
    public float diaOvershoot = 1.05f;
    public float diaOvershootBackTime = 0.08f;

    private void OnEnable()
    {

        InitHidden(imgChGroup);
        InitHidden(imgDiaGroup);
        InitHidden(btnPicGroup);

        if (imgDiaRect == null && imgDiaGroup != null)
            imgDiaRect = imgDiaGroup.GetComponent<RectTransform>();

        if (imgDiaRect != null)
            imgDiaRect.localScale = Vector3.one * diaStartScale;

        if (imgChVideo != null)
        {
            imgChVideo.isLooping = true;
            imgChVideo.Play();
        }

        StartCoroutine(IntroSequence());
    }

    private void OnDisable()
    {
        if (imgChVideo != null)
            imgChVideo.Stop();
    }

    private IEnumerator IntroSequence()
    {

        if (imgChGroup != null)
            yield return StartCoroutine(FadeIn(imgChGroup, chFadeDuration));

        yield return new WaitForSeconds(delayDia);

        if (imgDiaGroup != null)
            yield return StartCoroutine(DiaPopIn(imgDiaGroup, imgDiaRect, diaPopDuration, diaStartScale));

        yield return new WaitForSeconds(delayBtn);

        if (btnPicGroup != null)
            yield return StartCoroutine(FadeIn(btnPicGroup, fadeDuration));
    }

    private void InitHidden(CanvasGroup group)
    {
        if (group == null) return;
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    private IEnumerator FadeIn(CanvasGroup group, float duration)
    {
        if (group == null || duration <= 0f) yield break;

        float t = 0f;
        group.alpha = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);
            group.alpha = n;
            yield return null;
        }

        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    private IEnumerator DiaPopIn(CanvasGroup group, RectTransform rect, float duration, float startScale)
    {
        if (group == null || rect == null || duration <= 0f)
            yield break;

        float t = 0f;

        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;

        Vector3 s0 = Vector3.one * startScale;
        Vector3 s1 = Vector3.one;

        rect.localScale = s0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);

            group.alpha = n;

            float ease = 1f - Mathf.Pow(1f - n, 3f);
            rect.localScale = Vector3.LerpUnclamped(s0, s1, ease);

            yield return null;
        }

        rect.localScale = s1;
        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }
}
