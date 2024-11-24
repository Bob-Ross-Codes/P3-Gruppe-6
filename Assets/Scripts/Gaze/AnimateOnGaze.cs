using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnGaze : GazeActivation
{
    public override float ActivationTime => 0.2f; // Time required to look at the object before destruction

    public Animator targetAnimator; // Reference to the Animator component on another GameObject

    public GameObject targetObject; // Object to destroy

    public override void OnLookedAt()
    {
       if (targetAnimator != null)
                {
                    // Trigger the animation
                    targetAnimator.SetTrigger("Hide");
                    killWoman();
                }
                else
                {
                    Debug.LogError("Target Animator is not assigned!");
                }
    
    }
    private IEnumerator killWoman()
    {
        yield return new WaitForSeconds(1f);Destroy(targetObject);
    }

}
