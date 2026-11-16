using UnityEngine;

public class LogCamsNow : MonoBehaviour
{
    void Start()
    {
        var devs = WebCamTexture.devices;
        Debug.Log($"[Cams] count={devs.Length}");
        for (int i = 0; i < devs.Length; i++)
            Debug.Log($"[Cams] {i}: {devs[i].name}, front={devs[i].isFrontFacing}");
    }
}
