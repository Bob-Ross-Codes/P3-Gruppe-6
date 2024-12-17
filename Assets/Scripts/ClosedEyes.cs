using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles blink detection and plays corresponding sound effects when the player blinks.
/// </summary>
public class ClosedEyes : MonoBehaviour
{
    [Header("Blink Settings")]
    [SerializeField] private Gaze gaze; // Reference to the Gaze detection script
    [SerializeField] private float blinkOffSet = 0.5f; // Time required to trigger the blink sound

    private float blinkTime = 0f; // Tracks the duration of a blink
    private bool blinking = false; // Tracks if the blink sound is currently playing

    /// <summary>
    /// Monitors blink input and triggers sound effects.
    /// </summary>
    private void Update()
    {
        // Check for blinking via keyboard or gaze input
        if (Input.GetKeyDown(KeyCode.B) || gaze._blinking)
        {
            if (!blinking)
            {
                blinkTime += Time.deltaTime;

                // Trigger blink sound if blinking exceeds the offset
                if (blinkTime >= blinkOffSet)
                {
                    blinking = true;
                    AkSoundEngine.PostEvent("Play_Eyes_Closed", gameObject);
                }
            }
        }
        else
        {
            // Stop the blink sound and reset variables when blinking stops
            AkSoundEngine.PostEvent("Stop_Eyes_Closed", gameObject);
            blinkTime = 0f;
            blinking = false;
        }
    }
}