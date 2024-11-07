using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRotation : MonoBehaviour
{
    private Vector3 lastPosition; // Stores the position from the last frame

    void Start()
    {
        lastPosition = transform.position; // Initialize last position
    }

    void Update()
    {
        // Calculate the movement direction by subtracting last position from current position
        Vector3 movementDirection = transform.position - lastPosition;

        // Check if there's movement
        if (movementDirection != Vector3.zero)
        {
            // Calculate the rotation to face the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);

            // Apply the rotation to the GameObject
            transform.rotation = targetRotation;

            // Adjust the rotation to face y 270 degrees
            transform.rotation *= Quaternion.Euler(0f, 90f, 0f);
        }

        // Update lastPosition for the next frame
        lastPosition = transform.position;
    }
}

