using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the end of the final hallway sequence by detecting the player's collision with a trigger.
/// Loads the "Ending" scene when the player reaches the collider.
/// </summary>
public class EndScript : MonoBehaviour
{
    /// <summary>
    /// Detects when the player enters the collider.
    /// Loads the "Ending" scene if the colliding object has the "Player" tag.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has reached the end of the hallway");

            // Load the final scene called "Ending"
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
        }
    }
}