using System.Collections;
using UnityEngine;
/// <summary>
/// Copilot was used to refine this script
/// </summary>

public class EnableGroupWithBlink : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The group of objects to enable.")]
    public GameObject groupToEnable; // Assign the group in the Inspector

    [Tooltip("Duration for each blink (on/off cycle).")]
    public float blinkDuration = 0.2f; // How long the object stays on/off during a blink

    [Tooltip("Time between the two blink sequences.")]
    public float timeBetweenBlinks = 1.0f; // Gap between the two blink sequences

    private bool isBlinking = false; // To prevent re-triggering

    private void OnTriggerEnter(Collider other)
    {
        // Trigger only if the player enters and the blinking is not already active
        if (other.CompareTag("Player") && !isBlinking)
        {
            StartCoroutine(BlinkTwiceAndEnable());
        }
    }

    private IEnumerator BlinkTwiceAndEnable()
    {
        isBlinking = true; // Prevent further triggers

        if (groupToEnable == null)
        {
            Debug.LogError("Group to Enable is not assigned in the script!");
            yield break;
        }

        // Ensure the group is initially disabled
        groupToEnable.SetActive(false);

        // Perform the first blink sequence
        yield return StartCoroutine(PerformBlink());

        // Wait between blinks
        yield return new WaitForSeconds(timeBetweenBlinks);

        // Perform the second blink sequence
        yield return StartCoroutine(PerformBlink());

        // Ensure the group is fully enabled after blinking
        groupToEnable.SetActive(true);

        isBlinking = false; // Reset blinking flag
    }

    private IEnumerator PerformBlink()
    {
        // Blink on and off (toggle twice)
        for (int i = 0; i < 2; i++) // Two toggles (on/off)
        {
            groupToEnable.SetActive(!groupToEnable.activeSelf); // Toggle state
            yield return new WaitForSeconds(blinkDuration); // Wait for blink duration
        }
    }
}
