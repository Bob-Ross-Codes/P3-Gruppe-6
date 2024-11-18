using System.Collections;
using UnityEngine;

public class DeAgro : GazeActivation
{
    public Animator monsteranimater; // Animator reference
    public GameObject monsterPrefab; // Monster prefab reference

    private float lookAtTime = 0f; // Tracks how long the collider is being looked at
    private Coroutine deAgroCoroutine = null; // Reference to the countdown coroutine
    private bool isBeingLookedAt = false; // Tracks if the player is looking at the collider

    public override float ActivationTime => 0.1f; // Defines how quickly `OnLookedAt()` activates

    public override void OnLookedAt()
    {
        if (!isBeingLookedAt)
        {
            // Player started looking at the collider
            isBeingLookedAt = true;
            Debug.Log("Player started looking at DeAgro collider.");

            // Trigger the monster animation
            if (monsteranimater != null)
            {
                monsteranimater.SetTrigger("DeAgro");
            }

            // Start the countdown coroutine if not already running
            if (deAgroCoroutine == null)
            {
                deAgroCoroutine = StartCoroutine(StartDeAgroCountdown());
            }
        }
    }

    private void Update()
    {
        // Continuously check if the player is still looking at the collider
        if (!IsLookingAt())
        {
            if (isBeingLookedAt)
            {
                // Player stopped looking at the collider
                Debug.Log("Player looked away from DeAgro collider.");
                ResetDeAgroTimer();
            }
        }
    }

    private IEnumerator StartDeAgroCountdown()
    {
        lookAtTime = 0f; // Reset the timer
        Debug.Log("Countdown started.");

        while (lookAtTime < 7f)
        {
            lookAtTime += Time.deltaTime;

            // If the player stops looking, exit the coroutine
            if (!isBeingLookedAt)
            {
                Debug.Log("Player interrupted gaze. Stopping countdown.");
                yield break;
            }

            yield return null;
        }

        // Destroy the monster after 7 continuous seconds of looking
        Debug.Log("7 seconds reached. Destroying monster...");
        if (monsterPrefab != null)
        {
            Destroy(monsterPrefab);
        }

        deAgroCoroutine = null; // Clean up the coroutine reference
    }

    private void ResetDeAgroTimer()
    {
        // Reset the state and stop the coroutine
        isBeingLookedAt = false;
        lookAtTime = 0f;

        if (deAgroCoroutine != null)
        {
            StopCoroutine(deAgroCoroutine);
            deAgroCoroutine = null;
        }
    }

    private bool IsLookingAt()
    {
        // Replace this with your actual gaze detection logic.
        // For now, it's assumed that `OnLookedAt()` keeps `isBeingLookedAt` true.
        return isBeingLookedAt;
    }
}





