using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Include the Cinemachine namespace
using StarterAssets; // for FirstPersonController

public class ClosetHide : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera closetCamera;
    public FirstPersonController playerController; // Reference to the FirstPersonController script
    public GameObject objectToDisableHallwayOne; // Reference to the GameObject to disable when hiding
    public GameObject objectToEnableHallwayOne;

    public Animator leftDoorAnimator; // Animator for the left door
    public Animator rightDoorAnimator; // Animator for the right door
    public Animator doorHingeAnimator;
    public Transform player; // Reference to the player's transform
    public float interactionRange = 2.0f; // Set the interaction range
    public MonsterSequenceController sequenceController; // Reference to the MonsterSequenceController script

    // Måske den mest GOATede bool i hele spillet
    public bool isHiding = false;
    private bool canToggleHiding = true; // Flag to control the cooldown

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canToggleHiding)
        {
            if (isNearCloset()) // Check if player is near the closet
            {
                ToggleHiding();
                StartCoroutine(HidingCooldown()); // Start the cooldown coroutine
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
            closetCamera.Priority = 10;
            // the following line sets the first person controller to be speed to be 0
            playerController.MoveSpeed = 0;


            leftDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the left door
            rightDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the right door

            // Disable the specified GameObject (only once)
            objectToDisableHallwayOne.SetActive(false);

            // indset logic der timer det ordenligt

            doorHingeAnimator.SetTrigger("broken");
            // Player entered the closet
            sequenceController.OnPlayerHidden();
            Debug.Log("Player is hiding in the closet");


        }
        else
        {
            // Return to MainCamera, enable player movement, and close the doors
            mainCamera.Priority = 10;
            closetCamera.Priority = 0;

            playerController.MoveSpeed = 3.5f;

            leftDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the left door
            rightDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the right door
        }
    }

    private bool isNearCloset()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        return distanceToPlayer <= interactionRange;
    }

    private IEnumerator HidingCooldown()
    {
        canToggleHiding = false;
        yield return new WaitForSeconds(20);
        canToggleHiding = true;
    }
}

