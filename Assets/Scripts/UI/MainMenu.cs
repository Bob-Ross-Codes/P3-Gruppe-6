using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the main menu functionality, including starting the game and exiting the application.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Starts the game by loading the "Intro-video" scene.
    /// Called when the "Play" button is clicked.
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene("Intro-video"); // Load the scene named "Intro-video"
    }

    /// <summary>
    /// Quits the application.
    /// Called when the "Quit" button is clicked.
    /// </summary>
    public void Quit()
    {
        Application.Quit(); // Exit the application
    }
}