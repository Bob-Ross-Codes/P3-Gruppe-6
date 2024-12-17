using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the rotation of the monster to face its movement direction.
/// Ensures the monster rotates dynamically based on its movement each frame.
/// </summary>
public class MonsterRotation : MonoBehaviour
{
    [Header("Tracking Settings")]
    [Tooltip("Stores the monster's position from the last frame to calculate movement direction.")]
    private Vector3 lastPosition;

    /// <summary>
    /// Initializes the last position to the monster's starting position.
    /// </summary>
    void Start()
    {
        lastPosition = transform.position;
    }

    /// <summary>
    /// Updates the monster's rotation to face the direction of movement.
    /// Ensures the monster always rotates correctly when moving.
    /// </summary>
    void Update()
    {
        // Calculate the movement direction by subtracting the last position from the current position
        Vector3 movementDirection = transform.position - lastPosition;

        // Check if the monster is moving
        if (movementDirection != Vector3.zero)
        {
            // Calculate the target rotation to face the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            // Apply the calculated rotation to the monster
            transform.rotation = targetRotation;

            // Adjust the rotation to face a specific angle (e.g., 90 degrees on the Y-axis)
            transform.rotation *= Quaternion.Euler(0f, 90f, 0f);
        }

        // Update the last position for the next frame
        lastPosition = transform.position;
    }
}