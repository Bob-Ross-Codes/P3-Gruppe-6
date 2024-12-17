using UnityEngine;

/// <summary>
/// Manages a light component to simulate thunder effects by controlling light intensity and flickering behavior.
/// The light randomly flickers between minimum and maximum intensity values, with pauses between flicker sequences.
/// </summary>
public class ThunderLight : MonoBehaviour
{
    [Header("Light Component Settings")]
    [Tooltip("Reference to the Light component to control.")]
    [SerializeField] private Light pointLight;

    [Header("Intensity Settings")]
    [Tooltip("Minimum intensity value of the light.")]
    [SerializeField] private float minIntensity = 0f;

    [Tooltip("Maximum intensity value of the light.")]
    [SerializeField] private float maxIntensity = 2.0f;

    [Header("Flicker Timing Settings")]
    [Tooltip("Minimum delay between flickers in seconds.")]
    [SerializeField] private float minFlickerDelay = 0.05f;

    [Tooltip("Maximum delay between flickers in seconds.")]
    [SerializeField] private float maxFlickerDelay = 0.2f;

    [Header("Thunder Pause Settings")]
    [Tooltip("Minimum duration of pause between flicker sequences in seconds.")]
    [SerializeField] private float minThunderPause = 3f;

    [Tooltip("Maximum duration of pause between flicker sequences in seconds.")]
    [SerializeField] private float maxThunderPause = 8f;

    // Timer to track time until the next flicker or pause
    private float flickerTimer;

    // Indicates whether the light is currently flickering
    private bool isFlickering = false;

    /// <summary>
    /// Initializes the Light component and sets the initial timer for the first flicker pause.
    /// </summary>
    private void Start()
    {
        // Attempt to get the Light component if not already assigned
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
            if (pointLight == null)
            {
                Debug.LogError($"{nameof(ThunderLight)} requires a Light component on the same GameObject.");
                enabled = false; // Disable the script if Light component is missing
                return;
            }
        }

        // Initialize the flicker timer with a random pause duration
        flickerTimer = GetNextPauseDuration();
    }

    /// <summary>
    /// Updates the flicker timer and controls the light's flickering and pausing behavior.
    /// </summary>
    private void Update()
    {
        // Decrement the timer by the time elapsed since the last frame
        flickerTimer -= Time.deltaTime;

        // Check if it's time to perform a flicker or enter a pause
        if (flickerTimer <= 0f)
        {
            if (isFlickering)
            {
                // Randomly set the light's intensity to either maximum or minimum value
                pointLight.intensity = Random.value > 0.5f ? maxIntensity : minIntensity;

                // Reset the flicker timer with a random delay for the next flicker
                flickerTimer = Random.Range(minFlickerDelay, maxFlickerDelay);

                // 20% chance to end the flickering sequence and enter a thunder pause
                if (Random.value > 0.8f)
                {
                    isFlickering = false;
                    flickerTimer = GetNextPauseDuration();
                }
            }
            else
            {
                // If not currently flickering, set the light to minimum intensity and start flickering
                pointLight.intensity = minIntensity;
                isFlickering = true;
                flickerTimer = Random.Range(minFlickerDelay, maxFlickerDelay);
            }
        }
    }

    /// <summary>
    /// Generates a random duration for the thunder pause between flicker sequences.
    /// </summary>
    /// <returns>A random float value between minThunderPause and maxThunderPause.</returns>
    private float GetNextPauseDuration()
    {
        return Random.Range(minThunderPause, maxThunderPause);
    }
}