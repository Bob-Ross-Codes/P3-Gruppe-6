using System.Collections;
using UnityEngine;

/// <summary>
/// Handles de-agro behavior for the monster when the player looks at the collider for a specified duration.
/// </summary>
public class DeAgro : GazeActivation
{
    [Header("Monster Settings")]
    [SerializeField] private Animator monsterAnimator; // Animator for the monster
    [SerializeField] private GameObject monsterPrefab; // Monster prefab to destroy

    [Header("Closet Interaction")]
    [SerializeField] public ClosetHide closetHideScript; // Reference to the ClosetHide script

    private float lookAtTime = 0f; // Tracks how long the player has been looking
    private Coroutine deAgroCoroutine = null; // Reference to the active countdown coroutine
    private bool isBeingLookedAt = false; // Tracks if the player is looking at the collider

    /// <summary>
    /// Time required to detect player gaze activation.
    /// </summary>
    public override float ActivationTime => 0.1f;

    /// <summary>
    /// Called when the player starts looking at the collider.
    /// Triggers the monster animation and starts the de-agro countdown.
    /// </summary>
    public override void OnLookedAt()
    {
        if (!isBeingLookedAt)
        {
            isBeingLookedAt = true;
            Debug.Log("Player started looking at DeAgro collider.");

            // Trigger the monster's de-agro animation
            monsterAnimator?.SetTrigger("DeAgro");

            // Start countdown coroutine if it's not already running
            if (deAgroCoroutine == null)
            {
                deAgroCoroutine = StartCoroutine(StartDeAgroCountdown());
            }
        }
    }

    /// <summary>
    /// Continuously checks if the player is still looking at the collider.
    /// Resets the de-agro timer if the player looks away.
    /// </summary>
    private void Update()
    {
        if (!IsLookingAt() && isBeingLookedAt)
        {
            Debug.Log("Player looked away from DeAgro collider.");
            ResetDeAgroTimer();
        }
    }

    /// <summary>
    /// Starts a countdown to de-agro the monster.
    /// Destroys the monster if the player looks at the collider for 7 continuous seconds.
    /// </summary>
    private IEnumerator StartDeAgroCountdown()
    {
        lookAtTime = 0f;
        Debug.Log("DeAgro countdown started.");

        while (lookAtTime < 7f)
        {
            lookAtTime += Time.deltaTime;

            // Exit coroutine if the player stops looking
            if (!isBeingLookedAt)
            {
                Debug.Log("Player interrupted gaze. Countdown stopped.");
                yield break;
            }

            yield return null;
        }

        Debug.Log("7 seconds reached. Destroying monster...");
        if (monsterPrefab != null)
        {
            Destroy(monsterPrefab);
            closetHideScript.StartCoroutine(closetHideScript.WaitForMonsterToBeDestroyed());
        }

        deAgroCoroutine = null; // Clear the coroutine reference
    }

    /// <summary>
    /// Resets the de-agro timer and stops any ongoing countdown coroutine.
    /// </summary>
    private void ResetDeAgroTimer()
    {
        isBeingLookedAt = false;
        lookAtTime = 0f;

        if (deAgroCoroutine != null)
        {
            StopCoroutine(deAgroCoroutine);
            deAgroCoroutine = null;
        }

        Debug.Log("DeAgro timer reset.");
    }

    /// <summary>
    /// Determines if the player is still looking at the collider.
    /// Replace this with actual gaze detection logic.
    /// </summary>
    /// <returns>True if the player is looking at the collider; otherwise, false.</returns>
    private bool IsLookingAt()
    {
        return isBeingLookedAt;
    }
}