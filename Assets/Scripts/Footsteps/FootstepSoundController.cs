using UnityEngine;

/// <summary>
/// Controls the playback of footstep sounds using Wwise. 
/// Handles sound events, ground type detection, and wetness levels for dynamic footstep audio.
/// Integrates with player movement, hiding mechanics, and ground properties.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class FootstepSoundController : MonoBehaviour
{
    [Header("Wwise Settings")]
    [Tooltip("Name of the Wwise event for footstep sounds.")]
    [SerializeField] private string footstepEvent = "Footstep";

    [Tooltip("Name of the Wwise RTPC for player speed.")]
    [SerializeField] private string speedRTPC = "PlayerSpeed";

    [Tooltip("Name of the Wwise RTPC for ground wetness.")]
    [SerializeField] private string wetnessRTPC = "GroundWetness";

    [Tooltip("Name of the Wwise switch group for ground type.")]
    [SerializeField] private string groundTypeSwitchGroup = "GroundType";

    [Header("Gameplay Settings")]
    [Tooltip("Layers to detect as ground for footstep sounds.")]
    public LayerMask groundLayers;

    [Tooltip("Time interval between consecutive footsteps.")]
    public float stepInterval = 0.5f;

    [Tooltip("Raycast distance to check for the ground beneath the player.")]
    public float rayDistance = 1.5f;

    private float stepTimer = 0f; // Timer for controlling step intervals
    private CharacterController characterController; // Reference to the CharacterController component
    private ClosetHide closetHide; // Reference to the ClosetHide script

    // RTPC and switch variables
    private float playerSpeed; // Player's current movement speed
    private float groundWetness = 0f; // Default wetness value (dry ground)
    private string currentGroundType = "Tile"; // Default ground type

    private bool isHiding; // Tracks if the player is hiding

    /// <summary>
    /// Initializes references to required components.
    /// </summary>
    void Awake()
    {
        closetHide = GetComponent<ClosetHide>();
    }

    /// <summary>
    /// Retrieves the CharacterController component on start.
    /// </summary>
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    /// <summary>
    /// Updates footstep sounds based on player movement and ground detection.
    /// </summary>
    void Update()
    {
        // Check if the player is hiding
        if (closetHide != null)
        {
            isHiding = closetHide.isHiding;
        }

        // Update player speed and send to Wwise RTPC
        playerSpeed = characterController.velocity.magnitude;
        AkSoundEngine.SetRTPCValue(speedRTPC, playerSpeed);

        // Perform a raycast to detect ground type and wetness
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, groundLayers))
        {
            GroundTypeAndWetnessCheck(hit);
        }

        // Handle footstep timing based on movement and grounded state
        if (playerSpeed > 0.1f && characterController.isGrounded)
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; // Reset the timer when player is not moving
        }
    }

    /// <summary>
    /// Checks the ground type and wetness level based on raycast hit.
    /// Updates Wwise switches and RTPC values accordingly.
    /// </summary>
    /// <param name="hit">The RaycastHit result from the ground check.</param>
    private void GroundTypeAndWetnessCheck(RaycastHit hit)
    {
        // Determine ground type based on collider tags
        if (hit.collider.CompareTag("Carpet"))
        {
            currentGroundType = "Carpet";
        }
        else if (hit.collider.CompareTag("Tile"))
        {
            currentGroundType = "Tile";
        }
        else if (hit.collider.CompareTag("Wood"))
        {
            currentGroundType = "Wood";
        }

        // Retrieve ground wetness from a custom script (optional)
        GroundProperties groundProperties = hit.collider.GetComponent<GroundProperties>();
        if (groundProperties != null)
        {
            groundWetness = groundProperties.WetnessLevel;
        }
        else
        {
            groundWetness = 0f; // Default dry ground
        }

        // Update Wwise switch for ground type and RTPC for wetness
        AkSoundEngine.SetSwitch(groundTypeSwitchGroup, currentGroundType, gameObject);
        AkSoundEngine.SetRTPCValue(wetnessRTPC, groundWetness);
    }

    /// <summary>
    /// Plays the footstep sound if the player is not hiding.
    /// </summary>
    private void PlayFootstep()
    {
        if (!isHiding)
        {
            AkSoundEngine.PostEvent(footstepEvent, gameObject);
        }
    }
}