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
    private bool isFirstSpawn = true; // Track if it's the first spawn
    private int triggerCounter = 0; // Counter to track how many times the trigger has been entered
    private int SpawnPartOneMax = 3; // Number of times before spawning the last object in the list 







// instantiate the first object in the list
    public void StartChase()
    {
        if (objectsToSpawn.Count > 0)
        {
            currentObject = Instantiate(objectsToSpawn[currentIndex], spawnPoint.position, spawnPoint.rotation);
            currentIndex++;
            isFirstSpawn = false; // Track if it's the first spawn
        }
    }

    private void Start()
    {
        StartChase();
    }


//Trigger the next object
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isCooldownActive) // Check if player exits and cooldown is not active
        {
            StartCoroutine(SpawnNextObjectWithDelay());
            triggerCounter++;
            Debug.Log("Trigger Counter: " + triggerCounter);
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


        // Wait for 4 seconds
        yield return new WaitForSeconds(2f);


        // Check if it's the first spawn
        if (isFirstSpawn)
        {
            currentObject = Instantiate(objectsToSpawn[0], spawnPoint.position, spawnPoint.rotation);
            isFirstSpawn = false; // Set to false after the first spawn
        }
        else
        {
            // Instantiate a random object from the list from 1 to 5 if the triggerCounter is less than 6, excluding the first one
            if (triggerCounter < 6 && objectsToSpawn.Count > 1)
            {
            int randomIndex = Random.Range(1, Mathf.Min(objectsToSpawn.Count, 6)); // Only from index 1 to 5, as index 0 is used for the first spawn
            currentObject = Instantiate(objectsToSpawn[randomIndex], spawnPoint.position, spawnPoint.rotation);
            }
            else if (triggerCounter >= 6 && objectsToSpawn.Count > 6)
            {
            int randomIndex = Random.Range(6, objectsToSpawn.Count); // Only from index 6 and up
            currentObject = Instantiate(objectsToSpawn[randomIndex], spawnPoint.position, spawnPoint.rotation);
            }
            else if (objectsToSpawn.Count > 1)
            {
            int randomIndex = Random.Range(1, objectsToSpawn.Count); // Only from index 1 and up, as index 0 is used for the first spawn
            currentObject = Instantiate(objectsToSpawn[randomIndex], spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
            Debug.Log("No objects to spawn.");
            }
        }

        isCooldownActive = false; // Reset cooldown
    }
}

