using System.Collections;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;

public class BloodWallEyeDetector : GazeActivation
{
    public override float ActivationTime => 0.2f;

    public Light lightToFlicker;   // Reference to the light component
    public float flickerDuration = 3.0f;  // Total duration of flickering effect (on and off)
    public float flickerInterval;  // Interval between on and off toggles
    private int flickerCount = 0;
    private bool flickering;
    public GameObject Collider;

    public override void OnLookedAt()
    {
        if (flickering == false)
        {
            StartCoroutine(FlickerLight());
            flickering = true;
            Debug.Log("Eyes detected, starting to flicker");
        }
    }

    private IEnumerator FlickerLight()
    {
        float elapsedTime = 0f;  // Track the total time of flickering

        while (elapsedTime < flickerDuration)
        {
            // Toggle the light on and off at regular intervals
            lightToFlicker.enabled = !lightToFlicker.enabled;

            // Wait for the flicker interval before toggling again
            yield return new WaitForSeconds(flickerInterval);

            // Update the elapsed time
            elapsedTime += flickerInterval;
            flickerInterval = Random.Range(0.05f, 1f);
        }

        // Ensure the light is on when the flickering stops (optional)
        lightToFlicker.enabled = true;
        flickering = false;

        Debug.Log("Done flickering");
        if (flickerCount == 1)
        {
            Debug.Log("Destroying Gameobj");
            Destroy(Collider);      //SUICIDE!!!
        }
        else {
            flickerCount++;
        }
    }
}
