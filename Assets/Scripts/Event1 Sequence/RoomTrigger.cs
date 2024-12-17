using UnityEngine;

/// <summary>
/// Disables the door interaction script when the player enters the trigger zone.
/// </summary>
public class RoomTrigger : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the DoorOpenClose script attached to the door.")]
    public DoorOpenClose doorScript;

    /// <summary>
    /// Detects when a collider enters the trigger zone.
    /// If the collider is the player, it disables the door interaction script.
    /// </summary>
    /// <param name="other">The collider that entered the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger zone
        if (other.CompareTag("Player"))
        {
            // Ensure the door script is assigned
            if (doorScript != null)
            {
                // Disable the door interaction script
                doorScript.enabled = false;
                Debug.Log("Door interaction disabled.");
            }
            else
            {
                Debug.LogWarning("Door script is not assigned.");
            }
        }
    }
}