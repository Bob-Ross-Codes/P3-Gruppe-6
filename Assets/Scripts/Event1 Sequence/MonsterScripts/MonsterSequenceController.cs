using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // For NavMeshAgent (if using pathfinding)

public class MonsterSequenceController : MonoBehaviour
{
    public GameObject monsterPrefab; // Assign the monster prefab in the Inspector
    public Transform[] waypoints; // Array of waypoints for the monster to follow
    public float idleDuration = 2.0f; // Idle duration at specified waypoints
    private GameObject spawnedMonster; // Reference to the spawned monster

    private void Start()
    {
        if (waypoints.Length != 4)
        {
            Debug.LogError("Please assign exactly 4 waypoints in the Inspector.");
        }
    }

    public void OnPlayerHidden()
    {
        if (spawnedMonster == null)
        {
            StartCoroutine(SpawnAndControlMonster());
        }
    }

    private IEnumerator SpawnAndControlMonster()
    {
        // Spawn the monster at the first waypoint
        spawnedMonster = Instantiate(monsterPrefab, waypoints[0].position, Quaternion.identity);

        // Get a NavMeshAgent component (if using pathfinding), or any movement component on the monster prefab
        NavMeshAgent agent = spawnedMonster.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("Monster prefab must have a NavMeshAgent component.");
            yield break;
        }

        // Move the monster to each waypoint
        for (int i = 0; i < waypoints.Length; i++)
        {
            agent.SetDestination(waypoints[i].position);

            // Wait until the monster reaches the current waypoint
            while (Vector3.Distance(spawnedMonster.transform.position, waypoints[i].position) > 0.2f)
            {
                yield return null;
            }

            // Idle at waypoint 2 and waypoint 4
            if (i == 1 || i == 3)
            {
                yield return new WaitForSeconds(idleDuration);
            }
        }

        // Destroy the monster after reaching the last waypoint
        Destroy(spawnedMonster);
    }
}













