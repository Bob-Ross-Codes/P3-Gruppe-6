using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Include the Cinemachine namespace
using StarterAssets; // For FirstPersonController

/// <summary>
/// Handles player interactions with the hiding mechanic and note transformation in the hospital scene.
/// Switches between cameras when the player hides, adjusts player movement, and modifies the note's appearance.
/// </summary>
public class HospitalLook : MonoBehaviour
{
    [Header("Camera Settings")]
    [Tooltip("Reference to the main gameplay Cinemachine virtual camera.")]
    [SerializeField] private CinemachineVirtualCamera mainCamera;

    [Tooltip("Reference to the door-specific Cinemachine virtual camera when hiding.")]
    [SerializeField] private CinemachineVirtualCamera doorCamera;

    [Header("Player Settings")]
    [Tooltip("Reference to the player's FirstPersonController script to control movement.")]
    public FirstPersonController playerController;

    [Tooltip("Transform of the player GameObject.")]
    public Transform player;

    [Tooltip("The interaction range to detect proximity to a closet.")]
    public float interactionRange = 1.0f;

    private bool isHiding = false; // Tracks if the player is currently hiding

    [Header("Note Settings")]
    [Tooltip("Reference to the note GameObject to be transformed.")]
    [SerializeField] private GameObject note;

    private Vector3 initialScaleNote; // Stores the note's initial scale
    private Vector3 initialPositionNote; // Stores the note's initial position

    [Tooltip("The transformation applied to the note when the player hides.")]
    private Vector3 noteTransformation = new Vector3(0.119999997f, -0.0799999982f, 0.317999989f);

    [Tooltip("Reference to the PictureOpen script for simulated key press behavior.")]
    [SerializeField] private PictureOpen pictureOpen;

    [Header("Closet Settings")]
    [Tooltip("The capsule GameObject to enable/disable when hiding.")]
    [SerializeField] private GameObject capsule;

    /// <summary>
    /// Initializes camera states, note position, and scale at the start of the scene.
    /// </summary>
    void Start()
    {
        mainCamera.gameObject.SetActive(true);
        doorCamera.gameObject.SetActive(false);

        // Store the initial scale and position of the note
        initialScaleNote = note.transform.localScale;
        initialPositionNote = note.transform.localPosition;
    }

    /// <summary>
    /// Checks for player input to toggle hiding when near a closet.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isNearCloset()) // Check if the player is near the closet
            {
                ToggleHiding();
            }
        }
    }

    /// <summary>
    /// Coroutine to delay and adjust the note's position and scale when the player hides.
    /// </summary>
    private IEnumerator Note()
    {
        yield return new WaitForSeconds(0.5f);
        note.transform.localScale = initialScaleNote / 2.5f; // Reduce the note's scale
        note.transform.localPosition = noteTransformation;  // Move the note to a new position
    }

    /// <summary>
    /// Toggles the hiding state of the player.
    /// Switches cameras, disables/enables movement, and updates the note appearance.
    /// </summary>
    private void ToggleHiding()
    {
        isHiding = !isHiding; // Toggle hiding state

        if (isHiding)
        {
            pictureOpen.simulateKeyPress = true;

            // Hide the capsule and switch to door camera
            capsule.SetActive(false);
            mainCamera.gameObject.SetActive(false);
            doorCamera.gameObject.SetActive(true);

            // Disable player movement
            playerController.MoveSpeed = 0;

            // Adjust the note's appearance
            StartCoroutine(Note());
        }
        else
        {
            // Re-enable the capsule and switch back to the main camera
            capsule.SetActive(true);
            mainCamera.gameObject.SetActive(true);
            doorCamera.gameObject.SetActive(false);

            // Enable player movement
            playerController.MoveSpeed = 3.5f;

            // Reset the note's scale and position
            note.transform.localScale = initialScaleNote;
            note.transform.localPosition = initialPositionNote;
        }
    }

    /// <summary>
    /// Checks if the player is within the interaction range of the closet.
    /// </summary>
    /// <returns>True if the player is near the closet, false otherwise.</returns>
    private bool isNearCloset()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        return distanceToPlayer <= interactionRange;
    }
}