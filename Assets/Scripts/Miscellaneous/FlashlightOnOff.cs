using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flashlight functionality, including toggling the light on/off,
/// playing Wwise sound events, and adding a swerving effect to simulate movement.
/// </summary>
public class FlashlightOnOff : MonoBehaviour
{
    [Header("Flashlight Settings")]
    [Tooltip("Reference to the Light component representing the flashlight.")]
    public Light targetLight;

    [Tooltip("Maximum range of the flashlight.")]
    public float flashlightRange = 10f;

    private bool isLightOn = false; // Tracks the flashlight's state (on/off)

    [Header("Swerving Effect Settings")]
    [Tooltip("Reference to the player's camera for orientation.")]
    public Transform playerCamera;

    [Tooltip("Maximum angle for the swerving effect.")]
    public float swerveAmount = 5f;

    [Tooltip("Speed of the swerving effect.")]
    public float swerveSpeed = 2f;

    private Quaternion initialRotation; // Stores the initial rotation of the flashlight

    /// <summary>
    /// Initializes the flashlight's state and stores its initial rotation.
    /// </summary>
    void Start()
    {
        // Ensure the flashlight is off at the start
        if (targetLight != null)
        {
            targetLight.enabled = isLightOn;
        }

        // Store the initial rotation relative to the player
        initialRotation = transform.localRotation;
    }

    /// <summary>
    /// Updates the flashlight's state and applies the swerving effect based on player's movement.
    /// </summary>
    void Update()
    {
        HandleFlashlightToggle();
        ApplySwervingEffect();
    }

    /// <summary>
    /// Toggles the flashlight on/off when the right mouse button is clicked.
    /// Triggers appropriate Wwise sound events based on the flashlight state.
    /// </summary>
    private void HandleFlashlightToggle()
    {
        // Check for right-click (mouse button 1)
        if (Input.GetMouseButtonDown(1))
        {
            // Toggle the flashlight state
            isLightOn = !isLightOn;

            if (targetLight != null)
            {
                targetLight.enabled = isLightOn;
            }

            // Play Wwise sound events based on the flashlight state
            if (isLightOn)
            {
                AkSoundEngine.SetSwitch("FlashlightSwitch", "On", gameObject);
                AkSoundEngine.PostEvent("Flashlight_OnOff_Event", gameObject);
            }
            else
            {
                AkSoundEngine.SetSwitch("FlashlightSwitch", "Off", gameObject);
                AkSoundEngine.PostEvent("Flashlight_OnOff_Event", gameObject);
            }
        }
    }

    /// <summary>
    /// Applies a swerving effect to simulate movement of the flashlight.
    /// Adds a dynamic motion based on sine and cosine wave calculations.
    /// </summary>
    private void ApplySwervingEffect()
    {
        // Calculate an offset based on time and swerving speed
        float swayX = Mathf.Sin(Time.time * swerveSpeed) * swerveAmount;
        float swayY = Mathf.Cos(Time.time * swerveSpeed) * swerveAmount;

        // Create a rotation based on the calculated offset
        Quaternion swerveRotation = Quaternion.Euler(swayX, swayY, 0);

        // Apply the swerving effect on top of the initial rotation
        transform.localRotation = initialRotation * swerveRotation;
    }
}