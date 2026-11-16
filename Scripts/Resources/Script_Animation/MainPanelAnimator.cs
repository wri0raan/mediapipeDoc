using System.Collections;
using UnityEngine;

public class MainPanelAnimator : MonoBehaviour
{
    [Header("Target UI")]
    [SerializeField] private CanvasGroup imgTitleCanvasGroup;
    [SerializeField] private CanvasGroup btnStartCanvasGroup;

    [Header("Timing")]
    [SerializeField] private float titleFadeDuration = 1.0f;
    [SerializeField] private float delayBeforeButton = 1.5f;
    [SerializeField] private float buttonFadeDuration = 0.5f;

    private void Awake()
    {

        if (imgTitleCanvasGroup != null)
        {
            imgTitleCanvasGroup.alpha = 0f;
            imgTitleCanvasGroup.interactable = false;
            imgTitleCanvasGroup.blocksRaycasts = false;
        }

        if (btnStartCanvasGroup != null)
        {
            btnStartCanvasGroup.alpha = 0f;
            btnStartCanvasGroup.interactable = false;
            btnStartCanvasGroup.blocksRaycasts = false;
        }
    }

    private void OnEnable()
    {

        StartCoroutine(PlayMainAnimation());
    }

    private IEnumerator PlayMainAnimation()
    {

        yield return StartCoroutine(FadeCanvasGroup(
            imgTitleCanvasGroup, 0f, 1f, titleFadeDuration));

        imgTitleCanvasGroup.blocksRaycasts = true;

        yield return new WaitForSeconds(delayBeforeButton);

        yield return StartCoroutine(FadeCanvasGroup(
            btnStartCanvasGroup, 0f, 1f, buttonFadeDuration));

        btnStartCanvasGroup.interactable = true;
        btnStartCanvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float from, float to, float duration)
    {
        if (cg == null || duration <= 0f) yield break;

        float t = 0f;
        cg.alpha = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / duration);
            cg.alpha = Mathf.Lerp(from, to, lerp);
            yield return null;
        }

        cg.alpha = to;
    }
}
