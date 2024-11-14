using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNewSpawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject monster;
    [SerializeField] private GameObject[] monsterSpawners;

    // Static variable shared across all instances
    public static int cornerTurned = 0; // Counter for the number of corners turned
    public static int cornersTurnedUntilChange = 7;
    private string lastCollisionTag; // To store the tag of the last collision when cornerTurned is 7
    private bool hasTriggered = false; // Flag to control re-entry
    public MonsterChaseMvmnt monsterChaseMvmnt;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object's tag is "Player" and if this trigger hasn’t been activated yet
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Set flag to prevent re-triggering
            cornerTurned++; // Increment the cornerTurned counter
            
            // If the player has turned the specified number of corners
            if (cornerTurned == cornersTurnedUntilChange)
            {
                Debug.Log("Monster has turned " + cornersTurnedUntilChange + " corners.");
                lastCollisionTag = gameObject.tag; // Store the tag of the colliding object

                // Move monster based on last collision tag
                if (lastCollisionTag == "Target 1")
                    {foreach (GameObject spawner in monsterSpawners) {if (spawner.CompareTag("Target 4")) {monster.transform.position = spawner.transform.position;}}}
                if (lastCollisionTag == "Target 2")
                    {foreach (GameObject spawner in monsterSpawners) {if (spawner.CompareTag("Target 1")) {monster.transform.position = spawner.transform.position;}}}
                if (lastCollisionTag == "Target 3")
                    {foreach (GameObject spawner in monsterSpawners) {if (spawner.CompareTag("Target 2")) {monster.transform.position = spawner.transform.position;}}}
                if (lastCollisionTag == "Target 4")
                    {foreach (GameObject spawner in monsterSpawners) {if (spawner.CompareTag("Target 3")) {monster.transform.position = spawner.transform.position;}}}

                monsterChaseMvmnt.ReverseTargetPath();
            }
            Debug.Log("Corner turned: " + cornerTurned);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset the trigger flag when the player exits the collider
        if (other.CompareTag("Player"))
        {
            hasTriggered = false;
        }
    }
}
