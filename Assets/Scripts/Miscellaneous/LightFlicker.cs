using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates a flickering light effect, with a thunder-like pause between flickering cycles.
/// The light intensity alternates randomly during flicker mode and pauses for a random duration between cycles.
/// </summary>
public class LightFlicker : MonoBehaviour
{
    [Header("Light Settings")]
    [Tooltip("The minimum intensity of the light during flicker.")]
    public float minIntensity = 0f;

    [Tooltip("The maximum intensity of the light during flicker.")]
    public float maxIntensity = 2.0f;

    [Header("Flicker Timing Settings")]
    [Tooltip("The minimum delay between light flickers.")]
    public float minFlickerDelay = 0.05f;

    [Tooltip("The maximum delay between light flickers.")]
    public float maxFlickerDelay = 0.2f;

    [Header("Thunder Pause Settings")]
    [Tooltip("The minimum pause duration between thunder flicker cycles.")]
    public float minThunderPause = 3f;

    [Tooltip("The maximum pause duration between thunder flicker cycles.")]
    public float maxThunderPause = 8f;

    private Light pointLight; // Reference to the Light component
    private float flickerTimer; // Tracks the time remaining before the next flicker or pause
    private bool isFlickering = false; // Tracks whether the light is in flicker mode

    /// <summary>
    /// Initializes the Light component and sets the initial flicker timer.
    /// </summary>
    void Start()
    {
        pointLight = GetComponent<Light>();

        if (pointLight == null)
        {
            Debug.LogWarning("Light component is missing on this GameObject.");
            enabled = false; // Disable the script if no Light component is found
            return;
        }

        flickerTimer = GetNextPauseDuration(); // Set the initial pause duration
        pointLight.intensity = minIntensity; // Ensure the light starts off
    }

    /// <summary>
    /// Updates the flicker logic every frame, alternating between flicker and thunder pause modes.
    /// </summary>
    void Update()
    {
        flickerTimer -= Time.deltaTime;

        if (flickerTimer <= 0f)
        {
            if (isFlickering)
            {
                // In flicker mode: randomly turn the light on or off
                pointLight.intensity = Random.value > 0.5f ? maxIntensity : minIntensity;

                // Set the next flicker delay
                flickerTimer = Random.Range(minFlickerDelay, maxFlickerDelay);

                // Randomly end flicker mode to start a thunder pause
                if (Random.value > 0.8f)
                {
                    isFlickering = false;
                    flickerTimer = GetNextPauseDuration();
                }
            }
            else
            {
                // In thunder pause mode: turn off the light and prepare for the next flicker cycle
                pointLight.intensity = minIntensity;
                isFlickering = true;
                flickerTimer = Random.Range(minFlickerDelay, maxFlickerDelay);
            }
        }
    }

    /// <summary>
    /// Calculates a random duration for the thunder pause.
    /// </summary>
    /// <returns>A random pause duration between the specified minimum and maximum values.</returns>
    private float GetNextPauseDuration()
    {
        return Random.Range(minThunderPause, maxThunderPause);
    }
}