using System.Collections;
using UnityEngine;

public class ChooseIntroController : MonoBehaviour
{
    [Header("족자 BG (bg_pop)")]
    public RectTransform bgPop;
    public float scrollOpenDuration = 0.6f;

    [Header("Fade 대상")]
    public CanvasGroup imgSChGroup;
    public CanvasGroup imgSDiaGroup;
    public RectTransform imgSDiaRect;
    public CanvasGroup layoutVerGroup;

    [Header("타이밍 (초)")]
    public float fadeDuration = 0.5f;
    public float delayDia = 1.0f;
    public float delayLayout = 0.3f;

    [Header("img_s_dia 뿅 등장(스케일)")]
    public float diaPopDuration = 0.35f;
    public float diaStartScale = 0.05f;
    public float diaOvershoot = 1.05f;
    public float diaOvershootBackTime = 0.08f;

    Vector2 _bgEndSize;

    private void OnEnable()
    {
        StopAllCoroutines();

        if (bgPop != null)
        {
            _bgEndSize = bgPop.sizeDelta;
            bgPop.sizeDelta = new Vector2(_bgEndSize.x, 0f);
        }

        if (imgSDiaRect == null && imgSDiaGroup != null)
            imgSDiaRect = imgSDiaGroup.GetComponent<RectTransform>();

        InitHidden(imgSChGroup);
        InitHidden(imgSDiaGroup);
        InitHidden(layoutVerGroup);

        if (imgSDiaRect != null)
            imgSDiaRect.localScale = Vector3.one * diaStartScale;

        StartCoroutine(IntroSequence());
    }

    IEnumerator IntroSequence()
    {

        if (bgPop != null)
            yield return StartCoroutine(ScrollOpenBySizeDelta(bgPop, _bgEndSize, scrollOpenDuration));

        if (imgSChGroup != null)
            yield return StartCoroutine(FadeIn(imgSChGroup, fadeDuration, true));

        yield return new WaitForSeconds(delayDia);

        if (imgSDiaGroup != null && imgSDiaRect != null)
            yield return StartCoroutine(DiaPopIn(imgSDiaGroup, imgSDiaRect, diaPopDuration, diaStartScale));

        yield return new WaitForSeconds(delayLayout);

        if (layoutVerGroup != null)
            yield return StartCoroutine(FadeIn(layoutVerGroup, fadeDuration, true));
    }

    void InitHidden(CanvasGroup group)
    {
        if (group == null) return;

        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    IEnumerator ScrollOpenBySizeDelta(RectTransform target, Vector2 endSize, float duration)
    {
        float t = 0f;

        Vector2 startSize = new Vector2(endSize.x, 0f);
        target.sizeDelta = startSize;

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);
            target.sizeDelta = Vector2.Lerp(startSize, endSize, n);
            yield return null;
        }

        target.sizeDelta = endSize;
    }

    IEnumerator FadeIn(CanvasGroup group, float duration, bool enableRaycastOnEnd)
    {
        if (group == null) yield break;

        float t = 0f;

        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;

        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);
            group.alpha = n;
            yield return null;
        }

        group.alpha = 1f;

        if (enableRaycastOnEnd)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
        }
    }

    IEnumerator DiaPopIn(CanvasGroup group, RectTransform rect, float duration, float startScale)
    {
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
