using UnityEngine;
using UnityEngine.Events;

public class BtnBackPanel : MonoBehaviour
{
    [Header("현재 끌 패널")]
    public GameObject currentPanel;

    [Header("다시 켤 이전 패널")]
    public GameObject previousPanel;

    [Header("옵션: 추가 동작 (트래킹 재시작, 비디오 정지 등)")]
    public UnityEvent onBack;

    public void Back()
    {
        if (currentPanel) currentPanel.SetActive(false);

        if (previousPanel)
        {
            previousPanel.SetActive(true);
            previousPanel.transform.SetAsLastSibling();
        }

        onBack?.Invoke();
    }
}
