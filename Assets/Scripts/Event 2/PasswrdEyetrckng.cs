using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Handles the interaction with the password using eye-tracking.
/// Activates flashing images and notifies the KeypadController when the password is looked at.
/// </summary>
public class PasswrdEyetrckng : GazeActivation
{
    [Header("References")]
    [Tooltip("Reference to the KeypadController to handle password-related logic.")]
    public KeypadController keypadController;

    [Tooltip("The GameObject containing the flashing images to activate.")]
    [SerializeField] private GameObject flashingImages;

    /// <summary>
    /// The time required to look at the password before activating the event.
    /// </summary>
    public override float ActivationTime => 0.1f;

    /// <summary>
    /// Called when the user looks at the password for the required duration.
    /// Activates flashing images and notifies the KeypadController.
    /// </summary>
    public override void OnLookedAt()
    {
        if (flashingImages != null)
        {
            flashingImages.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Flashing images GameObject is not assigned.");
        }

        if (keypadController != null)
        {
            keypadController.LookAtPassword();
        }
        else
        {
            Debug.LogWarning("KeypadController is not assigned.");
        }

        Debug.Log("Looked At Password");
    }
}