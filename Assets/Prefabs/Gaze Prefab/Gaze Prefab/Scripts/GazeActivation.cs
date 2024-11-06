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
        lookTime += deltaTime;
        if (lookTime >= ActivationTime)
        {
            OnLookedAt();
            isActivated = true;
            lookTime = 0.0f;
        }
    }

    public void ResetLookTime()
    {
        lookTime = 0.0f;
       // isActivated = false;
    }
}
