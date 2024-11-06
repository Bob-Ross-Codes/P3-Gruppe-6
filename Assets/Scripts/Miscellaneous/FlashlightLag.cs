using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightLag : MonoBehaviour
{
    public Transform cameraTransform;  // Reference to the camera transform
    public float lagSpeed = 5f;  // Speed of the lag effect

    private Quaternion targetRotation;

    void Start()
    {
        // Initialize the target rotation to the camera's initial rotation
        targetRotation = cameraTransform.rotation;
    }

    void Update()
    {
        // Set the target rotation to the camera's current rotation
        targetRotation = cameraTransform.rotation;

        // Smoothly interpolate the flashlight's rotation towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lagSpeed);
    }
}
