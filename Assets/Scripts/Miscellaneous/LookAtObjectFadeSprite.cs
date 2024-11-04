using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtObjectFadeSprite : MonoBehaviour
{
    public Camera mainCamera; // The camera from which the ray is cast
    public Transform targetObject; // The object to check if the player is looking at (e.g., a door)
    public float fadeSpeed = 2f; // Speed of fading in and out
    public float maxDistance = 10f; // Maximum distance for the ray to detect the object

    private CanvasGroup spriteCanvasGroup; // CanvasGroup for fading
    private bool isLookingAtTarget;

    void Start()
    {
        // Get the CanvasGroup component attached to this object
        spriteCanvasGroup = GetComponent<CanvasGroup>();
        
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
    }

    private void HandleSpriteFade()
    {
        if (isLookingAtTarget)
        {
            // Fade in
            spriteCanvasGroup.alpha = Mathf.MoveTowards(spriteCanvasGroup.alpha, 1f, fadeSpeed * Time.deltaTime);
        }
        else
        {
            // Fade out
            spriteCanvasGroup.alpha = Mathf.MoveTowards(spriteCanvasGroup.alpha, 0f, fadeSpeed * Time.deltaTime);
        }
    }
}
