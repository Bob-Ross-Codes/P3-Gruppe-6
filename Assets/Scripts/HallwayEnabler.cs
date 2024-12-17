using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the activation and deactivation of hallways as the player progresses through the game.
/// Controls hallway enabling logic based on collision triggers with "HallwayEnabler" tagged objects.
/// </summary>
public class HallwayEnabler : MonoBehaviour
{
    [Header("Hallway Settings")]
    [Tooltip("Array of hallway GameObjects to enable or disable.")]
    [SerializeField] private GameObject[] hallways;

    [Tooltip("Array of GameObjects that act as triggers to enable hallways.")]
    [SerializeField] private GameObject[] hallwayEnablers;

    [Tooltip("Index of the current hallway to enable.")]
    [SerializeField] private int hallwayToEnable = 1;

    [Header("Lighting Settings")]
    [Tooltip("Reference to the LightManager script for managing lights.")]
    [SerializeField] private LightManager lightManager;

    /// <summary>
    /// Initializes the hallways by deactivating all hallways except the first one at the start.
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < hallways.Length; i++)
        {
            if (i != 0) // Only leave the first hallway active
            {
                hallways[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Handles trigger events. When the player collides with a "HallwayEnabler" object,
    /// it activates the next hallway, deactivates the previous one, and updates the lighting.
    /// </summary>
    /// <param name="other">The collider entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "HallwayEnabler" tag
        if (other.CompareTag("HallwayEnabler"))
        {
            // Destroy the corresponding enabler object
            Destroy(hallwayEnablers[hallwayToEnable]);

            // Update the lighting system
            lightManager.allLights = FindObjectsOfType<Light>();

            // Ensure the hallways array is not null and has valid elements
            if (hallways != null && hallways.Length > hallwayToEnable)
            {
                Debug.Log("You are in hallway: " + hallways[hallwayToEnable].name);

                // Activate the next hallway
                if (hallwayToEnable + 1 < hallways.Length && hallways[hallwayToEnable + 1] != null)
                {
                    hallways[hallwayToEnable + 1].SetActive(true);
                    Debug.Log("Enabling hallway: " + hallways[hallwayToEnable + 1].name);
                }

                // Deactivate the previous hallway
                if (hallwayToEnable > 0)
                {
                    hallways[hallwayToEnable - 1].SetActive(false);
                    Debug.Log("Disabling hallway: " + hallways[hallwayToEnable - 1].name);
                }

                // Move to the next hallway index
                hallwayToEnable++;
            }
            else
            {
                Debug.LogWarning("Hallways array is empty, not set, or out of bounds.");
            }
        }
    }
}