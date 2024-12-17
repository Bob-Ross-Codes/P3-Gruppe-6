using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Include the Cinemachine namespace
using StarterAssets; // For FirstPersonController

/// <summary>
/// Manages the transition of the player into a running state upon entering a trigger zone.
/// Adjusts player speed, modifies footstep sound intervals, initiates camera FOV transitions,
/// and activates a monster GameObject. Also handles the destruction of specified objects when the player exits the trigger.
/// </summary>
public class RunEnableTrigger : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("The new walking speed to set for the player upon entering the trigger.")]
    public float newWalkSpeed = 2.0f;

    [Tooltip("Reference to the Cinemachine virtual camera for FOV transitions.")]
    public CinemachineVirtualCamera cinemachineCamera;

    [Tooltip("Reference to the FootstepSoundController to modify footstep intervals.")]
    public FootstepSoundController footstepSoundController;

    [Tooltip("Reference to the monster GameObject to activate.")]
    public GameObject monster;

    [Header("Object Destruction Settings")]
    [Tooltip("The GameObject to destroy when the player exits the trigger.")]
    public GameObject objectToDestroy;

    /// <summary>
    /// Initializes the state by ensuring the monster is inactive at the start.
    /// </summary>
    private void Start()
    {
        if (monster != null)
        {
            monster.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Monster GameObject is not assigned. Please assign it in the Inspector.");
        }
    }

    /// <summary>
    /// Detects when the player enters the trigger zone.
    /// Upon entry, it updates the player's movement speed, modifies footstep intervals,
    /// initiates a smooth transition of the camera's field of view (FOV), and activates the monster.
    /// </summary>
    /// <param name="other">The collider that enters the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Attempt to get the FirstPersonController component from the player
            FirstPersonController controller = other.GetComponent<FirstPersonController>();
            if (controller != null)
            {
                controller.MoveSpeed = newWalkSpeed; // Ensure FirstPersonController has a MoveSpeed property
                footstepSoundController.stepInterval = 0.3f; // Adjust footstep interval for running

                // Start a smooth transition of the camera's FOV from current to 90 over 1 second
                if (cinemachineCamera != null)
                {
                    StartCoroutine(SmoothTransition(cinemachineCamera.m_Lens.FieldOfView, 90f, 1.0f));
                }
                else
                {
                    Debug.LogWarning("Cinemachine Virtual Camera is not assigned. Please assign it in the Inspector.");
                }
            }
            else
            {
                Debug.LogWarning("FirstPersonController component not found on Player. Ensure the Player has the script attached.");
            }

            // Activate the monster GameObject
            if (monster != null)
            {
                monster.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Monster GameObject is not assigned. Please assign it in the Inspector.");
            }
        }
    }

    /// <summary>
    /// Coroutine to smoothly transition a float value from a start to an end value over a specified duration.
    /// Used here to transition the camera's field of view (FOV).
    /// </summary>
    /// <param name="from">The starting value of the transition.</param>
    /// <param name="to">The target value of the transition.</param>
    /// <param name="duration">The duration over which the transition occurs.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    private IEnumerator SmoothTransition(float from, float to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Interpolate the FOV based on elapsed time
            cinemachineCamera.m_Lens.FieldOfView = Mathf.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        // Ensure the final value is set
        cinemachineCamera.m_Lens.FieldOfView = to;
    }

    /// <summary>
    /// Detects when the player exits the trigger zone.
    /// Initiates the destruction of a specified GameObject after a delay.
    /// </summary>
    /// <param name="other">The collider that exits the trigger zone.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(DestroyObjectWithDelay(4.0f));
        }
    }

    /// <summary>
    /// Coroutine to destroy a specified GameObject after a given delay.
    /// </summary>
    /// <param name="delay">The time in seconds to wait before destroying the object.</param>
    /// <returns>IEnumerator for coroutine execution.</returns>
    private IEnumerator DestroyObjectWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }
        else
        {
            Debug.LogWarning("Object to Destroy is not assigned. Please assign it in the Inspector.");
        }
    }
}