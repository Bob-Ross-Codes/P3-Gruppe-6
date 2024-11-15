using UnityEngine;

public class KeypadCameraController : MonoBehaviour
{
    public float sensitivity = 0.1f; // Sensitivity of camera movement
    public float maxAngle = 30f; // Maximum angle the camera can rotate to the right
    private float currentAngle = 0f; // Current rotation angle of the camera
    private float startAngle;

    private bool isHoveringOverKeypad = false; // Track if the cursor is over the keypad

    void Start()
    {
        // Store the initial angle to ensure relative rotation
        startAngle = transform.localEulerAngles.y;
    }

    void Update()
    {
        if (isHoveringOverKeypad) return; // Do not move the camera if hovering over the keypad

        // Get the mouse movement
        float mouseX = Input.GetAxis("Mouse X");

        // Calculate the desired angle
        float desiredAngle = currentAngle + mouseX * sensitivity;

        // Clamp the angle to the allowed range
        desiredAngle = Mathf.Clamp(desiredAngle, 0f, maxAngle);

        // Apply the rotation
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, startAngle + desiredAngle, transform.localEulerAngles.z);

        // Update the current angle
        currentAngle = desiredAngle;
    }

    // Methods to enable/disable camera movement
    public void SetHoveringOverKeypad(bool isHovering)
    {
        isHoveringOverKeypad = isHovering;
    }
}
