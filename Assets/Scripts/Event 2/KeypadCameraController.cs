using UnityEngine;

public class KeypadCameraController : MonoBehaviour
{
    public float sensitivity = 0.1f; // Sensitivity of camera movement
    public float maxAngle = 30f; // Maximum angle the camera can rotate to the right
    private float currentAngle = 0f; // Current rotation angle of the camera
    private float startAngle;

    private bool isHoveringOverKeypad = false; // Track if the cursor is over the keypad
    private bool isHoveringOverPassword = false; // Track if the cursor is over the password

    // Layer mask for selecting what to raycast against (e.g., only the keypad and password)
    public LayerMask interactableLayer;

    public KeypadController keypadController; // Reference to the keypad controller to trigger password change

    void Start()
    {
        // Store the initial angle to ensure relative rotation
        startAngle = transform.localEulerAngles.y;
    }

    void Update()
    {
        // Check for raycast hover detection before moving the camera
        CheckMouseHover();

        if (isHoveringOverKeypad) return; // Do not move the camera if hovering over the keypad or password

        // Get the mouse movement (to rotate the camera)
        float mouseX = Input.GetAxis("Mouse X");

        // Calculate the desired angle and clamp it to the allowed range
        float desiredAngle = currentAngle + mouseX * sensitivity;
        desiredAngle = Mathf.Clamp(desiredAngle, 0f, maxAngle);

        // Apply the rotation based on the mouse movement
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, startAngle + desiredAngle, transform.localEulerAngles.z);

        // Update the current angle
        currentAngle = desiredAngle;

        // Check if the camera is hovering over the password and change it if needed
        if (isHoveringOverPassword)
        {
            ChangePassword();
        }

         if (isHoveringOverKeypad)
        {
            ChangePicture();
        }
    }

    // Method to check if the mouse is hovering over the keypad or password using raycasting
    void CheckMouseHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast a ray to detect if it hits an object in the interactable layer
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            // Check if the raycast hits the keypad (based on its tag)
            if (hit.collider.CompareTag("Keypad"))
            {
                isHoveringOverKeypad = true; // Set flag if the mouse is over the keypad
            }
            else
            {
                isHoveringOverKeypad = false; // Reset flag if the mouse is not over the keypad
            }

            // Check if the raycast hits the password (based on its tag)
            if (hit.collider.CompareTag("Password"))
            {
                isHoveringOverPassword = true; // Set flag if the mouse is over the password
            }
            else
            {
                isHoveringOverPassword = false; // Reset flag if the mouse is not over the password
            }
        }
        else
        {
            isHoveringOverKeypad = false; // Reset flag if the raycast doesn't hit anything
            isHoveringOverPassword = false; // Reset flag if the raycast doesn't hit anything
        }
    }

    // Method to change the password (triggered when hovering over the password)
    void ChangePassword()
    {
        // Call the method in the keypad controller to change the password
        keypadController.LookAtPassword();

        // Optionally, log or perform any additional actions when the password changes
        Debug.Log("Password has been changed!");
    }

    void ChangePicture()
    {
        // Call the method in the keypad controller to change the password
        keypadController.LookAtKeypad();

        // Optionally, log or perform any additional actions when the password changes
        Debug.Log("Password has been changed!");
    }

    // Methods to enable/disable camera movement (if needed)
    public void SetHoveringOverKeypad(bool isHovering)
    {
        isHoveringOverKeypad = isHovering;
    }

    public void SetHoveringOverPassword(bool isHovering)
    {
        isHoveringOverPassword = isHovering;
    }
}
