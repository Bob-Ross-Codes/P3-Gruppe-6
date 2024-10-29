using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenClose : MonoBehaviour
{
    public float interactionRange = 5f; // Range within which the player can interact with the door
    public Transform player; // Reference to the player's transform
    public Animator doorAnimator; // Reference to the door's Animator
    private bool isOpen = false; // Track whether the door is open or closed
    private bool isInRange = false;
    private bool canPress = true; // Track if player can press the key

    void Update()
    {
        CheckPlayerDistance();

        if (isInRange && Input.GetKeyDown(KeyCode.E) && canPress)
        {
            StartCoroutine(ToggleDoorWithDelay());
        }
    }

    // Check if the player is within interaction range
    void CheckPlayerDistance()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= interactionRange)
        {
            isInRange = true;
            // Display UI text like "Press E to Open/Close"
        }
        else
        {
            isInRange = false;
            // Hide UI text
        }
    }

    // Toggle the door between open and closed states with a delay
    IEnumerator ToggleDoorWithDelay()
    {
        canPress = false; // Disable key presses

        if (isOpen)
        {
            doorAnimator.SetTrigger("Open");
            isOpen = false;
        }
        else
        {
            doorAnimator.SetTrigger("Close");
            isOpen = true;
        }

        // Wait for 1 second before allowing the next key press
        yield return new WaitForSeconds(0.5f);
        canPress = true; // Re-enable key presses after delay
    }
}




