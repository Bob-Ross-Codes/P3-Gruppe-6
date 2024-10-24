using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FootstepSoundController : MonoBehaviour
{
    [Header("Wwise Settings")]
    [SerializeField] private string footstepEvent = "Footstep";  // Wwise footstep event name
    [SerializeField] private string speedRTPC = "PlayerSpeed";   // Wwise RTPC for speed
    [SerializeField] private string wetnessRTPC = "GroundWetness"; // Wwise RTPC for wetness
    [SerializeField] private string groundTypeSwitchGroup = "GroundType"; // Wwise switch group for ground type

    [Header("Gameplay Settings")]
    public LayerMask groundLayers;    // Layers for ground detection
    public float stepInterval = 0.5f; // Time between footsteps
    public float rayDistance = 1.5f;  // Distance for ground check
    private float stepTimer = 0f;
    private CharacterController characterController;

    // Variables to store RTPC and switch values
    private float playerSpeed;
    private float groundWetness = 0f;  // Default dry
    private string currentGroundType = "Carpet";  // Default ground type

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Update player speed based on movement
        playerSpeed = characterController.velocity.magnitude;
        AkSoundEngine.SetRTPCValue(speedRTPC, playerSpeed);

        // Raycast to detect ground type and wetness
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance, groundLayers))
        {
            GroundTypeAndWetnessCheck(hit);
        }

        // Step timer logic
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
            stepTimer = 0f;
        }
    }

    private void GroundTypeAndWetnessCheck(RaycastHit hit)
    {
        // Example logic for ground type switch (can be expanded)
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

        // Example logic for wetness (can be expanded)
        // Assuming ground wetness is based on a property in a custom script
        GroundProperties groundProperties = hit.collider.GetComponent<GroundProperties>();
        if (groundProperties != null)
        {
            groundWetness = groundProperties.WetnessLevel;
        }
        else
        {
            groundWetness = 0f;  // Default dry if no script attached
        }

        // Set Wwise switch for ground type and RTPC for wetness
        AkSoundEngine.SetSwitch(groundTypeSwitchGroup, currentGroundType, gameObject);
        AkSoundEngine.SetRTPCValue(wetnessRTPC, groundWetness);
    }

    private void PlayFootstep()
    {
        AkSoundEngine.PostEvent(footstepEvent, gameObject);
    }
}
