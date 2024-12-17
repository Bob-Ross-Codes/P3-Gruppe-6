using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages object spawning as the player progresses through a run scene.
/// Spawns a sequence of objects at a defined spawn point as triggers are hit.
/// Also monitors player blinking (via the Gaze script) to determine if the ending scene should load.
/// </summary>
public class RunHallwayChanger : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("List of objects to spawn at each trigger event.")]
    public List<GameObject> objectsToSpawn;

    [Tooltip("Reference to the spawn point Transform where objects will appear.")]
    public Transform spawnPoint;

    [Tooltip("The prefab to spawn when the trigger counter hits the final threshold.")]
    public GameObject finalPrefab;

    [Header("Gaze Settings")]
    [Tooltip("Reference to the Gaze script for detecting blinking.")]
    [SerializeField] private Gaze gaze;

    [Tooltip("Is the player currently blinking? (Tracked via Gaze)")]
    [SerializeField] private bool isBlinking;

    [Tooltip("Timer for how long the player has been blinking.")]
    [SerializeField] private float blinkTime = 0f;

    // Constants for configuration
    private const int FIRST_SPAWN_INDEX = 0;
    private const int MIN_RANDOM_INDEX = 1;
    private const int MID_SECTION_TRIGGER_THRESHOLD = 5;
    private const int MID_SECTION_START_INDEX = 4;
    private const int FINAL_TRIGGER_THRESHOLD = 6;
    private const float SPAWN_DELAY = 2f;

    private GameObject currentObject; // Reference to the currently spawned object
    private int triggerCounter = 0;   // Counts how many times the player triggered spawns
    private bool isCooldownActive = false; // Ensures a delay between spawns
    private float blinkingTime = 0f;       // Tracks how long the player is blinking

    /// <summary>
    /// Starts the initial chase by spawning the first object, if available.
    /// </summary>
    private void Start()
    {
        StartChase();
    }

    /// <summary>
    /// Spawns the first object from the list to initiate the sequence.
    /// </summary>
    public void StartChase()
    {
        if (objectsToSpawn.Count > 0)
        {
            currentObject = Instantiate(objectsToSpawn[FIRST_SPAWN_INDEX], spawnPoint.position, spawnPoint.rotation);
        }
    }

    /// <summary>
    /// Monitors player blinking and trigger counts each frame.
    /// If the player blinks for more than 3 seconds and triggerCounter > 1, load the Ending scene.
    /// </summary>
    private void Update()
    {
        if (gaze.Blinking)
        {
            blinkTime += Time.deltaTime;
            if (blinkTime > 3f && triggerCounter > 1)
            {
                Debug.Log("Blinking time: " + blinkTime);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
            }
        }
        else
        {
            blinkTime = 0f; // Reset the timer when player is not blinking
        }
    }

    /// <summary>
    /// Trigger event when the player exits a collider.
    /// If the collider is the player and no cooldown is active, spawn the next object and increment triggerCounter.
    /// </summary>
    /// <param name="other">The collider that exited the trigger zone.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isCooldownActive)
        {
            StartCoroutine(SpawnNextObjectWithDelay());
            triggerCounter++;
            Debug.Log("Trigger Counter: " + triggerCounter);
        }
    }

    /// <summary>
    /// Spawns the next object after a short delay, destroying the current object first.
    /// Determines which object to spawn based on triggerCounter.
    /// </summary>
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
        if (nextIndex == -2)
        {
            // Spawn the final prefab
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

    /// <summary>
    /// Determines the index of the next object to spawn based on how many triggers have been activated.
    /// </summary>
    /// <returns>
    /// An integer index representing the next object to spawn,
    /// -2 if the final prefab should be spawned,
    /// or -1 if no valid object is found.
    /// </returns>
    private int DetermineNextIndex()
    {
        if (objectsToSpawn.Count <= 1) return -1;

        if (triggerCounter == FINAL_TRIGGER_THRESHOLD)
        {
            return -2; // Special code to indicate final prefab should be spawned
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