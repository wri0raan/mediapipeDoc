using System.Collections;
using UnityEngine;

public class VideoIntroFadeController : MonoBehaviour
{
    [Header("위쪽 이미지 2개")]
    public CanvasGroup topBubble;
    public RectTransform topBubbleRect;
    public CanvasGroup topPerson;

    [Header("아래 묶음 하나 (QR + 이름 + 글 등)")]
    public CanvasGroup bottomGroup;

    [Header("아래 버튼 UI (마지막에 등장)")]
    public CanvasGroup bottomButtons;

    [Header("타이밍 설정 (초)")]
    public float initialDelay = 0.3f;

    [Tooltip("topPerson 등장 후 말풍선 나오기까지 대기")]
    public float delayBeforeBubble = 0.25f;

    [Tooltip("말풍선 등장 후 아래 묶음 나오기까지 대기")]
    public float delayBeforeBottom = 0.5f;

    [Tooltip("아래 묶음 등장 후 버튼 나오기까지 대기")]
    public float delayBeforeButtons = 0.3f;

    [Header("페이드 설정")]
    public float fadeDuration = 0.5f;
    public bool playOnEnable = true;

    [Header("말풍선 뿅 등장(스케일)")]
    public float bubblePopDuration = 0.35f;
    public float bubbleStartScale = 0.05f;
    public float bubbleOvershoot = 1.05f;
    public float bubbleOvershootBackTime = 0.08f;

    Coroutine routine;

    void OnEnable()
    {
        if (playOnEnable)
            Play();
    }

    public void Play()
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {

        if (topBubbleRect == null && topBubble != null)
            topBubbleRect = topBubble.GetComponent<RectTransform>();

        InitHidden(topPerson);
        InitHidden(topBubble);
        InitHidden(bottomGroup);
        InitHidden(bottomButtons);

        if (topBubbleRect != null)
            topBubbleRect.localScale = Vector3.one * bubbleStartScale;

        if (initialDelay > 0f)
            yield return new WaitForSeconds(initialDelay);

        if (topPerson != null)
            yield return StartCoroutine(FadeIn(topPerson, fadeDuration, true));

        if (delayBeforeBubble > 0f)
            yield return new WaitForSeconds(delayBeforeBubble);

        if (topBubble != null && topBubbleRect != null)
            yield return StartCoroutine(BubblePopIn(topBubble, topBubbleRect, bubblePopDuration, bubbleStartScale));

        if (delayBeforeBottom > 0f)
            yield return new WaitForSeconds(delayBeforeBottom);

        if (bottomGroup != null)
            yield return StartCoroutine(FadeIn(bottomGroup, fadeDuration, true));

        if (delayBeforeButtons > 0f)
            yield return new WaitForSeconds(delayBeforeButtons);

        if (bottomButtons != null)
            yield return StartCoroutine(FadeIn(bottomButtons, fadeDuration, true));

        routine = null;
    }

    void InitHidden(CanvasGroup group)
    {
        if (group == null) return;

        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
        group.gameObject.SetActive(true);
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

    IEnumerator BubblePopIn(CanvasGroup group, RectTransform rect, float duration, float startScale)
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
