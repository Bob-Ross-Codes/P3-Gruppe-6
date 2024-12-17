using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles triggering a flickering light effect and spawning an object (e.g., a scary woman model) 
/// when the player enters a designated trigger zone.
/// After flickering finishes, it enables the target object and associated gaze collider.
/// </summary>
public class ScaryWoman : MonoBehaviour
{
    [Header("Object Settings")]
    [Tooltip("The object to spawn after the flicker effect ends.")]
    [SerializeField] private GameObject targetObject;

    [Tooltip("The GazeCollider GameObject to activate after spawning the target.")]
    [SerializeField] private GameObject GazeCollider;

    [Header("Light Flicker Settings")]
    [Tooltip("Reference to the LightManager script controlling flickering.")]
    [SerializeField] private LightManager lightManager;

    [Tooltip("Duration of the flickering effect in seconds.")]
    [SerializeField] private float flickerDuration = 2f;

    [Tooltip("Speed of the flickering effect.")]
    [SerializeField] private float flickerSpeed = 1f;

    [Tooltip("Whether to include the player's handlight in the flicker effect.")]
    [SerializeField] private bool includeHandLight = true;

    private bool objectSpawned = false; // Ensures the object spawns only once

    /// <summary>
    /// Initializes the target object and gaze collider state.
    /// Ensures the object is initially visible and the gaze collider is disabled.
    /// </summary>
    private void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Target Object is not assigned. Please assign it in the Inspector.");
        }

        if (GazeCollider != null)
        {
            GazeCollider.SetActive(false);
        }
        else
        {
            Debug.LogWarning("GazeCollider is not assigned. Please assign it in the Inspector.");
        }
    }

    /// <summary>
    /// Detects when the player enters the trigger zone.
    /// If the flicker hasn't been triggered and the object not spawned, starts the flicker effect and schedules object spawning.
    /// </summary>
    /// <param name="other">The collider entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !objectSpawned)
        {
            if (lightManager != null)
            {
                // Start the flicker effect
                lightManager.StartFlicker(flickerDuration, flickerSpeed, includeHandLight);

                // Spawn the object after the flicker effect is done
                StartCoroutine(SpawnAfterFlicker());
            }
            else
            {
                Debug.LogError("LightManager is not assigned. Please assign it in the Inspector.");
            }
        }
    }

    /// <summary>
    /// Waits for the flicker duration, then activates the target object and gaze collider.
    /// Marks the object as spawned to prevent repeat triggers.
    /// </summary>
    private IEnumerator SpawnAfterFlicker()
    {
        // Wait until the flickering is complete
        yield return new WaitForSeconds(flickerDuration);

        // Spawn the object
        if (targetObject != null)
        {
            targetObject.SetActive(true);

            if (GazeCollider != null)
            {
                GazeCollider.SetActive(true);
            }
            else
            {
                Debug.LogWarning("GazeCollider was not assigned, cannot enable it.");
            }

            objectSpawned = true;
        }
        else
        {
            Debug.LogError("Target Object is not assigned. Cannot enable target object and collider.");
        }
    }
}