using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles despawning objects and spawning a new prefab when the player exits a trigger area.
/// </summary>
public class DespawnScript : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject objectToSpawn; // The prefab to instantiate
    [SerializeField] private Transform spawnPoint; // The position and rotation for the new prefab

    /// <summary>
    /// Called when an object exits the trigger collider.
    /// Despawns tagged objects and spawns a new prefab if the exiting object is the player.
    /// </summary>
    /// <param name="other">The collider that exited the trigger area.</param>
    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object is the player
        if (other.CompareTag("Player"))
        {
            // Find and destroy all objects with the "DespawnOnTrigger" tag
            GameObject[] objectsToDespawn = GameObject.FindGameObjectsWithTag("DespawnOnTrigger");
            foreach (GameObject obj in objectsToDespawn)
            {
                Destroy(obj);
            }

            // Spawn a new object at the specified spawn point
            if (objectToSpawn != null && spawnPoint != null)
            {
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
                Destroy(spawnedObject, 4f); // Automatically destroy the spawned object after 4 seconds
            }
            else
            {
                Debug.LogWarning("No object assigned to spawn or spawn point is missing.");
            }
        }
    }
}