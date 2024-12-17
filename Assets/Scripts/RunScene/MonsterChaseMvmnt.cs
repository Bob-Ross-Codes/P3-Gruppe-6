using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the movement and behavior of a monster chasing the player.
/// Handles waypoint navigation, rotation towards the player, and interaction with sound effects and jumpscares.
/// </summary>
public class MonsterChaseMvmnt : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Array of target BoxColliders for the monster to navigate.")]
    public GameObject[] targetBoxes;

    [Tooltip("Transform of the player.")]
    public Transform player;

    [Tooltip("Transform of the monster.")]
    [SerializeField] private Transform monster;

    [Tooltip("Reference to the JumpscareManager for triggering events.")]
    public JumpscareManager jumpscareManager;

    [Tooltip("Reference to the player GameObject.")]
    public GameObject Player;

    [Header("Movement Settings")]
    [Tooltip("Movement speed of the monster.")]
    [SerializeField] private float _moveSpeed = 20f;

    [Tooltip("Maximum speed of the monster.")]
    [SerializeField] private float monsterMaxSpeed = 15;

    [Tooltip("Rotation speed for smoothly turning towards the player.")]
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Sequence Settings")]
    [Tooltip("Number of targets in the final hallway.")]
    [SerializeField] private int finalHallwayCount = 20;

    private int currentTargetIndex = 0; // Index of the current target in the navigation path
    private bool isMoving = true; // Tracks if the monster is currently moving
    private float destroyTimer = 3f; // Timer before destroying the monster
    private bool isTimerRunning = false; // Tracks if the destroy timer is active

    /// <summary>
    /// Initializes the monster's movement and starts the chase sound effects.
    /// </summary>
    void Start()
    {
        if (targetBoxes.Length != 4)
        {
            Debug.LogError("Please assign exactly 4 BoxColliders.");
        }

        AkSoundEngine.PostEvent("Play_Monster_Chase", gameObject);
        AkSoundEngine.SetRTPCValue("RTPC_MonsterState", 1, Player);
        AkSoundEngine.PostEvent("Play_Monster_Sounds", Player);
    }

    /// <summary>
    /// Updates the monster's position, rotation, and behavior based on proximity to the player.
    /// </summary>
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, monster.position);

        // Adjust the move speed based on distance to the player
        MoveSpeed = distanceToPlayer / 1.2f;

        if (isMoving && targetBoxes.Length == 4)
        {
            MoveToTarget();
            LookAtPlayer();
        }

        // Trigger jumpscare and ending sequence if player is within proximity
        if (distanceToPlayer < 5)
        {
            AkSoundEngine.PostEvent("Play_Death_Jumpscare", Player);
            jumpscareManager.TriggerJumpscare();

            isTimerRunning = true;
        }

        // Handle destroy timer
        if (isTimerRunning)
        {
            destroyTimer -= Time.deltaTime;

            if (destroyTimer <= 0)
            {
                AkSoundEngine.PostEvent("Stop_Whispers", Player);
                Destroy(gameObject);
                UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
            }
        }
    }

    /// <summary>
    /// Moves the monster towards the current target in the navigation path.
    /// Updates the target index when the current target is reached.
    /// </summary>
    private void MoveToTarget()
    {
        Vector3 targetPosition = targetBoxes[currentTargetIndex].transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            currentTargetIndex = (currentTargetIndex + 1) % targetBoxes.Length;
        }
    }

    /// <summary>
    /// Rotates the monster to face the player smoothly.
    /// </summary>
    private void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            targetRotation *= Quaternion.Euler(0, 90, 0); // Adjust rotation for alignment
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Reverses the navigation path for the monster and updates the current target index.
    /// </summary>
    public void ReverseTargetPath()
    {
        System.Array.Reverse(targetBoxes);

        int nextTargetIndex = (currentTargetIndex + 1) % targetBoxes.Length;
        currentTargetIndex = (nextTargetIndex - 1 + targetBoxes.Length) % targetBoxes.Length;

        if (currentTargetIndex == 0)
        {
            currentTargetIndex = targetBoxes.Length - 1;
        }

        Debug.Log($"Target path reversed. New starting index: {currentTargetIndex}");
    }

    /// <summary>
    /// Getter and setter for the monster's movement speed, clamped between 5 and the maximum speed.
    /// </summary>
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = Mathf.Clamp(value, 5f, monsterMaxSpeed); }
    }
}