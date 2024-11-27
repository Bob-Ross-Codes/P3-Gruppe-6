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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _blinking = true;
            StartCoroutine(FadeCanvas(1f, 1f)); // Fade to black over 1 second
        }

        if (_blinking)
        {
            blinkTime += Time.deltaTime;

            // Optional: Add logic here if you need to trigger other behaviors based on blinkTime
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _blinking = false;
            blinkTime = 0;
            StartCoroutine(FadeCanvas(0f, 1f)); // Fade back to normal over 1 second
        }
    }

    private IEnumerator FadeCanvas(float targetAlpha, float duration)
    {
        if (isFading) yield break; // Prevent multiple fade coroutines from running simultaneously

        isFading = true;

        // Ensure the canvas is active while fading
        deathCanvas.gameObject.SetActive(true);

        float startAlpha = deathCanvasGroup.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            deathCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }

        // Ensure the final alpha value is set (to avoid small inaccuracies)
        deathCanvasGroup.alpha = targetAlpha;

        // Disable the canvas once fade out is complete
        if (targetAlpha == 0f)
        {
            deathCanvas.gameObject.SetActive(false);
        }

        isFading = false;
    }
}
