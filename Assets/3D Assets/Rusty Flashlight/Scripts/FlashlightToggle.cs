using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

/// <summary>
/// Controls the flashlight toggle functionality.
/// Handles enabling/disabling the light GameObject and triggering corresponding Wwise sound events.
/// </summary>
public class FlashlightToggle : MonoBehaviour
{
    [Header("Flashlight Settings")]
    [Tooltip("The light GameObject to toggle on/off.")]
    public GameObject lightGO;

    // Tracks whether the flashlight is on or off
    private bool isOn = false;

    /// <summary>
    /// Initializes the flashlight state to 'off' when the game starts.
    /// Ensures the light starts in a disabled state.
    /// </summary>
    void Start()
    {
        if (lightGO != null)
        {
            lightGO.SetActive(isOn); // Ensure light is off initially
        }
        else
        {
            Debug.LogWarning("Light GameObject (lightGO) is not assigned in the Inspector.");
        }
    }

    /// <summary>
    /// Monitors player input to toggle the flashlight state.
    /// Toggles light visibility and triggers Wwise sound events when the 'X' key is pressed.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ToggleFlashlight();
        }
    }

    /// <summary>
    /// Toggles the flashlight state.
    /// Updates the light's visibility and sends corresponding Wwise sound events.
    /// </summary>
    private void ToggleFlashlight()
    {
        // Toggle the flashlight state
        isOn = !isOn;

        if (lightGO != null)
        {
            lightGO.SetActive(isOn);

            // Trigger appropriate Wwise events
            if (isOn)
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
        else
        {
            Debug.LogWarning("Light GameObject (lightGO) is not assigned in the Inspector.");
        }
    }
}