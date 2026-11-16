using System.Collections;
using UnityEngine;

public class PopPointIntroController : MonoBehaviour
{
    [Header("족자 애니메이션")]
    public ScrollOpenUI scrollOpen;
    public bool forcePlayOnEnable = false;

    [Header("btn_qr 페이드인")]
    public CanvasGroup btnQrGroup;
    public float btnFadeDuration = 0.4f;
    public float extraDelayAfterScroll = 1.0f;

    Coroutine _routine;

    void OnEnable()
    {
        if (_routine != null)
            StopCoroutine(_routine);

        _routine = StartCoroutine(Sequence());
    }

    IEnumerator Sequence()
    {

        InitHidden(btnQrGroup);

        if (scrollOpen != null && forcePlayOnEnable)
            scrollOpen.Play();

        float wait = 0f;
        if (scrollOpen != null)
            wait = scrollOpen.delayBeforePlay + scrollOpen.duration;

        if (wait > 0f)
            yield return new WaitForSecondsRealtime(wait);

        if (extraDelayAfterScroll > 0f)
            yield return new WaitForSecondsRealtime(extraDelayAfterScroll);

        if (btnQrGroup != null)
            yield return StartCoroutine(FadeIn(btnQrGroup, btnFadeDuration, true));

        _routine = null;
    }

    void InitHidden(CanvasGroup group)
    {
        if (group == null) return;
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
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
}
