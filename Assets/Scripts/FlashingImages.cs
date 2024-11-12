using AK.Wwise;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashingImages : MonoBehaviour
{
    [SerializeField] private Image canvasImage;
    [SerializeField] private Sprite[] images;
    [SerializeField] private float flashSpeed = 0.5f;
    [SerializeField] private int currentImage = 0;

    private Coroutine flashCoroutine;

    void Start()
    {
        // start by disabling the canvas image
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
            canvasImage.enabled = false;
        }
    }

    private IEnumerator FlashImages()
    {
        while (true)
        {
            // Set the current image
            canvasImage.sprite = images[currentImage];
            // Move to the next image
            currentImage = (currentImage + 1) % images.Length;
            // Wait for one frame
            yield return null;
            // Wait for the specified flash speed
            canvasImage.enabled = false;
            yield return new WaitForSeconds(flashSpeed);
            canvasImage.enabled = true;
        }
    }
}
