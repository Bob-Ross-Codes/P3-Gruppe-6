using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHallwayChanger : MonoBehaviour
{
    public List<GameObject> objectsToSpawn; // List of objects to spawn
    public Transform spawnPoint; // Reference to the spawn point
    private GameObject currentObject; // Reference to the currently instantiated object
    private int currentIndex = 0; // Index to track the current object in the list
    private bool isCooldownActive = false; // Track if delay is active

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isCooldownActive) // Check if player exits and cooldown is not active
        {
            StartCoroutine(SpawnNextObjectWithDelay());
        }
    }

    private IEnumerator SpawnNextObjectWithDelay()
    {
        isCooldownActive = true; // Set cooldown to active

        // Destroy the currently instantiated object if it exists
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Instantiate the next object in the list if it exists
        if (currentIndex < objectsToSpawn.Count)
        {
            currentObject = Instantiate(objectsToSpawn[currentIndex], spawnPoint.position, spawnPoint.rotation);
            currentIndex++;
        }
        else
        {
            Debug.Log("No more objects to spawn.");
        }

        isCooldownActive = false; // Reset cooldown
    }
}
