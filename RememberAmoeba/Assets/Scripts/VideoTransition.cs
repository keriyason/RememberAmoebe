using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoTransition : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private string sceneToLoad;

    void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();
    }

    void OnEnable()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
