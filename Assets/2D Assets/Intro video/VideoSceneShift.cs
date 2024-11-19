using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneShift : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component
    public string sceneToLoad; // Name of the scene to load after the video ends

    void Start()
    {

           
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        
        // Hide the cursor
        Cursor.visible = false;
    
        // Make sure we have a reference to the VideoPlayer component
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        // Register for the video end event
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    // This function will be called when the video finishes playing
    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    void OnDestroy()
    {
        // Unregister the event when the object is destroyed
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}
