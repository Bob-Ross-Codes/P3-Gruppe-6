using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles gaze-based interaction with a keypad. Triggers events when the player looks at the keypad
/// for the required duration, including notifying the `KeypadController` and managing flashing images.
/// </summary>
public class KeypdEyetrckng : GazeActivation
{
    [Header("References")]
    [Tooltip("Reference to the KeypadController to handle keypad-specific logic.")]
    public KeypadController keypadController;

    [Tooltip("GameObject representing the flashing images to deactivate upon gaze activation.")]
    [SerializeField] private GameObject flashingImages;

    /// <summary>
    /// The time in seconds the player must look at the keypad to trigger the activation.
    /// </summary>
    public override float ActivationTime => 2.5f;

    /// <summary>
    /// Called when the player has looked at the keypad for the required duration.
    /// Notifies the KeypadController and deactivates flashing images if they exist.
    /// </summary>
    public override void OnLookedAt()
    {
        // Notify the KeypadController of the gaze activation
        keypadController.LookAtKeypad();

        // Deactivate flashing images if assigned
        if (flashingImages != null)
        {
            flashingImages.SetActive(false);
        }

        // Log the activation event
        Debug.Log("Looked At Keypad");
    }
}