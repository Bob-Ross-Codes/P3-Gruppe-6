using UnityEngine;

public class WwiseFirstPersonFootsteps: MonoBehaviour
{
    [Header("Wwise Settings")]
    [SerializeField] private string FootstepsEventPath;    // Use this in the Editor to select our Footsteps Event.
    [SerializeField] private string JumpingEventPath;      // Use this in the Editor to select our Jumping Event.
    [SerializeField] private string MaterialParameterName; // The name of the Wwise parameter that controls which material the player is walking on.
    [SerializeField] private string SpeedParameterName;    // The name of the Wwise parameter that controls the speed of the footsteps.
    [SerializeField] private string JumpOrLandParameterName; // The name of the Wwise parameter for jumping or landing sounds.

    [Header("Playback Settings")]
    [SerializeField] private float StepDistance = 2.0f;    // How far the player must travel before they hear a footstep.
    [SerializeField] private float RayDistance = 1.2f;     // How far the raycast will travel down to check for a floor.
    [SerializeField] private float StartRunningTime = 0.3f; // The time between steps for running footsteps.
    [SerializeField] private string JumpInputName = "Jump"; // Name of the input for jumping (usually "Jump").
    public string[] MaterialTypes;                        // Array of strings representing different material types for footstep sounds.

    [HideInInspector] public int DefaultMaterialValue;     // Default material value if no material is detected.

    private float stepRandom;       // Adds random variation to the step distance.
    private Vector3 prevPos;        // Holds the previous position of the player.
    private float distanceTravelled; // How far the player has traveled since their last step.
    private RaycastHit hit;         // Information about the object hit by the raycast.
    private int materialValue;      // The current material value for Wwise.
    private bool playerTouchingGround; // Whether the player is currently grounded.
    private bool previouslyTouchingGround; // Whether the player was grounded in the previous frame.
    private float timeTakenSinceStep; // Timer for tracking the time between steps.
    private int playerRunning;      // Speed parameter for determining whether the player is running or walking.

    private void Start()
    {
        stepRandom = Random.Range(0f, 0.5f);   // Randomize the step distance.
        prevPos = transform.position;          // Set the player's initial position.
    }

    private void Update()
    {
        // Draws a debug ray to visualize the ground check.
        Debug.DrawRay(transform.position, Vector3.down * RayDistance, Color.blue);

        // Ground check and jumping/landing sound logic.
        GroundedCheck();
        if (playerTouchingGround && Input.GetButtonDown(JumpInputName))
        {
            MaterialCheck();
            PlayJumpOrLand(true); // Play jump sound.
        }
        if (!previouslyTouchingGround && playerTouchingGround)
        {
            MaterialCheck();
            PlayJumpOrLand(false); // Play landing sound.
        }
        previouslyTouchingGround = playerTouchingGround;

        // Footstep sound logic based on movement.
        timeTakenSinceStep += Time.deltaTime;
        distanceTravelled += (transform.position - prevPos).magnitude;

        if (distanceTravelled >= StepDistance + stepRandom)
        {
            MaterialCheck();
            SpeedCheck();
            PlayFootstep();
            stepRandom = Random.Range(0f, 0.5f); // Randomize the next step distance.
            distanceTravelled = 0f;              // Reset the traveled distance.
        }
        prevPos = transform.position;            // Update the previous position.
    }

    private void MaterialCheck()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RayDistance))
        {
            // Check if the hit object has a material component or use the default value.
            WwiseMaterialSetter materialSetter = hit.collider.gameObject.GetComponent<WwiseMaterialSetter>();
            if (materialSetter != null)
            {
                materialValue = materialSetter.MaterialValue;
            }
            else
            {
                materialValue = DefaultMaterialValue;
            }
        }
        else
        {
            materialValue = DefaultMaterialValue; // Use default if no material is found.
        }
    }

    private void SpeedCheck()
    {
        if (timeTakenSinceStep < StartRunningTime)
        {
            playerRunning = 1; // Running footsteps.
        }
        else
        {
            playerRunning = 0; // Walking footsteps.
        }
        timeTakenSinceStep = 0f; // Reset step timer.
    }

    private void GroundedCheck()
    {
        Physics.Raycast(transform.position, Vector3.down, out hit, RayDistance);
        playerTouchingGround = hit.collider != null; // True if the raycast hit something.
    }

    private void PlayFootstep()
    {
        if (playerTouchingGround)
        {
            AkSoundEngine.PostEvent(FootstepsEventPath, gameObject);
            AkSoundEngine.SetRTPCValue(MaterialParameterName, materialValue, gameObject);
            AkSoundEngine.SetRTPCValue(SpeedParameterName, playerRunning, gameObject);
        }
    }

    private void PlayJumpOrLand(bool isJumping)
    {
        AkSoundEngine.PostEvent(JumpingEventPath, gameObject);
        AkSoundEngine.SetRTPCValue(MaterialParameterName, materialValue, gameObject);
        AkSoundEngine.SetRTPCValue(JumpOrLandParameterName, isJumping ? 0 : 1, gameObject);
    }
}
