using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the gaze interaction with the door. 
/// Stops the sound played by the `Journal2EyeDetector` when the player looks at the door.
/// </summary>
public class Door2EyeDetector : GazeActivation
{
    /// <summary>
    /// The required time to activate the gaze event, set to 0.01 seconds.
    /// </summary>
    public override float ActivationTime => 0.01f;

    [Header("References")]
    [Tooltip("Reference to the Journal2EyeDetector script to stop its sound.")]
    [SerializeField] private Journal2EyeDetector journal2EyeDetector;

    /// <summary>
    /// Called when the player looks at the door for the required time.
    /// Stops the sound effect from the Journal2EyeDetector script.
    /// </summary>
    public override void OnLookedAt()
    {
        // Stop the sound from the Journal2EyeDetector
        journal2EyeDetector.StopSound();
    }
}