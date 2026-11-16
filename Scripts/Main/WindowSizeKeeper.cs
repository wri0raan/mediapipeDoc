using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-2000)]
public class WindowSizeKeeper : MonoBehaviour
{
    public int defaultWidth = 720;
    public int defaultHeight = 1280;
    public FullScreenMode defaultMode = FullScreenMode.Windowed;

    const string KEY_W = "win_w";
    const string KEY_H = "win_h";
    const string KEY_FS = "win_fs";

    static WindowSizeKeeper _inst;
    int lastW, lastH;
    bool bootApplied;

    void Awake()
    {
        if (_inst != null) { Destroy(gameObject); return; }
        _inst = this;
        DontDestroyOnLoad(gameObject);

        ApplyOnBoot();
        SceneManager.sceneLoaded += (_, __) => PreventOtherResizers();
    }

    void ApplyOnBoot()
    {
        int w = PlayerPrefs.GetInt(KEY_W, -1);
        int h = PlayerPrefs.GetInt(KEY_H, -1);
        int fs = PlayerPrefs.GetInt(KEY_FS, (int)defaultMode);

        var mode = (FullScreenMode)fs;

        if (w < 0 || h < 0)
        {

            Screen.SetResolution(defaultWidth, defaultHeight, false);
            Screen.fullScreenMode = defaultMode;
        }
        else
        {

            Screen.SetResolution(w, h, mode == FullScreenMode.ExclusiveFullScreen);
            Screen.fullScreenMode = mode;
        }

        lastW = Screen.width;
        lastH = Screen.height;
        bootApplied = true;
        Debug.Log($"[WSK] Applied {Screen.width}x{Screen.height} ({Screen.fullScreenMode})");
    }

    void Update()
    {
        if (!bootApplied) return;

        if (lastW != Screen.width || lastH != Screen.height)
        {
            lastW = Screen.width;
            lastH = Screen.height;
            PlayerPrefs.SetInt(KEY_W, lastW);
            PlayerPrefs.SetInt(KEY_H, lastH);
            PlayerPrefs.SetInt(KEY_FS, (int)Screen.fullScreenMode);
            PlayerPrefs.Save();
            Debug.Log($"[WSK] Saved {lastW}x{lastH} ({Screen.fullScreenMode})");
        }
    }

    void PreventOtherResizers()
    {
        foreach (var br in FindObjectsOfType<BootResolution>(true))
            Destroy(br);
    }
}
