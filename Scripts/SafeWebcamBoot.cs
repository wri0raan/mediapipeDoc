using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class SafeWebcamBoot : MonoBehaviour
{
    public RawImage preview;

    WebCamTexture tex;

    IEnumerator Start()
    {

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.LogError("[Cam] Permission denied");
            yield break;
        }

        var devs = WebCamTexture.devices;
        Debug.Log("[Cam] Devices=" + devs.Length);
        if (devs.Length == 0) yield break;

        var d = devs.OrderBy(v => v.isFrontFacing ? 1 : 0).First();

        tex = new WebCamTexture(d.name, 1280, 720, 30);
        preview.texture = tex;

        tex.Play();

        float t = 0f;
        while (t < 3f && tex.width <= 16) { t += Time.unscaledDeltaTime; yield return null; }
        Debug.Log($"[Cam] Started {d.name} => {tex.width}x{tex.height}");
        if (tex.width <= 16) Debug.LogError("[Cam] width<=16 init fail");
    }

    void OnDestroy()
    {
        if (tex != null && tex.isPlaying) tex.Stop();
    }
}
