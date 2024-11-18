using AK.Wwise;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashingImages : MonoBehaviour
{
    [SerializeField] private Image canvasImage;
    [SerializeField] private Sprite[] images;
    [SerializeField] private float minFlashSpeed = 0.1f;
    [SerializeField] private float maxFlashSpeed = 0.5f;
    [SerializeField] private int currentImage = 0;

    private Coroutine flashCoroutine;

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
            if (flashCoroutine == null)
            {
                // Enable the canvas image
                canvasImage.enabled = true;
                // Start the coroutine
                flashCoroutine = StartCoroutine(FlashImages());
            }
        }
    }

    private IEnumerator FlashImages()
    {
        for (int i = 0; i < 4; i++)
        {
            // Set the current image
            canvasImage.sprite = images[currentImage];
            // Move to the next image
            currentImage = (currentImage + 1) % images.Length;
            // Wait for the specified flash speed
            yield return new WaitForSeconds(UnityEngine.Random.Range(minFlashSpeed, maxFlashSpeed));
            // Toggle the canvas image
            canvasImage.enabled = !canvasImage.enabled;
        }
        // Disable the GameObject after flashing
        gameObject.SetActive(false);
        flashCoroutine = null;
    }
}
