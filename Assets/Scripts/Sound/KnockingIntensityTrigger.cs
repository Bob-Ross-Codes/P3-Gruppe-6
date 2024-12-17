using UnityEngine;
using AK.Wwise;

/// <summary>
/// Controls the intensity of a knocking sound effect based on the player's presence in the room
/// and their interaction with the environment. The knocking intensity increases over time while
/// the player remains in the trigger area, and stops when the player hides in a closet.
/// </summary>
public class KnockingIntensityTrigger : MonoBehaviour
{
    [Header("Wwise Settings")]
    [Tooltip("The Wwise RTPC (Real-Time Parameter Control) for adjusting the knocking intensity.")]
    public AK.Wwise.RTPC knockingIntensityParameter;

    [Header("Intensity Settings")]
    [Tooltip("The rate at which the knocking intensity increases.")]
    public float intensityIncreaseRate = 10.0f;

    [Tooltip("Animator controlling the knocking animation.")]
    [SerializeField] private Animator animator;

    private float currentIntensity = 0; // Current intensity level of the knocking sound
    private bool isPlayerInside = false; // Tracks if the player is inside the trigger area
    private ClosetHide closetHide; // Reference to the ClosetHide script

    /// <summary>
    /// Finds and initializes the ClosetHide reference at the start of the game.
    /// </summary>
    void Start()
    {
        closetHide = FindObjectOfType<ClosetHide>(); // Locate the ClosetHide script in the scene
    }

    /// <summary>
    /// Called when a collider enters the trigger zone. Starts the knocking effect for the player.
    /// </summary>
    /// <param name="other">The collider entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            currentIntensity = 0; // Reset the knocking intensity
            knockingIntensityParameter.SetGlobalValue(currentIntensity); // Set the initial RTPC value
        }
    }

    /// <summary>
    /// Called when a collider exits the trigger zone. Resets the knocking effect when the player leaves.
    /// </summary>
    /// <param name="other">The collider exiting the trigger zone.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            currentIntensity = 0; // Reset the knocking intensity
            knockingIntensityParameter.SetGlobalValue(currentIntensity); // Reset the RTPC value
        }
    }

    /// <summary>
    /// Updates the knocking intensity while the player is in the trigger zone.
    /// Stops the knocking if the player hides in the closet.
    /// </summary>
    private void Update()
    {
        if (isPlayerInside)
        {
            // Increase the knocking intensity over time
            currentIntensity += intensityIncreaseRate * Time.deltaTime;
            currentIntensity = Mathf.Clamp(currentIntensity, 0, 30); // Clamp the intensity to a maximum value
            knockingIntensityParameter.SetGlobalValue(currentIntensity); // Update the RTPC value
            animator.SetTrigger("Knocking"); // Trigger the knocking animation

            // Check if the player is hiding in the closet
            if (closetHide != null && closetHide.isHiding)
            {
                Destroy(gameObject); // Destroy this script's GameObject

                // Stop the knocking sound
                AkSoundEngine.PostEvent("Stop_Knocking_Event", gameObject);
            }
        }
    }
}