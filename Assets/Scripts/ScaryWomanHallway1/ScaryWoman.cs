using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryWoman : MonoBehaviour
{
    public Animator targetAnimator; // Reference to the Animator component on another GameObject
    public string Hide; // The name of the trigger parameter in the Animator

    private void OnCollisionEnter(Collision collision)
    {
        if (targetAnimator != null)
        {
            // Trigger the animation
            targetAnimator.SetTrigger(Hide);
        }
        else
        {
            Debug.LogError("Target Animator is not assigned!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetAnimator != null)
        {
            // Trigger the animation
            targetAnimator.SetTrigger(Hide);
        }
        else
        {
            Debug.LogError("Target Animator is not assigned!");
        }
    }
}
