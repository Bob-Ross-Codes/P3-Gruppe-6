using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadEvents : MonoBehaviour
{
    public Light[] lights; // Array to hold the lights you want to enable

    private void Start()
    {
        // Disable all lights initially
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
        
        // Start the coroutine to enable lights with delay
        StartCoroutine(EnableLightsWithDelay());
    }

    private IEnumerator EnableLightsWithDelay()
    {
        foreach (Light light in lights)
        {
            yield return new WaitForSeconds(1.5f); // Wait for 1 second
            light.enabled = true; // Enable the light
        }
    }
}
