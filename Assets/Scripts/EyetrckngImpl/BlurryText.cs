using UnityEngine;
using System.Collections;

public class BlurryText : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float fadeDuration = 12f;  // Duration of fade-out in seconds
    private float timer = 0f;
    private bool isPaused = false;
    private Coroutine fadeCoroutine;
    private bool fading = false;

    void Awake()
    {
        // Get the SpriteRenderer component from a child GameObject
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found in child GameObjects.");
        }
    }

    public void StartFadeOut()
    {
        // Start the fade-out coroutine if not already running
        if (fadeCoroutine == null && fading == false)
        {
            fadeCoroutine = StartCoroutine(FadeOut());
            fading = true;
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

    private IEnumerator FadeOut()
    {
        Color originalColor = spriteRenderer.color;

        while (timer < fadeDuration)
        {
            // Check if we should pause
            if (isPaused)
            {
                yield return null;  // Wait until the next frame
                continue;
            }

            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);  // Calculate alpha from 1 to 0
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;  // Wait for the next frame
        }

        // Ensure it's fully transparent at the end
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        // Fade-out complete, reset coroutine reference
        fadeCoroutine = null;
    }
}
