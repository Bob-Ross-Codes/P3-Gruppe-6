using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles menu functionality, including transitioning to the gameplay scene and exiting the application.
/// </summary>
public class Menu : MonoBehaviour
{
    /// <summary>
    /// Called when the "Play" button is clicked.
    /// Loads the gameplay scene.
    /// </summary>
    public void OnPlayButton()
    {
        SceneManager.LoadScene(1); // Load the scene with index 1
    }

    /// <summary>
    /// Called when the "Quit" button is clicked.
    /// Exits the application.
    /// </summary>
    public void OnQuitButton()
    {
        Application.Quit(); // Quit the application
    }
}