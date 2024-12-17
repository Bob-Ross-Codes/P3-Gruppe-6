using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the animation states of a hand interacting with a picture. Handles animations for moving the hand in,
/// flipping it, and moving it out, as well as cleaning up after animations are complete.
/// </summary>
public class PictureOpen : MonoBehaviour
{
    [Header("Animation and Object Settings")]
    [Tooltip("Animator component controlling the hand animations.")]
    public Animator animator;

    [Tooltip("GameObject representing the Arrrr sprite to be destroyed.")]
    public GameObject ArrrrSprite;

    [Header("Simulation Settings")]
    [Tooltip("Simulates a key press for testing purposes.")]
    public bool simulateKeyPress;

    // Tracks if the hand is in the scene
    private bool isHandIn = false;

    // Tracks if the hand is flipped
    private bool isHandFlipped = false;

    /// <summary>
    /// Initializes the animation state by triggering the HandIn animation.
    /// </summary>
    void Start()
    {
        animator.SetTrigger("HandIn");
        isHandIn = true;
    }

    /// <summary>
    /// Listens for key presses to control animations and destroy the Arrrr sprite.
    /// </summary>
    void Update()
    {
        // Check for R key press or simulated key press
        if (Input.GetKeyDown(KeyCode.R) || simulateKeyPress)
        {
            // Reset simulated key press
            if (simulateKeyPress) simulateKeyPress = false;

            // Destroy the sprite if it exists
            if (ArrrrSprite != null)
            {
                Destroy(ArrrrSprite);
            }

            // Handle animations based on the current state
            if (!isHandIn)
            {
                // Trigger HandIn animation
                animator.SetTrigger("HandIn");
                isHandIn = true;
            }
            else if (isHandIn && !isHandFlipped)
            {
                // Trigger HandFlip animation
                animator.ResetTrigger("HandIn");
                animator.SetTrigger("HandFlip");
                Debug.Log("Flip!!!");
                isHandFlipped = true;
            }
            else if (isHandIn && isHandFlipped)
            {
                // Trigger HandOut animation
                animator.SetTrigger("HandOut");
                Debug.Log("Out!!!");

                // Start coroutine to reset states after the animation
                StartCoroutine(WaitForHandOutAnimation());
            }
        }
    }

    /// <summary>
    /// Waits for the HandOut animation to finish before resetting animation states and triggers.
    /// </summary>
    private IEnumerator WaitForHandOutAnimation()
    {
        // Wait for the length of the HandOut animation
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Reset states and animation triggers
        isHandIn = false;
        isHandFlipped = false;
        animator.ResetTrigger("HandIn");
        animator.ResetTrigger("HandFlip");
        animator.ResetTrigger("HandOut");
    }
}