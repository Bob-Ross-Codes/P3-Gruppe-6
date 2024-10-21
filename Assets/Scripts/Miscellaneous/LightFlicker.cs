using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Reference to the Light component
    private Light pointLight;

    // Minimum and maximum intensity values
    public float minIntensity = 0f;
    public float maxIntensity = 2.0f;

    // Flicker timing settings
    public float minFlickerDelay = 0.1f;
    public float maxFlickerDelay = 0.5f;

    private float flickerTimer;

    void Start()
    {
        // Get the Light component attached to the GameObject
        pointLight = GetComponent<Light>();

        // Set an initial flicker delay
        flickerTimer = Random.Range(minFlickerDelay, maxFlickerDelay);
    }

    void Update()
    {
        // Decrease the timer
        flickerTimer -= Time.deltaTime;

        // If the timer reaches zero, flicker the light
        if (flickerTimer <= 0f)
        {
            // Randomly turn the light on or off
            pointLight.intensity = Random.value > 0.5f ? maxIntensity : minIntensity;

            // Reset the flicker timer to a new random value
            flickerTimer = Random.Range(minFlickerDelay, maxFlickerDelay);
        }
    }
}
