using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnGaze : GazeActivation
{
    public override float ActivationTime => 2.0f; // Time required to look at the object before destruction

    public Animator targetAnimator; // Reference to the Animator component on another GameObject

    public override void OnLookedAt()
    {
       if (targetAnimator != null)
                {
                    // Trigger the animation
                    targetAnimator.SetTrigger("Hide");
                }
                else
                {
                    Debug.LogError("Target Animator is not assigned!");
                }
    
    }
    
}
