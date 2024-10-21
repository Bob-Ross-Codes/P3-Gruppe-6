using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetHide : MonoBehaviour
{
    public Animator leftDoorAnimator; // Animator for the left door
    public Animator rightDoorAnimator; // Animator for the right door
    public Transform hidePosition; // The target position for the player camera
    public float cameraTransitionSpeed = 2f; // Speed at which the camera moves
    public KeyCode hideKey = KeyCode.E; // The key to trigger hiding

    private Transform playerCamera; // Reference to the player's camera
    private bool isHiding = false;
    private bool canHide = false;

    private void Start()
    {
        playerCamera = Camera.main.transform;
    }

    private void Update()
    {
        if (canHide && Input.GetKeyDown(hideKey))
        {
            ToggleHide();
        }

        // Move the camera to the target position if hiding
        if (isHiding)
        {
            playerCamera.position = Vector3.Lerp(playerCamera.position, hidePosition.position, Time.deltaTime * cameraTransitionSpeed);
        }
    }

    private void ToggleHide()
    {
        isHiding = !isHiding;

        // Play door animations based on hiding state
        leftDoorAnimator.SetBool("isOpen", isHiding);
        rightDoorAnimator.SetBool("isOpen", isHiding);

        if (!isHiding)
        {
            // If the player stops hiding, return the camera to its original position
            playerCamera.position = transform.position; // Adjust this as needed to return the camera
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canHide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canHide = false;
        }
    }
}