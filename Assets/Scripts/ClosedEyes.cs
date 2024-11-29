using System.Collections;
using UnityEngine;

public class ClosedEyes : MonoBehaviour
{
    [SerializeField] private float blinkOffSet = 0.5f; // Delay before fade starts (optional)
    private float blinkTime; // Tracks how long the player is holding down 'B'

    [SerializeField] private Canvas deathCanvas; // Canvas to fade in and out
    [SerializeField] private CanvasGroup deathCanvasGroup; // CanvasGroup for controlling fade transparency

    public bool _blinking; // Tracks if the player is blinking
    private bool isFading; // Ensures fade coroutine is not restarted unnecessarily
    public float fadeSpeed = 2f;
    private float targetAlpha = 0f;

    private bool hasPlayedSound = false; // Flag to ensure the sound is played once

    void Update()
    {
        // If the spacebar is pressed, set the target alpha to 1 (eyes closed)
        if (Input.GetKey(KeyCode.Space))
        {
            targetAlpha = 1f;
            _blinking = true;
            blinkTime += Time.deltaTime;

            // Play sound only once when space is first pressed
            if (!hasPlayedSound)
            {
                AkSoundEngine.PostEvent("Play_Eyes_Closed", gameObject);
                hasPlayedSound = true; // Mark that the sound has been played
            }
        }
        else // If the spacebar is not pressed, set the target alpha to 0 (eyes open)
        {
            targetAlpha = 0f;
            _blinking = false;
            blinkTime = 0;

            // Stop the sound when the spacebar is released
            if (hasPlayedSound)
            {
                AkSoundEngine.PostEvent("Stop_Eyes_Closed", gameObject);
                hasPlayedSound = false; // Reset the sound flag
            }
        }

        // Smoothly interpolate the canvas alpha toward the target alpha
        deathCanvasGroup.alpha = Mathf.MoveTowards(deathCanvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }
}
