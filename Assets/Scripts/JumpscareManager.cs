using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the jumpscare functionality, including enabling the jumpscare prefab,
/// triggering sound effects, and fading in and out a death canvas.
/// </summary>
public class JumpscareManager : MonoBehaviour
{
    [Header("Jumpscare Settings")]
    [Tooltip("The prefab used for the jumpscare effect.")]
    public GameObject jumpscarePrefab;

    [Tooltip("The duration for which the jumpscare is displayed.")]
    public float scareDuration = 2f;

    [Header("UI Settings")]
    [Tooltip("The canvas displayed during the jumpscare sequence.")]
    [SerializeField] private Canvas deathCanvas;

    [Tooltip("The CanvasGroup component used to fade the death canvas in and out.")]
    [SerializeField] private CanvasGroup deathCanvasGroup;

    /// <summary>
    /// Disables the jumpscare prefab at the start of the game.
    /// </summary>
    private void Start()
    {
        if (jumpscarePrefab != null)
        {
            jumpscarePrefab.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Jumpscare prefab is not assigned.");
        }
    }

    /// <summary>
    /// Triggers the jumpscare sequence by activating the prefab, playing sound effects,
    /// and initiating the death canvas fade-in/out coroutine.
    /// </summary>
    public void TriggerJumpscare()
    {
        if (jumpscarePrefab != null)
        {
            AkSoundEngine.SetRTPCValue("RTPC_MonsterState", 2); // Set Wwise sound parameter
            StartCoroutine(HandleDeath());
        }
        else
        {
            Debug.LogWarning("Jumpscare prefab is not assigned.");
        }
    }

    /// <summary>
    /// Handles the full jumpscare sequence, including enabling/disabling the prefab
    /// and fading the death canvas in and out.
    /// </summary>
    private IEnumerator HandleDeath()
    {
        // Enable the death canvas and jumpscare prefab
        deathCanvas.gameObject.SetActive(true);
        jumpscarePrefab.SetActive(true);

        // Fade in the death canvas
        float duration = 5f;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            deathCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        // Wait for the jumpscare duration
        yield return new WaitForSeconds(scareDuration);

        // Disable the jumpscare prefab
        jumpscarePrefab.SetActive(false);

        // Fade out the death canvas
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            deathCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }

        // Wait for 2 seconds (optional/redundant based on comment)
        yield return new WaitForSeconds(2f);

        // Disable the death canvas
        deathCanvas.gameObject.SetActive(false);
    }
}