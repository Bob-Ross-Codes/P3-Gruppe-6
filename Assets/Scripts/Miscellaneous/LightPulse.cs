using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a pulsing light effect by oscillating the intensity of the light component over time.
/// The effect is created using a sine wave for smooth transitions between minimum and maximum intensities.
/// </summary>
public class LightPulse : MonoBehaviour
{
    [Header("Pulse Settings")]
    [Tooltip("Minimum intensity of the light during the pulse.")]
    [SerializeField] private float minIntensity = 0f;

    [Tooltip("Maximum intensity of the light during the pulse.")]
    [SerializeField] private float maxIntensity = 6f;

    [Tooltip("Speed at which the light pulses.")]
    [SerializeField] private float pulseSpeed = 5f;

    private Light areaLight; // Reference to the Light component attached to the GameObject

    /// <summary>
    /// Initializes the Light component reference.
    /// </summary>
    private void Awake()
    {
        // Get the Light component attached to the same GameObject
        areaLight = GetComponent<Light>();
    }

    /// <summary>
    /// Updates the light's intensity each frame to create the pulsing effect.
    /// </summary>
    private void Update()
    {
        if (areaLight != null)
        {
            // Calculate intensity using a sine wave mapped to the range [minIntensity, maxIntensity]
            float intensity = Mathf.Lerp(
                minIntensity, 
                maxIntensity, 
                (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2
            );

            // Apply the calculated intensity to the light
            areaLight.intensity = intensity;
        }
        else
        {
            Debug.LogWarning("Light component is missing. Please attach a Light component to this GameObject.");
        }
    }
}