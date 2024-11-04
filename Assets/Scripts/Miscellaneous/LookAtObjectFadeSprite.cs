using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObjectFadeSprite : MonoBehaviour
{
    public Camera mainCamera; // The camera from which the ray is cast
    public Transform targetObject; // The object to check if the player is looking at (e.g., a door)
    public float fadeSpeed = 2f; // Speed of fading in and out
    public float maxDistance = 10f; // Maximum distance for the ray to detect the object

    private SpriteRenderer spriteRenderer; // SpriteRenderer for changing color alpha
    private bool isLookingAtTarget;

    void Start()
    {
        // Get the SpriteRenderer component attached to this object
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the sprite starts fully transparent
        SetSpriteAlpha(0f);
        
        // If no camera is assigned, automatically find the main camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        CheckIfLookingAtTarget();
        HandleSpriteFade();
    }

    private void CheckIfLookingAtTarget()
    {
        if (targetObject == null || spriteRenderer == null)
        {
            isLookingAtTarget = false;
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Check if the ray hits the targetObject
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            isLookingAtTarget = hit.transform == targetObject;
        }
        else
        {
            isLookingAtTarget = false;
        }

        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
    }

    private void HandleSpriteFade()
    {
        // Determine target alpha based on whether looking at the target
        float targetAlpha = isLookingAtTarget ? 1f : 0f;

        // Smoothly transition to the target alpha value
        float currentAlpha = Mathf.MoveTowards(spriteRenderer.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
        
        // Set the sprite's color with the new alpha value
        SetSpriteAlpha(currentAlpha);
    }

    // Helper method to set the alpha of the sprite's color
    private void SetSpriteAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
