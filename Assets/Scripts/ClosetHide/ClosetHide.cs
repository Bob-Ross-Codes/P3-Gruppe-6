using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Include the Cinemachine namespace
using StarterAssets; // for FirstPersonController

public class ClosetHide : MonoBehaviour
{
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera closetCamera;
    public FirstPersonController playerController; // Reference to the FirstPersonController script

    public Animator leftDoorAnimator; // Animator for the left door
    public Animator rightDoorAnimator; // Animator for the right door
    public Transform player; // Reference to the player's transform
    public float interactionRange = 2.0f; // Set the interaction range

    private bool isHiding = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isNearCloset()) // Check if player is near the closet
            {
                ToggleHiding();
            }
        }
    }

    private void ToggleHiding()
    {
        isHiding = !isHiding;

        if (isHiding)
        {
            // Switch to ClosetCamera, disable player movement, and open the doors
            mainCamera.Priority = 0;
            closetCamera.Priority = 1;
            playerController.enabled = false;

            leftDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the left door
            rightDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the right door
        }
        else
        {
            // Return to MainCamera, enable player movement, and close the doors
            mainCamera.Priority = 1;
            closetCamera.Priority = 0;
            playerController.enabled = true;

           leftDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the left door
            rightDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the right door
        }
    }

    private bool isNearCloset()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        return distanceToPlayer <= interactionRange;
    }
}
