using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in the Inspector
    public Transform player; // Assign the player transform in the Inspector
    public float spawnRadius = 10f; // Radius around the player to spawn enemies
    public int numberOfEnemiesToSpawn = 5; // Number of enemies to keep on the level
    public float enemySpeed = 2f; // Speed of the enemies moving towards the player
    public string enemyTag = "Enemy"; // Tag to assign to the enemies

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        SpawnEnemies(numberOfEnemiesToSpawn);
    }

    void Update()
    {
        // Remove any null enemies from the list (those that have been destroyed)
        spawnedEnemies.RemoveAll(enemy => enemy == null);

        // Check if more enemies need to be spawned
        if (spawnedEnemies.Count < numberOfEnemiesToSpawn)
        {
            int enemiesToSpawn = numberOfEnemiesToSpawn - spawnedEnemies.Count;
            SpawnEnemies(enemiesToSpawn);
        }

        MoveEnemiesTowardsPlayer();
    }

    void SpawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPosition = player.position + (Random.insideUnitSphere * spawnRadius);
            spawnPosition.y = player.position.y; // Keep enemies at the same height as the player
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.tag = enemyTag; // Assign the tag to the enemy
            spawnedEnemies.Add(enemy);
            enemy.AddComponent<Enemy>(); // Ensure each enemy has the Enemy script
        }
    }

    void MoveEnemiesTowardsPlayer()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Vector3 direction = (player.position - enemy.transform.position).normalized;
                enemy.transform.position += direction * enemySpeed * Time.deltaTime;
            }
        }
    }
}
