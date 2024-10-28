using UnityEngine;
using StarterAssets;  // Import the StarterAssets namespace
using FMODUnity;     // Import FMODUnity for event control

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public FMODUnity.EventReference MyEvent;  // FMOD event reference
    private FMOD.Studio.EventInstance eventInstance;  // Event instance to control play and stop
    public bool isPaused = false;
    void Start()
    {
        // Create the event instance at the start of the script
        eventInstance = RuntimeManager.CreateInstance(MyEvent);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (FirstPersonController.isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Resume time in the game
        FirstPersonController.isPaused = false;  // Re-enable character controls

        // Hide and lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Stop the FMOD event
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Freeze the game
        FirstPersonController.isPaused = true;  // Disable character controls

        // Show and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Start the FMOD event
        eventInstance.start();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    void OnDestroy()
    {
        // Release the event instance when the object is destroyed
        eventInstance.release();
    }
}
