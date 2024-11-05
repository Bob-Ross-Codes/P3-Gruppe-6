using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightOnOff : MonoBehaviour
{
    public Light targetLight;  // Reference to the Light component
    public float flashlightRange = 10f; // Max range of the flashlight
    private bool isLightOn = false;  // Keep track of the light's state

 
 public Transform playerCamera;  // Reference to the player's camera
    public float swerveAmount = 5f; // Maximum angle for the swerve
    public float swerveSpeed = 2f;  // Speed of the swerving effect

    private Quaternion initialRotation;


    
    void Start()
    {
        // Ensure the light is off at the start (optional)
        if (targetLight != null)
        {
            targetLight.enabled = isLightOn;
        }



         // Store the initial rotation relative to the player
        initialRotation = transform.localRotation;
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



  // Calculate an offset based on the player's look direction
        float swayX = Mathf.Sin(Time.time * swerveSpeed) * swerveAmount;
        float swayY = Mathf.Cos(Time.time * swerveSpeed) * swerveAmount;

        // Apply the swerving effect on top of the initial rotation
        Quaternion swerveRotation = Quaternion.Euler(swayX, swayY, 0);
        transform.localRotation = initialRotation * swerveRotation;


}
}