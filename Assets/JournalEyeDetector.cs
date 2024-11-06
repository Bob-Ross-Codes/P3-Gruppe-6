using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Mediapipe.CopyCalculatorOptions.Types;

public class JournalEyeDetector : GazeActivation
{
    public override float ActivationTime => 2f;

    public GameObject BedToSpin;

    public float rotationSpeed = 1f;  // Initial rotation speed
    public float accelerationRate = 3f;  // Rate at which rotation speed increases

    public bool spinBed;

    public override void OnLookedAt()
    {
        spinBed = true;
        Debug.Log("spinBed = true");
    }

    private void FixedUpdate()
    {
        if (spinBed)
        {
            // Accelerate the rotation speed over time
            rotationSpeed += accelerationRate * Time.deltaTime;  // Increase rotation speed

            // Calculate the amount to rotate in this frame
            float rotation = rotationSpeed * Time.deltaTime;

            // Apply the rotation to the GameObject
            BedToSpin.transform.Rotate(0f, rotation, 0f);
        }
    }
}
