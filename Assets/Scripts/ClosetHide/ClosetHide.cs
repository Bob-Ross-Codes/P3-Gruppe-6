using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets; // Include the namespace for FirstPersonController
public class ClosetHide : MonoBehaviour

{
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;
    public Transform targetPosition;
    public float animationDelay = 1.0f;
    private bool isHiding = false;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (leftDoorAnimator == null || rightDoorAnimator == null)
        {
            Debug.LogError("Door Animators are not assigned.");
        }

        if (targetPosition == null)
        {
            Debug.LogError("Target Position is not assigned.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isHiding && IsPlayerNear())
        {
            StartCoroutine(HidePlayer());
        }
    }

    private bool IsPlayerNear()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance <= 2.0f;
    }

    private IEnumerator HidePlayer()
    {
        isHiding = true;

        // Play the door animations
        leftDoorAnimator.SetTrigger("isOpen");
        rightDoorAnimator.SetTrigger("isOpen");

        // Wait for the animation to finish
        yield return new WaitForSeconds(animationDelay);

        // Move player to the target position inside the closet
        player.transform.position = targetPosition.position;

        // Get the FirstPersonController component and disable movement
        FirstPersonController playerMovement = player.GetComponent<FirstPersonController>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false; // Disable movement
        }

        // Optional: wait for the player to press 'E' again to exit
        while (!Input.GetKeyDown(KeyCode.E))
        {
            yield return null;
        }

        // Play the door animations again (to close the doors)
        leftDoorAnimator.SetTrigger("isOpen");
        rightDoorAnimator.SetTrigger("isOpen");

        // Wait for the animation to finish
        yield return new WaitForSeconds(animationDelay);

        // Move the player back outside the closet
        player.transform.position = targetPosition.position + Vector3.forward * 2.0f;

        // Enable player movement again
        if (playerMovement != null)
        {
            playerMovement.enabled = true; // Enable movement
        }

        isHiding = false;
    }
}
