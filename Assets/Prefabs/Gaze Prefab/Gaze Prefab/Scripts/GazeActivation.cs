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
        if (isActivated) return; // Check if already activated

        lookTime += deltaTime;
        if (lookTime >= ActivationTime)
        {
            OnLookedAt();
            isActivated = true; // Mark as activated
            lookTime = 0.0f;
        }
    }

    public void ResetLookTime()
    {
        lookTime = 0.0f;
        isActivated = false; // Reset activation status if needed
    }
}
