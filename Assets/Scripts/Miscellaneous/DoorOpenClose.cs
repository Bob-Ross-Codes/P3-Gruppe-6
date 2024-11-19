using UnityEngine;

public class DoorOpenClose : MonoBehaviour
{
    public float interactionRange = 5f; // Range within which the player can interact with the door
    public Transform player; // Reference to the player's transform
    public Animator doorAnimator; // Reference to the door's Animator
    private bool isOpen = false; // Track whether the door is open or closed
    private bool isInRange = false;
    public bool isLocked = false;

    void Update()
    {
        CheckPlayerDistance();

        if (isInRange && Input.GetKeyDown(KeyCode.E) && !isLocked)
        {
            doorAnimator.SetTrigger("Toggle");
            doorAnimator.SetBool("IsOpen", true);
        }

        if (isInRange && Input.GetKeyDown(KeyCode.E) && isLocked)
        {
            AkSoundEngine.SetRTPCValue("RTPC_DoorState", 2); // 2 for locked
            AkSoundEngine.PostEvent("Door_SFX_Event", gameObject);
        }
    }


    public void ResetDoorToggle()
    {
        doorAnimator.SetBool("IsOpen", false);
    }

    public void PlayOpenSound()
    {
        AkSoundEngine.SetRTPCValue("RTPC_DoorState", 0); // 0 for open
        AkSoundEngine.PostEvent("Door_SFX_Event", gameObject);
    }

    // Called at the start of the closing phase of the animation
    public void PlayCloseSound()
    {
        AkSoundEngine.SetRTPCValue("RTPC_DoorState", 1); // 1 for closed
        AkSoundEngine.PostEvent("Door_SFX_Event", gameObject);
    }

    // Check if the player is within interaction range
    void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= interactionRange)
        {
            isInRange = true;
        }
        else
        {
            isInRange = false;
        }
    }



}








