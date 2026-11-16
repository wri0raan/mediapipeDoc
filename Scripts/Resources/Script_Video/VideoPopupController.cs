using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPopupController : MonoBehaviour
{
    public GameObject popupRoot;
    public VideoPlayer videoPlayer;
    public GameObject startButtonImage;

    void Awake()
    {
        if (popupRoot) popupRoot.SetActive(false);
    }

    public void OpenAndPlay()
    {
        if (popupRoot) popupRoot.SetActive(true);

        if (startButtonImage)
            startButtonImage.SetActive(false);

        if (videoPlayer)
        {
            videoPlayer.Stop();
            videoPlayer.Play();
        }
    }

    public void StopAndClose()
    {
        if (videoPlayer)
        {
            videoPlayer.Stop();
        }

        if (popupRoot) popupRoot.SetActive(false);

    }
}
