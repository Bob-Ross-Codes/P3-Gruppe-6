/// <summary>
/// Controls the behavior of the agro monster, including triggering jumpscares when the player looks at it.
/// </summary>
using UnityEngine;

public class AgroMonster : GazeActivation
{
    /// <summary>Time before the jumpscare is triggered.</summary>
    public override float ActivationTime => 0.1f;

    /// <summary>Animator for the monster's animations.</summary>
    public Animator monsterAnimator;

    /// <summary>Handles the player's gaze detection.</summary>
    [SerializeField] private Gaze gaze;

    /// <summary>Tracks how long the player has been looking at the monster.</summary>
    [SerializeField] private float timeToScare;

    /// <summary>Manager to trigger the jumpscare effect.</summary>
    [SerializeField] private JumpscareManager jumpscareManager;

    /// <summary>Reference to the monster prefab.</summary>
    [SerializeField] private GameObject monsterPrefab;

    /// <summary>Handles player hiding mechanics.</summary>
    [SerializeField] private ClosetHide closetHide;

    /// <summary>
    /// Called when the player looks at the monster.
    /// Increments the time and triggers animations or jumpscares.
    /// </summary>
    public override void OnLookedAt()
    {
        // Increment the time the player is looking at the monster
        timeToScare += Time.deltaTime;

        // Trigger the agro animation
        monsterAnimator.SetTrigger("Agro");
        Debug.Log($"Time to scare: {timeToScare}");

        // Trigger jumpscare if the player looks long enough
        if (timeToScare >= 1f)
        {
            jumpscareManager.TriggerJumpscare(); // Execute jumpscare
            Destroy(gameObject); // Remove the monster
            Destroy(monsterPrefab); // Remove the prefab
            closetHide.canToggleHiding = true; // Allow the player to hide
        }

        // Reset time to scare if the player blinks or looks away
        if (gaze._blinking)
        {
            timeToScare = 0;
        }
    }
}