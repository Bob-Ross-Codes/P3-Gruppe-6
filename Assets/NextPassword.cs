using UnityEngine;
using System.Collections;

public class NextPassword : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers; // Array of SpriteRenderers to fade sequentially
    public float fadeDuration = 12f; // Duration of fade-out in seconds
    private int currentSpriteIndex = 0; // Index of the sprite currently being faded
    private Coroutine fadeCoroutine;
    private bool isPaused = false;

    void Awake()
    {
        // Validate the array
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            Debug.LogError("SpriteRenderers array is empty or not assigned.");
        }
    }

    public void StartFadeOut()
    {
        // Start fading the first sprite if not already running
        if (fadeCoroutine == null && spriteRenderers.Length > 0)
        {
            fadeCoroutine = StartCoroutine(FadeOutSequentially());
        }
    }

    public void PauseFadeOut(bool pause)
    {
        if (pause != isPaused)
        {
            Debug.Log("isPaused changed to: " + pause);
        }
        isPaused = pause;
    }

    private IEnumerator FadeOutSequentially()
    {
        while (currentSpriteIndex < spriteRenderers.Length)
        {
            SpriteRenderer currentSpriteRenderer = spriteRenderers[currentSpriteIndex];

            if (currentSpriteRenderer == null)
            {
                Debug.LogWarning($"SpriteRenderer at index {currentSpriteIndex} is null. Skipping.");
                currentSpriteIndex++;
                continue;
            }

            yield return StartCoroutine(FadeOutSprite(currentSpriteRenderer));

            currentSpriteIndex++; // Move to the next sprite
        }

        fadeCoroutine = null; // All fades completed, reset coroutine reference
    }

    private IEnumerator FadeOutSprite(SpriteRenderer spriteRenderer)
    {
        float timer = 0f;
        Color originalColor = spriteRenderer.color;

        Debug.Log("Fader ud!");

        while (timer < fadeDuration)
        {
            if (isPaused)
            {
                yield return null; // Wait for the next frame while paused
                continue;
            }

            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration); // Calculate alpha from 1 to 0
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null; // Wait for the next frame
        }

        // Ensure it's fully transparent at the end
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
