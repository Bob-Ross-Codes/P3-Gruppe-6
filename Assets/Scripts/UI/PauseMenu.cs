using UnityEngine;
using StarterAssets;  // Import the StarterAssets namespace

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
<<<<<<< HEAD
    public bool isPaused = false;
=======
 
    public bool isPaused = false;
    void Start()
    {
 
    }
>>>>>>> parent of 18f9285 (Merge Wwise)

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
<<<<<<< HEAD
=======

>>>>>>> parent of 18f9285 (Merge Wwise)
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;  // Freeze the game
        FirstPersonController.isPaused = true;  // Disable character controls

        // Show and unlock the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
<<<<<<< HEAD
=======


>>>>>>> parent of 18f9285 (Merge Wwise)
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }

    
}
