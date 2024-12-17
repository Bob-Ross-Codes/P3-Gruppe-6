using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles gaze activation for triggering sound effects.
/// Plays a whispering sound when the player looks at the journal for the required duration
/// and stops the sound when necessary.
/// </summary>
public class Journal2EyeDetector : GazeActivation
{
    /// <summary>
    /// The time in seconds required for the player to look at the journal to activate the event.
    /// </summary>
    public override float ActivationTime => 2f;

    /// <summary>
    /// Called when the player has looked at the journal for the required duration.
    /// Starts playing the whispering sound.
    /// </summary>
    public override void OnLookedAt()
    {
        StartSound();
    }

    /// <summary>
    /// Stops the whispering sound effect.
    /// </summary>
    public void StopSound()
    {
        AkSoundEngine.PostEvent("Stop_Whispers", gameObject);
    }

    /// <summary>
    /// Starts the whispering sound effect.
    /// </summary>
    public void StartSound()
    {
        AkSoundEngine.PostEvent("Play_Whispers", gameObject);
    }
}