using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Casts a ray from the player's gaze location to interact with objects in the scene.
/// Highlights objects with a GazeActivation component and updates their look time.
/// </summary>
public class RayCastAtSight : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the Gaze script that provides the player's gaze location.")]
    public Gaze gazeScript;

    [Tooltip("Reference to the main camera used for casting the ray.")]
    public Camera mainCamera;

    [Header("Debug Settings")]
    [Tooltip("Enables debug logs and visualizes the ray in the scene view.")]
    public bool deBug = false;

    /// <summary>
    /// Called once per frame to handle raycasting from the player's gaze location.
    /// </summary>
    void Update()
    {
        // Ensure that the Gaze script and main camera references are assigned
        if (gazeScript != null && mainCamera != null)
        {
            // Get the gaze location from the Gaze script
            Vector2 gazeLocation = gazeScript.gazeLocation;

            // Convert the gaze location to a ray
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(gazeLocation.x, gazeLocation.y, 0));

            // Perform a raycast to detect objects
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (deBug)
                {
                    // Log the object hit by the ray for debugging
                    Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
                }

                // Check if the hit object has a GazeActivation component
                GazeActivation gazeActivation = hit.collider.GetComponent<GazeActivation>();
                if (gazeActivation != null)
                {
                    // Update the look time for the GazeActivation object
                    gazeActivation.UpdateLookTime(Time.deltaTime);
                }
            }

            // Draw the ray in the scene view for debugging purposes
            if (deBug)
            {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
            }
        }
    }
}