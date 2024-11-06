using UnityEngine;
using AK.Wwise;

public class KnockingIntensityTrigger : MonoBehaviour
{
    
    private ClosetHide closetHide; // Reference to the ClosetHide script
    public AK.Wwise.RTPC knockingIntensityParameter; // Game Parameter for intensity control
    public float intensityIncreaseRate = 10.0f;      // Rate of intensity increase

    private float currentIntensity = 0;              // Current intensity level
    private bool isPlayerInside = false;             // Track if player is in the room

    void Start()
    {
        closetHide = FindObjectOfType<ClosetHide>(); // Find the ClosetHide script
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            currentIntensity = 0; // Initialize intensity at 0
            knockingIntensityParameter.SetGlobalValue(currentIntensity); // Start the parameter at 0
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            currentIntensity = 0; // Reset intensity to 0 on exit
            knockingIntensityParameter.SetGlobalValue(currentIntensity); // Reset the parameter
        }
    }

    private void Update()
    {
        // Only increase the parameter while the player is in the room
        if (isPlayerInside)
        {
            currentIntensity += intensityIncreaseRate * Time.deltaTime;
            currentIntensity = Mathf.Clamp(currentIntensity, 0, 100); // Cap at 100
            knockingIntensityParameter.SetGlobalValue(currentIntensity); // Update the parameter value
            if (closetHide != null && closetHide.isHiding)
            {
                Destroy(gameObject);

                // Stop the knocking sound when the player hides in the closet after 2 seconds
                
                AkSoundEngine.PostEvent("Stop_Knocking_Event", gameObject);
            }
        }
    }
}
