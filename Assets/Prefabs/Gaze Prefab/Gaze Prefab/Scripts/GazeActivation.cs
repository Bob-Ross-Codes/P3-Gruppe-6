using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for implementing gaze-based interactions.
/// Tracks how long the player looks at an object and triggers an event when a specified threshold is reached.
/// </summary>
public abstract class GazeActivation : MonoBehaviour
{
    private float lookTime; // Tracks the total time the player has looked at the object
    private bool isActivated = false; // Indicates whether the activation has already occurred

    /// <summary>
    /// Defines the time required to look at the object before triggering the activation.
    /// </summary>
    public abstract float ActivationTime { get; }

    /// <summary>
    /// Abstract method to handle the activation logic when the gaze duration is met.
    /// </summary>
    public abstract void OnLookedAt();

    /// <summary>
    /// Updates the accumulated look time.
    /// Triggers the activation if the look time exceeds the specified threshold.
    /// </summary>
    /// <param name="deltaTime">The time increment to add to the look time.</param>
    public void UpdateLookTime(float deltaTime)
    {
        if (isActivated) return; // Do nothing if the object is already activated

        lookTime += deltaTime; // Accumulate look time

        if (lookTime >= ActivationTime) // Check if the accumulated time exceeds the threshold
        {
            OnLookedAt(); // Trigger the activation logic
            isActivated = true; // Mark the object as activated
            lookTime = 0.0f; // Reset the look time after activation
        }
    }

    /// <summary>
    /// Resets the accumulated look time to zero.
    /// </summary>
    public void ResetLookTime()
    {
        lookTime = 0.0f;
    }
}