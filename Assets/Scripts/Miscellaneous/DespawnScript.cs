using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnScript : MonoBehaviour
{
    public GameObject objectToSpawn; // The prefab to instantiate
    public Transform spawnPoint; // The empty GameObject to take position and rotation from

    private void OnTriggerExit(Collider other)
    {
        // Check if the exiting object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Find and destroy all objects with the "DespawnOnTrigger" tag
            GameObject[] objectsToDespawn = GameObject.FindGameObjectsWithTag("DespawnOnTrigger");
            foreach (GameObject obj in objectsToDespawn)
            {
                Destroy(obj);
            }

            // Spawn a new object with the specified prefab, position, and rotation from the spawnPoint
            if (objectToSpawn != null && spawnPoint != null)
            {
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
                Destroy(spawnedObject, 4f); // Destroy the instantiated prefab after 4 seconds
            }
            else
            {
                Debug.LogWarning("No object assigned to spawn or spawn point is missing.");
            }
        }
    }
}
