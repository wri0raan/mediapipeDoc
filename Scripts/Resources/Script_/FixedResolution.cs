using UnityEngine;

public class FixedResolution : MonoBehaviour
{
    void Start()
    {

        Debug.Log($"[Before] Screen: {Screen.width} x {Screen.height}");

        int targetW = 1080;
        int targetH = 1920;

        int screenW = Display.main.systemWidth;
        int screenH = Display.main.systemHeight;

        if (screenH < targetH)
        {
            float aspect = (float)targetW / targetH;
            targetH = screenH - 80;
            targetW = Mathf.RoundToInt(targetH * aspect);
        }

        Screen.SetResolution(targetW, targetH, false);

        Debug.Log($"[After] Screen: {Screen.width} x {Screen.height}");
    }
}
