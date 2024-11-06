using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneShift : MonoBehaviour
{
    // Time in seconds to wait before loading the next scene
    public float delayTime = 30f;
    
    // The name or index of the scene to load
    public string sceneName;

    private void Start()
    {
        // Start the coroutine when the script runs
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        // Wait for the specified amount of time
        yield return new WaitForSeconds(delayTime);

        // Load the scene
        SceneManager.LoadScene(sceneName);
    }
}
