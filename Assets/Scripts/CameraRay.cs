using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRay : MonoBehaviour
{
    public Camera mainCamera;  // Reference to the main camera
    public float rayDistance = 100f;  // Distance the ray can travel

    public Color rayColor = Color.green; // Color of the visualized ray

    void Update()
    {
        // Continuously shoot the ray
        Shoot();
    }

    void Shoot()
    {
        // Create a ray from the camera's position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

// Draw the ray in the Scene view for debugging purposes
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, rayColor, 30.0f);  // Ray will last for 2 seconds


        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Check if the object hit has the tag "Enemy"
            if (hit.collider.CompareTag("Enemy"))
            {
                // Destroy the enemy object
                Destroy(hit.collider.gameObject);
                Debug.Log("Enemy destroyed: " + hit.collider.gameObject.name);
            }
        }
    }
}
