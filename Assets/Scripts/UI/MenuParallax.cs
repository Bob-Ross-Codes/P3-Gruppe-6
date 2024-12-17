using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a parallax effect in the menu by moving objects based on mouse movement.
/// The objects subtly shift positions relative to the cursor for a dynamic UI experience.
/// </summary>
public class MenuParallax : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("Multiplier to control the parallax effect intensity.")]
    public float offsetMultiplier = 0.05f;

    private Vector3 startPosition; // Stores the initial position of the object

    /// <summary>
    /// Records the initial position of the object when the script starts.
    /// </summary>
    private void Start()
    {
        startPosition = transform.position;
    }

    /// <summary>
    /// Updates the object's position to create a parallax effect based on mouse movement.
    /// </summary>
    private void Update()
    {
        // Get the normalized mouse position, centered at (0, 0) with a range of [-0.5, 0.5]
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f);

        // Calculate the target position with an offset multiplier for subtle movement
        Vector3 targetPosition = startPosition + new Vector3(offset.x * offsetMultiplier, offset.y * offsetMultiplier, 0);

        // Update the object's position to follow the calculated target position
        transform.position = targetPosition;
    }
}