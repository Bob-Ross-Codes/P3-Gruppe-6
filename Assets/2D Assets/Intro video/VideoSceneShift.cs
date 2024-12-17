/// <summary>
/// Plays a video and switches to the next scene when the video ends.
/// </summary>
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneShift : MonoBehaviour
{
    public VideoPlayer videoPlayer; // VideoPlayer component reference
    public string sceneToLoad; // Scene to load after the video ends

    private void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Assign VideoPlayer if not already assigned
        if (videoPlayer == null)
            videoPlayer = GetComponent<VideoPlayer>();

        // Register the event for when the video ends
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Load the specified scene when the video ends
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnDestroy()
    {
        // Unregister the video end event
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}