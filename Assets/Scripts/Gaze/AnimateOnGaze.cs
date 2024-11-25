using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnGaze : GazeActivation
{
    public override float ActivationTime => 0.6f; // Time required to look at the object before destruction

    public Animator targetAnimator; // Reference to the Animator component on another GameObject
    public Animator targetDoorAnimator; // Reference to the Animator component on another GameObject



    public override void OnLookedAt()
    {
        if (targetAnimator != null && targetDoorAnimator != null)
        {
            // Trigger both animations
            targetAnimator.SetTrigger("Hide");
            targetDoorAnimator.SetTrigger("DoorOpen");
          
        }
        else
        {
            Debug.LogError("Target Animator or Target Door Animator is not assigned!");
        }
    }

}
