using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GazeActivation : MonoBehaviour
{
    private float lookTime;
    private bool isActivated = false;

    // Abstract property for activation time
    public abstract float ActivationTime { get; }

    public abstract void OnLookedAt();

    public void UpdateLookTime(float deltaTime)
    {
        //if (isActivated) return; // If already activated, do nothing

        lookTime += deltaTime; // Accumulate look time

        if (lookTime >= ActivationTime) // Check if the look time exceeds the threshold
        {
            OnLookedAt();
            isActivated = true; // Mark as activated
            lookTime = 0.0f; // Reset the look time after activation
        }
    }

    public void ResetLookTime()
    {
        lookTime = 0.0f;
       
    }
}
