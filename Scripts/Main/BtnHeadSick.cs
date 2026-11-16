using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BtnHeadSick : MonoBehaviour
{
    [Header("Load")]
    public string sceneName = "CheakHead";

    [Header("SFX")]
    public AudioClip clickSfx;
    public float extraDelay = 0.02f;

    Button btn;
    AudioSource src;

    void Awake()
    {
        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveListener(OnClick);
            btn.onClick.AddListener(OnClick);
        }

        src = GetComponent<AudioSource>();
        if (!src) src = gameObject.AddComponent<AudioSource>();
        src.playOnAwake = false;
        src.spatialBlend = 0f;
        src.ignoreListenerPause = true;
    }

    public void OnClick()
    {
        StartCoroutine(PlaySfxThenLoad());
    }

    IEnumerator PlaySfxThenLoad()
    {
        if (clickSfx)
        {
            src.PlayOneShot(clickSfx);
            yield return new WaitForSecondsRealtime(clickSfx.length + extraDelay);
        }
        SceneManager.LoadScene(sceneName);
    }
}
