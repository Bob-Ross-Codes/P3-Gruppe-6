/// <summary>
/// Triggers animations and performs actions when the player looks at an object for a set duration.
/// </summary>
using System.Collections;
using UnityEngine;

public class AnimateOnGaze : GazeActivation
{
    /// <summary>
    /// Time required to look at the object before triggering the animations.
    /// </summary>
    public override float ActivationTime => 1f;

    [Header("Animator References")]
    [SerializeField] private Animator targetAnimator; // Animator for the target object
    [SerializeField] private Animator targetDoorAnimator; // Animator for the target door

    /// <summary>
    /// Activates when the object is looked at for the required duration.
    /// Triggers the specified animations.
    /// </summary>
    public override void OnLookedAt()
    {
        if (targetAnimator != null && targetDoorAnimator != null)
        {
            // Trigger animations on both objects
            targetAnimator.SetTrigger("Hide");
            targetDoorAnimator.SetTrigger("DoorOpen");
        }
        else
        {
            Debug.LogError("Target Animator or Target Door Animator is not assigned!");
        }
    }
}