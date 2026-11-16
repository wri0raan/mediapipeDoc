using System.Collections;
using UnityEngine;

public class ScrollOpenUI : MonoBehaviour
{
    public enum ScrollDirection { Horizontal, Vertical }

    [Header("펼칠 대상")]
    public RectTransform target;

    [Header("설정")]
    public ScrollDirection direction = ScrollDirection.Horizontal;
    public float duration = 0.6f;

    [Tooltip("OnEnable 이후 애니메이션 시작까지 기다릴 시간(초)")]
    public float delayBeforePlay = 0f;

    public bool playOnEnable = true;

    [Tooltip("true면 sizeDelta로 펼쳐서 9-slice 고정영역 유지, false면 기존 스케일 방식")]
    public bool useSizeDelta = true;

    Coroutine _playing;

    void Reset()
    {
        target = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        if (playOnEnable)
            Play();
    }

    public void Play()
    {
        if (_playing != null)
            StopCoroutine(_playing);

        _playing = StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        if (target == null) yield break;

        if (useSizeDelta)
        {
            Vector2 endSize = target.sizeDelta;
            Vector2 startSize = endSize;

            if (direction == ScrollDirection.Horizontal)
                startSize.x = 0f;
            else
                startSize.y = 0f;

            target.sizeDelta = startSize;

            if (delayBeforePlay > 0f)
                yield return new WaitForSeconds(delayBeforePlay);

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float n = Mathf.Clamp01(t / duration);
                target.sizeDelta = Vector2.Lerp(startSize, endSize, n);
                yield return null;
            }

            target.sizeDelta = endSize;
        }
        else
        {

            Vector3 end = target.localScale;
            Vector3 start = end;

            if (direction == ScrollDirection.Horizontal)
                start.x = 0f;
            else
                start.y = 0f;

            target.localScale = start;

            if (delayBeforePlay > 0f)
                yield return new WaitForSeconds(delayBeforePlay);

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float n = Mathf.Clamp01(t / duration);
                target.localScale = Vector3.Lerp(start, end, n);
                yield return null;
            }

            target.localScale = end;
        }

        _playing = null;
    }
}
