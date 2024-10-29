using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 0.05f; // Small multiplier for a subtle effect

    private Vector3 startPosition; // Initial position of each object

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Get normalized mouse position centered at (0, 0) with a range of [-0.5, 0.5]
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition) - new Vector3(0.5f, 0.5f);

        // Calculate the target position with a small offset multiplier for a subtle effect
        Vector3 targetPosition = startPosition + new Vector3(offset.x * offsetMultiplier, offset.y * offsetMultiplier, 0);

        // Directly set the object position to follow the cursor
        transform.position = targetPosition;
    }
}

