using System.Collections;
using UnityEngine;

/// <summary>
/// Manages the enabling of lights in a sequence with specified delays when the scene starts.
/// This script disables all specified lights initially and then enables each light one by one
/// with a delay between each activation.
/// </summary>
public class SceneLoadEvents : MonoBehaviour
{
    [Header("Light Settings")]
    [Tooltip("Array of Light components to manage.")]
    [SerializeField] private Light[] lights; // Array to hold the lights you want to enable

    [Header("Activation Settings")]
    [Tooltip("Delay in seconds before each light is enabled.")]
    [SerializeField] private float activationDelay = 1.5f; // Delay before enabling each light

    /// <summary>
    /// Called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// Initializes the light states by disabling them and starts the coroutine to enable them sequentially.
    /// </summary>
    private void Start()
    {
        if (lights == null || lights.Length == 0)
        {
            Debug.LogWarning($"{nameof(SceneLoadEvents)}: No lights assigned in the Inspector.");
            return;
        }

        // Disable all lights initially
        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.enabled = false;
            }
            else
            {
                Debug.LogWarning($"{nameof(SceneLoadEvents)}: A light in the array is not assigned.");
            }
        }

        // Start the coroutine to enable lights with delay
        StartCoroutine(EnableLightsWithDelay());
    }

    /// <summary>
    /// Coroutine that enables each light in the 'lights' array one by one with a specified delay.
    /// </summary>
    /// <returns>IEnumerator for coroutine execution.</returns>
    private IEnumerator EnableLightsWithDelay()
    {
        foreach (Light light in lights)
        {
            if (light != null)
            {
                yield return new WaitForSeconds(activationDelay); // Wait for the specified delay
                light.enabled = true; // Enable the light
                Debug.Log($"{nameof(SceneLoadEvents)}: {light.name} has been enabled.");
            }
            else
            {
                Debug.LogWarning($"{nameof(SceneLoadEvents)}: Encountered a null light reference. Skipping activation.");
            }
        }

        Debug.Log($"{nameof(SceneLoadEvents)}: All lights have been enabled.");
    }
}