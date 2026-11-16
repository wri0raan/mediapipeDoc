using UnityEngine;

public class BtnStart : MonoBehaviour
{
    [Tooltip("시작 화면 진행할 루트")]
    public GameObject StartOverlay;

    public void Open()
    {
        if (!StartOverlay) return;
        StartOverlay.SetActive(true);

        StartOverlay.transform.SetAsLastSibling();
    }
}
