using AK.Wwise;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the flashing image effect triggered when the player enters a specific area.
/// Plays sound events and loops through a series of images with configurable speed and duration.
/// </summary>
public class FlashingImages : MonoBehaviour
{
    [Header("UI Settings")]
    [Tooltip("The Image component to display the flashing images.")]
    [SerializeField] public Image canvasImage;

    [Tooltip("Array of sprites to flash.")]
    [SerializeField] private Sprite[] images;

    [Header("Flash Timing Settings")]
    [Tooltip("Minimum speed of the image flashes.")]
    [SerializeField] private float minFlashSpeed = 0.01f;

    [Tooltip("Maximum speed of the image flashes.")]
    [SerializeField] private float maxFlashSpeed = 0.1f;

    [Tooltip("Delay before flashing begins.")]
    [SerializeField] private float waitTime;

    [Tooltip("Number of loops before flashing stops.")]
    [SerializeField] private int blinks;

    private int currentImage = 0; // Index of the current image being displayed
    private Coroutine flashCoroutine; // Reference to the active flash coroutine

    [Header("Player Interaction Settings")]
    [Tooltip("The player GameObject.")]
    [SerializeField] private GameObject player;

    [Tooltip("Wwise event to play when flashing starts.")]
    [SerializeField] private AK.Wwise.Event startSoundEvent;

    private bool playerInTrigger = false; // Tracks if the player is within the trigger zone
    public bool flashing; // Indicates whether the flashing process is active

    /// <summary>
    /// Disables the canvas image on start to ensure it's hidden initially.
    /// </summary>
    private void Start()
    {
        canvasImage.enabled = false;
    }

    /// <summary>
    /// Called when a collider enters the trigger zone.
    /// Starts the flashing coroutine if the player enters.
    /// </summary>
    /// <param name="other">The collider entering the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (flashCoroutine == null)
            {
                flashCoroutine = StartCoroutine(FlashImages());
            }
        }
    }

    /// <summary>
    /// Called when a collider exits the trigger zone.
    /// Stops the flashing coroutine if the player exits.
    /// </summary>
    /// <param name="other">The collider exiting the trigger zone.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
            }
        }
    }

    /// <summary>
    /// Coroutine that handles the flashing of images.
    /// Plays the associated sound event and loops through the images until conditions are met.
    /// </summary>
    private IEnumerator FlashImages()
    {
        flashing = true;

        // Wait before starting the flashing
        yield return new WaitForSeconds(waitTime);

        // Play the start sound event
        startSoundEvent.Post(gameObject);

        canvasImage.enabled = true;
        int currentLoop = 0;

        // Loop through the flashing images
        while (playerInTrigger && currentLoop < blinks)
        {
            // Display the current image
            canvasImage.sprite = images[currentImage];

            // Move to the next image
            currentImage = (currentImage + 1) % images.Length;

            // Display the image for a brief moment
            yield return null;

            // Hide the image
            canvasImage.enabled = false;

            // Wait for a random duration before showing the next image
            yield return new WaitForSeconds(UnityEngine.Random.Range(minFlashSpeed, maxFlashSpeed));

            // Enable the image again
            canvasImage.enabled = true;

            // Increment the loop counter
            currentLoop++;
        }

        // Ensure the canvas image is hidden and the flashing state is reset
        canvasImage.enabled = false;
        flashing = false;
        flashCoroutine = null;

        // Stop the sound event
        startSoundEvent.Stop(gameObject);

        // Deactivate the GameObject after flashing
        gameObject.SetActive(false);
    }
}