using UnityEngine;
using UnityEngine.Video;

public class VideoStopper : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
    }

    public void StopVideo()
    {
        if (videoPlayer == null) return;

        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
    }
}
