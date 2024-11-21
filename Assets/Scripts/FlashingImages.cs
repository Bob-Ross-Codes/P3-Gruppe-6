using AK.Wwise;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashingImages : MonoBehaviour
{
    [SerializeField] private Image canvasImage;
    [SerializeField] private Sprite[] images;
    [SerializeField] private float minFlashSpeed = 0.01f;
    [SerializeField] private float maxFlashSpeed = 0.1f;
    [SerializeField] private int currentImage = 0;
    [SerializeField] private int blinks; // Number of times to loop
    [SerializeField] private float waitTime;

    private Coroutine flashCoroutine;
    private bool playerInTrigger = false;

    void Start()
    {
        // Start by disabling the canvas image
        canvasImage.enabled = false;
        // Ensure the canvasImage and images array are set
        if (canvasImage == null || images.Length == 0)
        {
            Debug.LogError("Canvas Image or Images array not set.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          //  Debug.Log("BLINKING ACTIVATED");
            playerInTrigger = true;
            if (flashCoroutine == null)
            {
                // Enable the canvas image
              
                // Start the coroutine
                flashCoroutine = StartCoroutine(FlashImages());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           // Debug.Log("BLINKING DEACTIVATED");
            playerInTrigger = false;
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
                // Disable the canvas image
                canvasImage.enabled = false;
            }
        }
    }

    private IEnumerator FlashImages()
    {

        yield return new WaitForSeconds(waitTime); // Hvis du vil have flashing timet til at starte efter et bestemt tidspunkt, spaghetti kode
        canvasImage.enabled = true;
        int currentLoop = 0;

        while (playerInTrigger && currentLoop < blinks)
        {
            // Set the current image
            canvasImage.sprite = images[currentImage];
        //    Debug.Log($"Displaying image {currentImage}");
            // Move to the next image
            currentImage = (currentImage + 1) % images.Length;
            // Show the image for one frame
            yield return null;
            // Disable the canvas image
            canvasImage.enabled = false;
            // Wait for the specified flash speed before showing the next image
            yield return new WaitForSeconds(UnityEngine.Random.Range(minFlashSpeed, maxFlashSpeed));
            // Enable the canvas image for the next frame
            canvasImage.enabled = true;

            // Increment the loop counter
            currentLoop++;
        }

        // Ensure the canvas image is disabled when the player leaves the trigger or loop count is reached
        canvasImage.enabled = false;
        flashCoroutine = null;

        // Disable the GameObject after flashing
        gameObject.SetActive(false);
    }
}
