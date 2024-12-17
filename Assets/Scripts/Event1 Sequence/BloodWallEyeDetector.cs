/// <summary>
/// Handles light flickering when the player looks at the blood wall.
/// </summary>
using System.Collections;
using UnityEngine;

public class BloodWallEyeDetector : GazeActivation
{
    [Header("Light Flickering Settings")]
    [SerializeField] private Light lightToFlicker; // The light to flicker
    [SerializeField] private float flickerDuration = 3.0f; // Total duration of flickering
    [SerializeField] private float flickerInterval = 0.5f; // Interval between light toggles

    [Header("Object Settings")]
    [SerializeField] private GameObject colliderObject; // GameObject to destroy after flickering

    private int flickerCount = 0; // Tracks how many times flickering has occurred
    private bool isFlickering = false; // Is the light currently flickering?

    /// <summary>
    /// Time required to activate the effect after detecting gaze.
    /// </summary>
    public override float ActivationTime => 0.2f;

    /// <summary>
    /// Triggers the light flickering effect when the player looks at the blood wall.
    /// </summary>
    public override void OnLookedAt()
    {
        if (!isFlickering)
        {
            StartCoroutine(FlickerLight());
            isFlickering = true;
            Debug.Log("Player gaze detected, flickering started.");
        }
    }

    /// <summary>
    /// Toggles the light on and off at random intervals for the set duration.
    /// </summary>
    private IEnumerator FlickerLight()
    {
        float elapsedTime = 0f;

        while (elapsedTime < flickerDuration)
        {
            lightToFlicker.enabled = !lightToFlicker.enabled; // Toggle light state
            yield return new WaitForSeconds(flickerInterval); // Wait for the interval

            elapsedTime += flickerInterval; // Increment elapsed time
            flickerInterval = Random.Range(0.05f, 1f); // Set a new random interval
        }

        lightToFlicker.enabled = true; // Ensure the light remains on
        isFlickering = false;

        Debug.Log("Flickering completed.");

        if (flickerCount == 0)
        {
            Debug.Log("Destroying collider object.");
            Destroy(colliderObject);
        }
        else
        {
            flickerCount++;
        }
    }
}