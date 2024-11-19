using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Include the Cinemachine namespace
using StarterAssets; // For FirstPersonController

public class ClosetHide : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera closetCamera;
    public FirstPersonController playerController; // Reference to the FirstPersonController script
    public GameObject objectToDisableHallwayOne; // Reference to the GameObject to disable when hiding
    public GameObject objectToEnableHallwayOne;
    public Transform roomDoor;

    public Animator leftDoorAnimator; // Animator for the left door
    public Animator rightDoorAnimator; // Animator for the right door
    public Animator doorHingeAnimator;
    public Transform player; // Reference to the player's transform
    public float interactionRange = 2.0f; // Set the interaction range
    public MonsterSequenceController sequenceController; // Reference to the MonsterSequenceController script

    public DeAgro deAgroScript; // Reference to the DeAgro script
    public bool isHiding = false;
    public bool canToggleHiding = true; // Flag to control hiding
    private bool hasEnteredClosetOnce = false; // Tracks if the player has entered the closet at least once

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canToggleHiding)
        {
            if (isNearCloset()) // Check if player is near the closet
            {
                ToggleHiding();

                if (!hasEnteredClosetOnce)
                {
                    // Disable hiding after the first entry
                    hasEnteredClosetOnce = true;
                    canToggleHiding = false; // Lock hiding until the monster is destroyed
                }
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
            playerController.MoveSpeed = 0;

            leftDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the left door
            rightDoorAnimator.SetTrigger("ToggleDoor"); // Play open-close animation for the right door

            objectToDisableHallwayOne.SetActive(false);

            doorHingeAnimator.SetTrigger("broken");
            sequenceController.OnPlayerHidden();
            Debug.Log("Player is hiding in the closet");
        }
        else
        {
            player.LookAt(roomDoor);
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

    public IEnumerator WaitForMonsterToBeDestroyed()
    {
        Debug.Log("Hiding locked. Waiting for monster to be destroyed.");

        // Wait until the monsterPrefab is destroyed (null)
        while (deAgroScript != null && deAgroScript.monsterPrefab != null)
        {
            yield return null; // Wait for the next frame
        }

        // Once the monster is destroyed, allow the player to leave
        Debug.Log("Monster destroyed. Hiding unlocked.");
        AkSoundEngine.PostEvent("Stop_Monster_Sounds", gameObject);
        canToggleHiding = true; // Set canToggleHiding to true
    }
}



