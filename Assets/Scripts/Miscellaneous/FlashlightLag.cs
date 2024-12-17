using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adds a lagging effect to the flashlight, making it smoothly follow the camera's rotation.
/// The effect simulates inertia, creating a more dynamic and immersive feel.
/// </summary>
public class FlashlightLag : MonoBehaviour
{
    [Header("Lag Settings")]
    [Tooltip("Reference to the camera's Transform that the flashlight will follow.")]
    public Transform cameraTransform;

    [Tooltip("Speed at which the flashlight catches up to the camera's rotation.")]
    [SerializeField] private float lagSpeed = 5f;

    private Quaternion targetRotation; // Stores the target rotation for the flashlight

    /// <summary>
    /// Initializes the target rotation to match the camera's initial rotation.
    /// Ensures a smooth start for the lag effect.
    /// </summary>
    void Start()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned. Please assign it in the Inspector.");
            enabled = false; // Disable the script if cameraTransform is not assigned
            return;
        }

        targetRotation = cameraTransform.rotation;
    }

    /// <summary>
    /// Updates the flashlight's rotation each frame to create a lagging effect.
    /// Smoothly interpolates the flashlight's rotation toward the camera's current rotation.
    /// </summary>
    void Update()
    {
        if (cameraTransform != null)
        {
            // Set the target rotation to match the camera's current rotation
            targetRotation = cameraTransform.rotation;

            // Interpolate the flashlight's rotation towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lagSpeed);
        }
    }
}