using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureOpen : MonoBehaviour
/*{
    public GameObject picture;
    private bool isPictureActive = false;
    private bool isPictureRotated = false;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isPictureActive)
            {
                picture.SetActive(true);
                isPictureActive = true;
            }
            else
            {
                picture.transform.Rotate(0, 180, 0);
                isPictureRotated = true;
                isPictureActive = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && isPictureRotated)
        {
            isPictureRotated = false;
            picture.SetActive(false);

        }

    }
}*/

{
    public Animator animator;
    private bool isHandIn = false;        // Tracks if the hand is in
    private bool isHandFlipped = false;  // Tracks if the hand is flipped

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isHandIn)
            {
                // Trigger the HandIn animation
                animator.SetTrigger("HandIn");
                isHandIn = true;
            }
            else if (isHandIn && !isHandFlipped)
            {
                // Trigger the HandFlip animation
                animator.SetTrigger("HandFlip");
                Debug.Log ("Flip!!!");
                isHandFlipped = true;
            }
            else if (isHandIn && isHandFlipped)
            {
                // Trigger the HandOut animation
                animator.SetTrigger("HandOut");
                 Debug.Log ("Out!!!");

                // Reset the flags after HandOut
                 StartCoroutine(WaitForHandOutAnimation());
            }
        }
    }


    // Coroutine to wait for the HandOut animation to finish
    private System.Collections.IEnumerator WaitForHandOutAnimation()
    {
        // Wait for the HandOut animation length
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Reset the states
        isHandIn = false;
        isHandFlipped = false;

        // Clear all triggers to avoid unexpected behavior
        animator.ResetTrigger("HandIn");
        animator.ResetTrigger("HandFlip");
        animator.ResetTrigger("HandOut");
    }
}

