using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the swinging and twisting motion of a GameObject by applying randomized angles and speeds.
/// The object will swing around a specified axis and twist around another axis, creating dynamic and varied movements.
/// </summary>
public class Swing_simon : MonoBehaviour
{
    [Header("Swing Angle Settings")]
    [Tooltip("Minimum angle (in degrees) for the swinging motion.")]
    [SerializeField, Range(1f, 30f)] private float swingAngleMin = 5f;

    [Tooltip("Maximum angle (in degrees) for the swinging motion.")]
    [SerializeField, Range(1f, 30f)] private float swingAngleMax = 15f;

    [Header("Swing Speed Settings")]
    [Tooltip("Minimum speed for the swinging motion.")]
    [SerializeField, Range(1f, 30f)] private float swingSpeedMin = 1f;

    [Tooltip("Maximum speed for the swinging motion.")]
    [SerializeField, Range(1f, 30f)] private float swingSpeedMax = 5f;

    [Header("Twist Angle Settings")]
    [Tooltip("Minimum angle (in degrees) for the twisting motion.")]
    [SerializeField, Range(1f, 30f)] private float twistAngleMin = 5f;

    [Tooltip("Maximum angle (in degrees) for the twisting motion.")]
    [SerializeField, Range(1f, 30f)] private float twistAngleMax = 15f;

    [Header("Twist Speed Settings")]
    [Tooltip("Minimum speed for the twisting motion.")]
    [SerializeField, Range(1f, 30f)] private float twistSpeedMin = 1f;

    [Tooltip("Maximum speed for the twisting motion.")]
    [SerializeField, Range(1f, 30f)] private float twistSpeedMax = 5f;

    [Header("Axis Settings")]
    [Tooltip("Axis around which the object will swing.")]
    [SerializeField] private Vector3 swingAxis = Vector3.forward;

    [Tooltip("Axis around which the object will twist.")]
    [SerializeField] private Vector3 twistAxis = Vector3.up;

    // Starting rotation of the object to maintain the original orientation
    private Quaternion initialRotation;

    // Current swing and twist angles and speeds
    private float swingAngle;
    private float swingSpeed;
    private float twistAngle;
    private float twistSpeed;

    /// <summary>
    /// Initializes the object's rotation and sets randomized swing and twist angles and speeds within specified ranges.
    /// </summary>
    void Start()
    {
        // Store the initial rotation of the object
        initialRotation = transform.localRotation;

        // Assign random values within the specified ranges for swing and twist
        swingAngle = Random.Range(swingAngleMin, swingAngleMax);
        swingSpeed = Random.Range(swingSpeedMin, swingSpeedMax);
        twistAngle = Random.Range(twistAngleMin, twistAngleMax);
        twistSpeed = Random.Range(twistSpeedMin, twistSpeedMax);
    }

    /// <summary>
    /// Updates the object's rotation each frame to create swinging and twisting motions.
    /// The swing and twist are calculated using sine waves for smooth, oscillating movement.
    /// </summary>
    void Update()
    {
        // Calculate the current swing angle using a sine wave based on time and swing speed
        float swing = Mathf.Sin(Time.time * swingSpeed) * swingAngle;

        // Calculate the current twist angle using a sine wave based on time and twist speed
        float twist = Mathf.Sin(Time.time * twistSpeed) * twistAngle;

        // Create rotation quaternions for swinging and twisting around their respective axes
        Quaternion swingRotation = Quaternion.AngleAxis(swing, swingAxis);
        Quaternion twistRotation = Quaternion.AngleAxis(twist, twistAxis);

        // Combine the initial rotation with the swing and twist rotations
        transform.localRotation = initialRotation * swingRotation * twistRotation;
    }
}