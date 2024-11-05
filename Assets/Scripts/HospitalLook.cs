using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // Include the Cinemachine namespace
using StarterAssets; // for FirstPersonController

public class HospitalLook : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera mainCamera;
    [SerializeField] private CinemachineVirtualCamera doorCamera;
    public FirstPersonController playerController; // Reference to the FirstPersonController script
    public Transform player; // Reference to the player's transform
    public float interactionRange = 2.0f; // Set the interaction range
    private bool isHiding = false;

    void Start()
    {
        mainCamera.gameObject.SetActive(true);
        doorCamera.gameObject.SetActive(false);
    }

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
            // Switch to DoorCamera, disable player movement
            mainCamera.gameObject.SetActive(false);
            doorCamera.gameObject.SetActive(true);
            playerController.enabled = false;
        }
        else
        {
            // Return to MainCamera, enable player movement
            mainCamera.gameObject.SetActive(true);
            doorCamera.gameObject.SetActive(false);
            playerController.enabled = true;
        }
    }

    private bool isNearCloset()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        return distanceToPlayer <= interactionRange;
    }
}