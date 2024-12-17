using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Handles player movement and camera control using mouse and keyboard input.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class Controller : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f; // Player movement speed
    [SerializeField] private float mouseSensitivity = 100f; // Mouse sensitivity for looking around

    [Header("Camera Setup")]
    [SerializeField] private Transform playerBody; // The player's body transform
    [SerializeField] private CinemachineVirtualCamera virtualCamera; // Cinemachine virtual camera

    [Header("State")]
    public bool isPaused = false; // Tracks whether the game is paused

    private CharacterController characterController; // Reference to the CharacterController component
    private float xRotation = 0f; // Tracks vertical camera rotation

    /// <summary>
    /// Initializes components and locks the cursor.
    /// </summary>
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        // Set Cinemachine camera follow and look-at targets
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerBody;
            virtualCamera.LookAt = playerBody;
        }
    }

    /// <summary>
    /// Handles player movement and camera rotation each frame.
    /// </summary>
    private void Update()
    {
        if (!isPaused)
        {
            HandleMouseLook();
            HandleMovement();
        }
    }

    /// <summary>
    /// Processes mouse input to rotate the camera and player body.
    /// </summary>
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Adjust vertical rotation and clamp it
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply rotations
        playerBody.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    /// <summary>
    /// Handles player movement based on keyboard input.
    /// </summary>
    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal"); // Left-right movement
        float moveZ = Input.GetAxis("Vertical");   // Forward-backward movement

        // Calculate movement direction
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Apply movement to the character controller
        characterController.Move(move * speed * Time.deltaTime);
    }
}