using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the monster's spawning behavior and movement based on the player's progress through the scene.
/// Tracks how many corners the player has turned and moves the monster accordingly.
/// </summary>
public class MonsterNewSpawn : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The player GameObject.")]
    [SerializeField] private GameObject player;

    [Tooltip("The monster GameObject.")]
    [SerializeField] private GameObject monster;

    [Tooltip("Array of monster spawners in the scene.")]
    [SerializeField] private GameObject[] monsterSpawners;

    [Tooltip("Reference to the monster's chase movement script.")]
    public MonsterChaseMvmnt monsterChaseMvmnt;

    [Header("Corner Tracking")]
    [Tooltip("Counter for the number of corners the player has turned.")]
    public static int cornerTurned = 0;

    [Tooltip("Number of corners the player needs to turn before triggering a monster spawn.")]
    public static int cornersTurnedUntilChange = 7;

    private string lastCollisionTag; // Stores the tag of the last collision when cornerTurned reaches the threshold
    private bool hasTriggered = false; // Flag to prevent re-triggering the collider logic

    /// <summary>
    /// Triggered when the player enters the collider.
    /// Tracks corner turns and moves the monster when the threshold is reached.
    /// </summary>
    /// <param name="other">The collider that entered the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player and the trigger hasn't been activated yet
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Prevent re-triggering
            cornerTurned++; // Increment the corner turn counter
            
            Debug.Log("Corner turned: " + cornerTurned);

            // If the player has turned the specified number of corners
            if (cornerTurned == cornersTurnedUntilChange)
            {
                Debug.Log("Monster has turned " + cornersTurnedUntilChange + " corners.");
                lastCollisionTag = gameObject.tag; // Store the tag of the triggering object

                // Move the monster to a new spawner based on the last collision tag
                foreach (GameObject spawner in monsterSpawners)
                {
                    if (lastCollisionTag == "Target 1" && spawner.CompareTag("Target 4"))
                    {
                        monster.transform.position = spawner.transform.position;
                    }
                    else if (lastCollisionTag == "Target 2" && spawner.CompareTag("Target 1"))
                    {
                        monster.transform.position = spawner.transform.position;
                    }
                    else if (lastCollisionTag == "Target 3" && spawner.CompareTag("Target 2"))
                    {
                        monster.transform.position = spawner.transform.position;
                    }
                    else if (lastCollisionTag == "Target 4" && spawner.CompareTag("Target 3"))
                    {
                        monster.transform.position = spawner.transform.position;
                    }
                }

                // Reverse the monster's path
                monsterChaseMvmnt.ReverseTargetPath();
            }
        }
    }

    /// <summary>
    /// Triggered when the player exits the collider.
    /// Resets the trigger flag to allow re-entry.
    /// </summary>
    /// <param name="other">The collider that exited the trigger zone.</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasTriggered = false; // Reset the trigger flag
        }
    }
}