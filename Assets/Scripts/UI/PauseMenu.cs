using UnityEngine;
using StarterAssets;  // Import the StarterAssets namespace

/// <summary>
/// Manages the game's pause menu functionality, including pausing, resuming, and quitting the game.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("The UI GameObject for the pause menu.")]
    public GameObject pauseMenuUI;

    [Header("State Management")]
    [Tooltip("Tracks whether the game is currently paused.")]
    public bool isPaused = false;

    /// <summary>
    /// Detects input to toggle the pause menu when the Escape key is pressed.
    /// </summary>
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

    /// <summary>
    /// Resumes the game by hiding the pause menu and re-enabling character controls.
    /// </summary>
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;  // Resume game time
        FirstPersonController.isPaused = false;  // Re-enable character controls

        // Hide and lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Pauses the game by showing the pause menu and disabling character controls.
    /// </summary>
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Freeze game time
        FirstPersonController.isPaused = true;  // Disable character controls

        // Show and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Quits the game. Logs a message in the editor and closes the application in a build.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}