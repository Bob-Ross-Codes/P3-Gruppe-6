using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightOnOff : MonoBehaviour
{
    public Light targetLight;  // Reference to the Light component
    public float flashlightRange = 10f; // Max range of the flashlight
    private bool isLightOn = false;  // Keep track of the light's state

 

    void Start()
    {
        // Ensure the light is off at the start (optional)
        if (targetLight != null)
        {
            targetLight.enabled = isLightOn;
        }
    }

    void Update()
    {
        // Check for right-click (mouse button 1)
        if (Input.GetMouseButtonDown(1))
        {
            // Toggle light on/off
            isLightOn = !isLightOn;
            targetLight.enabled = isLightOn;
        }



}
}