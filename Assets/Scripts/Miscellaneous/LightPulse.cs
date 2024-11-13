using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour
{
    [SerializeField] private float minIntensity = 0f; // Minimum light intensity
    [SerializeField] private float maxIntensity = 6f; // Maximum light intensity
    [SerializeField] private float pulseSpeed = 5f; // Speed of pulsing effect

    private Light areaLight;

    private void Awake()
    {
        // Get the Light component attached to the same GameObject
        areaLight = GetComponent<Light>();
    }

    private void Update()
    {
        if (areaLight != null)
        {
            // Calculate intensity based on sine wave over time
            float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * pulseSpeed) + 1) / 2);
            
            // Apply the calculated intensity to the light
            areaLight.intensity = intensity;
        }
    }
}