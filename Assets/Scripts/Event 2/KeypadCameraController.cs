using UnityEngine;

/// <summary>
/// Controls the rotation of the keypad camera based on mouse movement.
/// Limits the rotation to a specified range and disables movement when hovering over the keypad.
/// </summary>
public class KeypadCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Sensitivity of the camera's movement.")]
    public float sensitivity = 0.1f;

    [Tooltip("Maximum angle the camera can rotate to the right.")]
    public float maxAngle = 30f;

    private float currentAngle = 0f; // Current rotation angle of the camera
    private float startAngle; // The initial rotation angle of the camera
    private bool isHoveringOverKeypad = false; // Tracks if the cursor is hovering over the keypad

    /// <summary>
    /// Stores the initial angle of the camera for relative rotation calculations.
    /// </summary>
    void Start()
    {
        startAngle = transform.localEulerAngles.y; // Initialize the starting angle
    }

    /// <summary>
    /// Updates the camera's rotation based on mouse movement,
    /// unless the cursor is hovering over the keypad.
    /// </summary>
    void Update()
    {
        if (isHoveringOverKeypad) return; // Prevent camera movement when hovering over the keypad

        // Get horizontal mouse movement
        float mouseX = Input.GetAxis("Mouse X");

        // Calculate the desired rotation angle
        float desiredAngle = currentAngle + mouseX * sensitivity;

        // Clamp the desired angle within the allowed range
        desiredAngle = Mathf.Clamp(desiredAngle, 0f, maxAngle);

        // Apply the rotation to the camera
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x, 
            startAngle + desiredAngle, 
            transform.localEulerAngles.z
        );

        // Update the current angle to reflect the applied rotation
        currentAngle = desiredAngle;
    }

    /// <summary>
    /// Enables or disables camera movement based on whether the cursor is over the keypad.
    /// </summary>
    /// <param name="isHovering">True if the cursor is hovering over the keypad; otherwise, false.</param>
    public void SetHoveringOverKeypad(bool isHovering)
    {
        isHoveringOverKeypad = isHovering;
    }
}