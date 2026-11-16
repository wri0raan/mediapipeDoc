using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class BootResolution : MonoBehaviour
{
    [Header("Target (Portrait 9:16)")]
    public int width = 720 ;
    public int height = 1280;
    public FullScreenMode mode = FullScreenMode.Windowed;

    private void Start()
    {

        StartCoroutine(ApplyResNextFrame());
    }

    private System.Collections.IEnumerator ApplyResNextFrame()
    {
        Debug.Log($"[BootResolution] wait & apply {width}x{height} {mode}");
        yield return null;

        Screen.SetResolution(width, height, false);
        Screen.fullScreenMode = mode;
        Application.targetFrameRate = 60;
        Debug.Log("[BootResolution] applied");
    }

}
