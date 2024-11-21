using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
      private Light pointLight;

    // Minimum and maximum intensity values
    public float minIntensity = 0f;
    public float maxIntensity = 2.0f;

    // Flicker timing settings
    public float minFlickerDelay = 0.05f;
    public float maxFlickerDelay = 0.2f;
    
    // Thunder pause settings
    public float minThunderPause = 3f; // Minimum pause before the next thunder flicker
    public float maxThunderPause = 8f; // Maximum pause before the next thunder flicker

    private float flickerTimer;
    private bool isFlickering = false;

    void Start()
    {
        pointLight = GetComponent<Light>();
        flickerTimer = GetNextPauseDuration();
         pointLight.intensity = minIntensity; // Ensure the light starts off
    }

    void Update()
    {
        flickerTimer -= Time.deltaTime;

        if (flickerTimer <= 0f)
        {
            if (isFlickering)
            {
                // Randomly turn the light on or off in flicker mode
                pointLight.intensity = Random.value > 0.5f ? maxIntensity : minIntensity;
                
                // Set next flicker delay within flicker mode
                flickerTimer = Random.Range(minFlickerDelay, maxFlickerDelay);

                // Randomly end flicker mode to enter thunder pause
                if (Random.value > 0.8f) // Adjust probability as needed
                {
                    isFlickering = false;
                    flickerTimer = GetNextPauseDuration();
                }
            }
            else
            {
                // If not flickering, turn off the light and wait for the next thunder cycle
                pointLight.intensity = minIntensity;
                isFlickering = true;
                flickerTimer = Random.Range(minFlickerDelay, maxFlickerDelay);
            }
        }
    }

    private float GetNextPauseDuration()
    {
        // Set a random duration for the thunder pause
        return Random.Range(minThunderPause, maxThunderPause);
    }
}
