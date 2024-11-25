using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryWoman : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // Object to spawn
    [SerializeField] private LightManager lightManager; // Reference to the LightManager
    [SerializeField] private float flickerDuration = 2f; // Duration of flickering
    [SerializeField] private float flickerSpeed = 1f; // Speed of flickering
    [SerializeField] private bool includeHandLight = true; // Whether to include the handlight in the flicker
    [SerializeField] private GameObject GazeCollider;

    private bool objectSpawned = false; // Ensure the object spawns only once


    private void Start()
    {
        targetObject.SetActive(true);
        GazeCollider.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !objectSpawned)
        {
            if (lightManager != null)
            {
                // Start the flicker effect
                lightManager.StartFlicker(flickerDuration, flickerSpeed, includeHandLight);

                // Spawn the object after the flicker effect is done
                StartCoroutine(SpawnAfterFlicker());
            }
            else
            {
                Debug.LogError("LightManager is not assigned!");
            }
        }
    }

    private System.Collections.IEnumerator SpawnAfterFlicker()
    {
        // Wait until the flickering is complete
        yield return new WaitForSeconds(flickerDuration);

        // Spawn the object
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            GazeCollider.SetActive(true);
            objectSpawned = true;
        }
        else
        {
            Debug.LogError("Target Object is not assigned!");
        }
    }
}
