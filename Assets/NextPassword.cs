/// <summary>
/// Handles sequential fading of password sprites.
/// Fades out each sprite in the provided array one by one over a specified duration.
/// </summary>
using UnityEngine;
using System.Collections;

public class NextPassword : MonoBehaviour
{
    [Header("Sprite Settings")]
    [Tooltip("Array of sprites to fade sequentially.")]
    public SpriteRenderer[] spriteRenderers;

    [Header("Fade Settings")]
    [Tooltip("Duration of the fade effect for each sprite.")]
    public float fadeDuration = 4f;

    private int currentSpriteIndex = 0; // Index of the sprite currently being faded
    private Coroutine fadeCoroutine; // Reference to the active fade coroutine
    private bool isPaused = false; // Tracks whether the fading process is paused

    /// <summary>
    /// Ensures that the sprite array is properly assigned.
    /// Logs an error if the array is empty or null.
    /// </summary>
    private void Awake()
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            Debug.LogError("SpriteRenderers array is empty or not assigned.");
        }
    }

    /// <summary>
    /// Starts the fade-out sequence for all sprites in the array.
    /// </summary>
    public void StartFadeOut()
    {
        if (fadeCoroutine == null && spriteRenderers.Length > 0)
        {
            fadeCoroutine = StartCoroutine(FadeOutSequentially());
        }
    }

    /// <summary>
    /// Pauses or resumes the fading process based on the given parameter.
    /// </summary>
    /// <param name="pause">Set to true to pause fading, or false to resume.</param>
    public void PauseFadeOut(bool pause)
    {
        if (pause != isPaused)
        {
            Debug.Log($"Pause state changed to: {pause}");
        }
        isPaused = pause;
    }

    /// <summary>
    /// Fades out all sprites sequentially, one after the other.
    /// </summary>
    private IEnumerator FadeOutSequentially()
    {
        while (currentSpriteIndex < spriteRenderers.Length)
        {
            SpriteRenderer currentSprite = spriteRenderers[currentSpriteIndex];
            if (currentSprite == null)
            {
                Debug.LogWarning($"SpriteRenderer at index {currentSpriteIndex} is null. Skipping.");
                currentSpriteIndex++;
                continue;
            }

            yield return StartCoroutine(FadeOutSprite(currentSprite));
            currentSpriteIndex++;
        }

        fadeCoroutine = null; // Reset the coroutine when the fading process is complete
    }

    /// <summary>
    /// Fades out a single sprite over the specified duration.
    /// </summary>
    /// <param name="spriteRenderer">The sprite to fade out.</param>
    private IEnumerator FadeOutSprite(SpriteRenderer spriteRenderer)
    {
        float timer = 0f;
        Color originalColor = spriteRenderer.color;

        while (timer < fadeDuration)
        {
            if (isPaused)
            {
                yield return null;
                continue;
            }

            timer += Time.deltaTime;

            // Calculate the alpha value using linear interpolation
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }

        // Ensure the sprite is fully transparent at the end of the fade
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}