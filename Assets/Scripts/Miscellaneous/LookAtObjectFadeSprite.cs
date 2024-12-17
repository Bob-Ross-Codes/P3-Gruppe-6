using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the fading of a sprite based on whether the player is looking at a specified target object.
/// The sprite becomes visible when the target is in the player's view and fades out otherwise.
/// </summary>
public class LookAtObjectFadeSprite : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The camera from which the ray is cast.")]
    public Camera mainCamera;

    [Tooltip("The object to check if the player is looking at (e.g., a door).")]
    public Transform targetObject;

    [Header("Fade Settings")]
    [Tooltip("The speed at which the sprite fades in and out.")]
    public float fadeSpeed = 2f;

    [Tooltip("The maximum distance for the ray to detect the target object.")]
    public float maxDistance = 10f;

    private SpriteRenderer spriteRenderer; // SpriteRenderer for changing the alpha value of the sprite
    private bool isLookingAtTarget; // Tracks whether the player is looking at the target object

    /// <summary>
    /// Initializes the sprite renderer, ensures the sprite starts fully transparent,
    /// and assigns the main camera if none is specified.
    /// </summary>
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Ensure the sprite starts fully transparent
        SetSpriteAlpha(0f);

        // Automatically assign the main camera if none is specified
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    /// <summary>
    /// Updates the script each frame by checking if the player is looking at the target,
    /// handling the sprite's fade effect, and allowing object destruction with the 'E' key.
    /// </summary>
    void Update()
    {
        CheckIfLookingAtTarget();
        HandleSpriteFade();

        // Destroy the object if the player presses 'E'
        if (Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Checks if the player is looking at the target object by casting a ray from the camera.
    /// Updates the `isLookingAtTarget` variable based on the raycast result.
    /// </summary>
    private void CheckIfLookingAtTarget()
    {
        if (targetObject == null || spriteRenderer == null)
        {
            isLookingAtTarget = false;
            return;
        }

        // Cast a ray from the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        // Check if the ray hits the target object
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            isLookingAtTarget = hit.transform == targetObject;
        }
        else
        {
            isLookingAtTarget = false;
        }

        // Debug visualization for the ray
        Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
    }

    /// <summary>
    /// Handles the fading of the sprite by adjusting its alpha value.
    /// The sprite fades in if the player is looking at the target and fades out otherwise.
    /// </summary>
    private void HandleSpriteFade()
    {
        // Target alpha based on whether the player is looking at the target
        float targetAlpha = isLookingAtTarget ? 1f : 0f;

        // Smoothly transition to the target alpha value
        float currentAlpha = Mathf.MoveTowards(spriteRenderer.color.a, targetAlpha, fadeSpeed * Time.deltaTime);

        // Set the sprite's color with the updated alpha value
        SetSpriteAlpha(currentAlpha);
    }

    /// <summary>
    /// Sets the alpha value of the sprite's color.
    /// </summary>
    /// <param name="alpha">The desired alpha value (0 to 1).</param>
    private void SetSpriteAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}