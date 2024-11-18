using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunHallwayChanger : MonoBehaviour
{


    [Header("Spawn Settings")]
    public List<GameObject> objectsToSpawn; // List of objects to spawn
    public Transform spawnPoint; // Reference to the spawn point
    public GameObject finalPrefab; // The specific prefab to spawn when the trigger counter hits the max
    private Gaze gaze;
    [SerializeField] bool isBlinking;



    // Constants for configuration
    private const int FIRST_SPAWN_INDEX = 0;
    private const int MIN_RANDOM_INDEX = 1;
    private const int MID_SECTION_TRIGGER_THRESHOLD = 5;
    private const int MID_SECTION_START_INDEX = 4;
    private const int FINAL_TRIGGER_THRESHOLD = 6;
    private const float SPAWN_DELAY = 2f;



    private GameObject currentObject; // Reference to the currently instantiated object
    private int triggerCounter = 0; // Counter for trigger events
    private bool isCooldownActive = false; // Track if cooldown is active
    private float blinkingTime = 0f; // Timer to track how long isBlinking is true




    
    private void Start()
    {
        StartChase();
        isBlinking = gaze.Blinking;
    }


    // Start spawning process with the first object
    public void StartChase()
    {
        if (objectsToSpawn.Count > 0)
        {
            currentObject = Instantiate(objectsToSpawn[FIRST_SPAWN_INDEX], spawnPoint.position, spawnPoint.rotation);
        }
    }


//trigger next scene if isBlinking bool is true for more then 3 seconds
    private void Update()
    {
        if (isBlinking)
        {
            blinkingTime += Time.deltaTime; // Increment timer if isBlinking is true
            if (blinkingTime > 3f && triggerCounter > 1) // Check both conditions
            {
                Debug.Log("Blinking time: " + blinkingTime);
            }
        }
        else
        {
            blinkingTime = 0f; // Reset timer if isBlinking is false
            Debug.Log("Blinking time reset");
        }
    }


    // Trigger event to handle object spawning when player exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isCooldownActive)
        {
            StartCoroutine(SpawnNextObjectWithDelay());
            triggerCounter++;
            Debug.Log("Trigger Counter: " + triggerCounter);
        }
    }

    // Coroutine to spawn the next object with a delay
    private IEnumerator SpawnNextObjectWithDelay()
    {
        isCooldownActive = true;
        yield return new WaitForSeconds(SPAWN_DELAY);

        // Destroy the current object if it exists
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        // Determine the next object index to spawn
        int nextIndex = DetermineNextIndex();
        if (nextIndex == -2) // Special case for final prefab
        {
            currentObject = Instantiate(finalPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else if (nextIndex >= 0)
        {
            currentObject = Instantiate(objectsToSpawn[nextIndex], spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.Log("No valid object to spawn.");
        }

        isCooldownActive = false;
    }

    // Determine the next object index based on trigger count and object list length
    private int DetermineNextIndex()
    {
        if (objectsToSpawn.Count <= 1) return -1;

        if (triggerCounter == FINAL_TRIGGER_THRESHOLD)
        {
            return -2; // Special code to signal the final prefab should be spawned
        }
        else if (triggerCounter < MID_SECTION_TRIGGER_THRESHOLD && objectsToSpawn.Count > 1)
        {
            return Random.Range(MIN_RANDOM_INDEX, Mathf.Min(objectsToSpawn.Count, MID_SECTION_START_INDEX));
        }
        else if (triggerCounter >= MID_SECTION_TRIGGER_THRESHOLD && objectsToSpawn.Count > MID_SECTION_START_INDEX)
        {
            return Random.Range(MID_SECTION_START_INDEX, objectsToSpawn.Count);
        }
        else
        {
            return Random.Range(MIN_RANDOM_INDEX, objectsToSpawn.Count);
        }
    }
}
