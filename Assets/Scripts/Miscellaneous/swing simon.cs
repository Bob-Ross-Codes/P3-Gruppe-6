using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSimon : MonoBehaviour
{
    // Range for the maximum swing angle (degrees)
    [SerializeField, Range(1f, 10f)] private float swingAngleMin;
    [SerializeField, Range(1f, 10f)] private float swingAngleMax;

    // Range for the speed of the swinging motion
    [SerializeField, Range(1f, 6f)] private float swingSpeedMin;
    [SerializeField, Range(1f, 6f)] private float swingSpeedMax;

    // Range for the maximum twist angle (degrees)
    [SerializeField, Range(1f, 10f)] private float twistAngleMin;
    [SerializeField, Range(1f, 10f)] private float twistAngleMax;

    // Range for the speed of the twisting motion
    [SerializeField, Range(1f, 6f)] private float twistSpeedMin;
    [SerializeField, Range(1f, 6f)] private float twistSpeedMax;

    // Swing axis (around which the light will swing)
    [SerializeField] private Vector3 swingAxis = Vector3.forward;

    // Twist axis (around which the light will twist)
    [SerializeField] private Vector3 twistAxis = Vector3.up;

    // Starting rotation of the light
    private Quaternion initialRotation;

    // Actual swing and twist values
    private float swingAngle;
    private float swingSpeed;
    private float twistAngle;
    private float twistSpeed;

    void Start()
    {
        // Store the initial rotation of the object
        initialRotation = transform.localRotation;

        // Apply random values to the swing and twist angles and speeds within the specified ranges
        swingAngle = Random.Range(swingAngleMin, swingAngleMax);
        swingSpeed = Random.Range(swingSpeedMin, swingSpeedMax);
        twistAngle = Random.Range(twistAngleMin, twistAngleMax);
        twistSpeed = Random.Range(twistSpeedMin, twistSpeedMax);
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