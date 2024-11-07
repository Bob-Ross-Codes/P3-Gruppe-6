using System.Collections;
using UnityEngine;

public class MonsterSequenceController : MonoBehaviour
{
    public GameObject monsterPrefab;        // Prefab of the monster to spawn
    public Transform[] waypoints;           // Waypoints for the monster to follow
    public float monsterSpeed = 2f;         // Speed of the monster movement

    public AudioClip doorBustSound;         // AudioClip for door bust sound
    private AudioSource audioSource;        // AudioSource for playing sounds

    private bool playerIsHidden = false;
    [SerializeField] private float SpawnRotation = 270f;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // Add AudioSource component if not already attached
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnPlayerHidden()
    {
        if (!playerIsHidden)
        {
            playerIsHidden = true;
            StartCoroutine(StartMonsterSequence());
        }
    }

    private IEnumerator StartMonsterSequence()
    {
        // Wait a few seconds for suspense
        yield return new WaitForSeconds(3);

        // Play the door bust sound
        audioSource.PlayOneShot(doorBustSound);

        // Wait for a short time, then spawn the monster
        yield return new WaitForSeconds(1);

        SpawnMonster();
    }

    private void SpawnMonster()
    {
        GameObject monster = Instantiate(monsterPrefab, waypoints[0].position, Quaternion.Euler(0f, SpawnRotation, 0f));
        MonsterMovement monsterMovement = monster.AddComponent<MonsterMovement>();

        // Set idle times at waypoints
        float idleTimeAtFirstWaypoint = 7f;  // Set idle time at the first waypoint
        float idleTimeAtLastWaypoint = 2f;   // Set idle time at the last waypoint

        // Initialize monster movement with waypoints, speed, and idle times
        monsterMovement.Initialize(waypoints, monsterSpeed, idleTimeAtFirstWaypoint, idleTimeAtLastWaypoint);
    }
}




