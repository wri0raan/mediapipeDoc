using UnityEngine;

public class ForceResolution : MonoBehaviour
{
    [Header("기준 비율")]
    public int aspectWidth = 1080;
    public int aspectHeight = 1920;

    [Header("모니터 높이 기준으로 창 크기 결정")]
    public bool useMonitorHeight = true;

    [Range(0.1f, 1f)]
    public float heightScale = 1f;

    public bool centerWindow = true;

    void Awake()
    {
#if !UNITY_EDITOR
        ApplyResolution();
#endif
    }

    void ApplyResolution()
    {
        int targetW, targetH;

        float aspect = (float)aspectWidth / aspectHeight;

        int monitorW = Display.main.systemWidth;
        int monitorH = Display.main.systemHeight;

        if (useMonitorHeight)
        {
            targetH = Mathf.RoundToInt(monitorH * heightScale);
            targetW = Mathf.RoundToInt(targetH * aspect);

            if (targetW > monitorW)
            {
                targetW = monitorW;
                targetH = Mathf.RoundToInt(targetW / aspect);
            }
        }
        else
        {
            targetW = aspectWidth;
            targetH = aspectHeight;
        }

        Screen.SetResolution(targetW, targetH, false);

        var window = GetComponent<ForceResolutionWindow>();
        if (window == null)
            window = gameObject.AddComponent<ForceResolutionWindow>();

        window.targetWidth = targetW;
        window.targetHeight = targetH;
        window.centerWindow = centerWindow;
    }
}
