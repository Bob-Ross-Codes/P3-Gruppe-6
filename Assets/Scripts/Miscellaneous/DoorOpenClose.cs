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
   

    void Update()
    {
        CheckPlayerDistance();

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
             doorAnimator.SetTrigger("Toggle");
              
        }

       
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




