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
    public float interactionRange = 1.0f; // Set the interaction range
    private bool isHiding = false;
    [SerializeField] private GameObject note;
    private Vector3 initialScaleNote;
    private Vector3 initialPositionNote;
    private Vector3 noteTransformation = new Vector3(0.119999997f, -0.0799999982f, 0.317999989f);
    [SerializeField] private PictureOpen pictureOpen;

    [SerializeField] private GameObject capsule;

    void Start()
    {
        mainCamera.gameObject.SetActive(true);
        doorCamera.gameObject.SetActive(false);
        initialScaleNote = note.transform.localScale;
        initialPositionNote = note.transform.localPosition;
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
    private IEnumerator Note()
    {
        yield return new WaitForSeconds(0.5f);
        note.transform.localScale = initialScaleNote / 2.5f;
        note.transform.localPosition = noteTransformation;
    }
    private void ToggleHiding()
    {
        isHiding = !isHiding;

        if (isHiding)
        {
            pictureOpen.simulateKeyPress = true;
            capsule.SetActive(false);
            // Switch to DoorCamera, disable player movement
            mainCamera.gameObject.SetActive(false);
            doorCamera.gameObject.SetActive(true);
            playerController.MoveSpeed = 0;
            StartCoroutine(Note());
        }
        else
        {
            capsule.SetActive(true);
            // Return to MainCamera, enable player movement
            mainCamera.gameObject.SetActive(true);
            doorCamera.gameObject.SetActive(false);
            playerController.MoveSpeed = 3.5f;
            note.transform.localScale = initialScaleNote;
            note.transform.localPosition = initialPositionNote;
        }
    }

    private bool isNearCloset()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        return distanceToPlayer <= interactionRange;
    }
}