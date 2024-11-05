using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastAtSight : MonoBehaviour
{
    //slet(//)       public Gaze gazeScript; // Reference to the Gaze script
    public Camera mainCamera; // Reference to the main camera

    // Update is called once per frame
    void Update()
    {
    //slet(//)      if (gazeScript != null && mainCamera != null)
        {
            // Get the gaze location
    //slet(//)          Vector2 gazeLocation = gazeScript.gazeLocation;

            // Convert gaze location to a ray
    //slet(//)          Ray ray = mainCamera.ScreenPointToRay(new Vector3(gazeLocation.x, gazeLocation.y, 0));

            // Perform the raycast
    //slet(//)         if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Log the hit object
   //slet(//)               Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

                // Optionally, you can do something with the hit object
                // For example, highlight it, interact with it, etc.
            }

            // Draw the ray in the scene view for debugging
    //slet(//)          Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        }
    }
}
