using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    // Maximum swing angle (degrees)
    public float swingAngle = 30f;
    
    // Speed of the swinging motion
    public float swingSpeed = 2f;

    // Maximum twist angle (degrees)
    public float twistAngle = 10f;

    // Speed of the twisting motion
    public float twistSpeed = 1f;

    // Swing axis (around which the light will swing)
    public Vector3 swingAxis = Vector3.forward;

    // Twist axis (around which the light will twist)
    public Vector3 twistAxis = Vector3.up;

    // Starting rotation of the light
    private Quaternion initialRotation;

    void Start()
    {
        // Store the initial rotation of the object
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Calculate the swing angle using a sine wave over time
        float swing = Mathf.Sin(Time.time * swingSpeed) * swingAngle;

        // Calculate the twist angle using a different sine wave over time
        float twist = Mathf.Sin(Time.time * twistSpeed) * twistAngle;

        // Apply both the swing and twist to the local rotation, relative to its original position
        Quaternion swingRotation = Quaternion.AngleAxis(swing, swingAxis);
        Quaternion twistRotation = Quaternion.AngleAxis(twist, twistAxis);

        // Combine the two rotations (swing and twist) and apply to the object's local rotation
        transform.localRotation = initialRotation * swingRotation * twistRotation;
    }
}


