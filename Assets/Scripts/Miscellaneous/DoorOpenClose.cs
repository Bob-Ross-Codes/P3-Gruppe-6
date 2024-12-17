using UnityEngine;

/// <summary>
/// Manages door interactions, including opening, closing, and sound effects.
/// Tracks player proximity to determine interaction range and handles locked door behavior.
/// </summary>
public class DoorOpenClose : MonoBehaviour
{
    [Header("Door Settings")]
    [Tooltip("Range within which the player can interact with the door.")]
    public float interactionRange = 5f;

    [Tooltip("Reference to the player's transform.")]
    public Transform player;

    [Tooltip("Reference to the door's Animator.")]
    public Animator doorAnimator;

    [Tooltip("Determines whether the door is locked.")]
    public bool isLocked = false;

    private bool isOpen = false; // Tracks whether the door is open or closed
    private bool isInRange = false; // Tracks whether the player is within interaction range

    /// <summary>
    /// Updates player interaction and handles door state changes.
    /// Checks the player's distance from the door and responds to input.
    /// </summary>
    void Update()
    {
        CheckPlayerDistance();

        // Handle interaction when player is in range and presses 'E'
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!isLocked)
            {
                doorAnimator.SetTrigger("Toggle");
                doorAnimator.SetBool("IsOpen", true);
            }
            else
            {
                PlayLockedDoorSound();
            }
        }
    }

    /// <summary>
    /// Resets the door toggle state, setting it to closed.
    /// Typically called during or after the closing animation.
    /// </summary>
    public void ResetDoorToggle()
    {
        doorAnimator.SetBool("IsOpen", false);
    }

    /// <summary>
    /// Plays the sound effect for opening the door.
    /// Sets the Wwise RTPC value for "open" state.
    /// </summary>
    public void PlayOpenSound()
    {
        AkSoundEngine.SetRTPCValue("RTPC_DoorState", 0); // 0 for open
        AkSoundEngine.PostEvent("Door_SFX_Event", gameObject);
    }

    /// <summary>
    /// Plays the sound effect for closing the door.
    /// Sets the Wwise RTPC value for "closed" state.
    /// </summary>
    public void PlayCloseSound()
    {
        AkSoundEngine.SetRTPCValue("RTPC_DoorState", 1); // 1 for closed
        AkSoundEngine.PostEvent("Door_SFX_Event", gameObject);
    }

    /// <summary>
    /// Checks the player's distance to the door and updates the interaction range status.
    /// </summary>
    void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        isInRange = distanceToPlayer <= interactionRange;
    }

    /// <summary>
    /// Plays the sound effect for a locked door.
    /// Sets the Wwise RTPC value for "locked" state.
    /// </summary>
    private void PlayLockedDoorSound()
    {
        AkSoundEngine.SetRTPCValue("RTPC_DoorState", 2); // 2 for locked
        AkSoundEngine.PostEvent("Door_SFX_Event", gameObject);
    }
}