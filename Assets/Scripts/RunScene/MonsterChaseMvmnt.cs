using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonsterChaseMvmnt : MonoBehaviour
{
    public GameObject[] targetBoxes; // Array to store the four BoxColliders
    public Transform player;
    [SerializeField] private Transform monster;

    [SerializeField] private float _moveSpeed = 20f;      // Private backing field for moveSpeed
    [SerializeField] private float monsterMaxSpeed = 15;
    private int currentTargetIndex = 0;  // To keep track of the current target (BoxCollider)
    private bool isMoving = true;        // Flag to control the movement
    [SerializeField] int finalHallwayCount = 20;
    public JumpscareManager jumpscareManager;
    public GameObject Player;

    private bool timerIsRunning = false;
    private float loadTimer = 3f;



    [SerializeField] private float rotationSpeed = 5f; // New variable to control rotation speed

    void Start()
    {
        // Ensure we have exactly 4 BoxColliders assigned in the inspector
        if (targetBoxes.Length != 4)
        {
            Debug.LogError("Please assign exactly 4 BoxColliders.");
        }
        AkSoundEngine.PostEvent("Play_Monster_Chase", gameObject);
        AkSoundEngine.SetRTPCValue("RTPC_MonsterState", 1, Player);
        AkSoundEngine.PostEvent("Play_Monster_Sounds", Player);
    }

    void Update()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(player.position, monster.position);

        // Set the moveSpeed based on the distance to the player, but cap it at monsterMaxSpeed
        MoveSpeed = distanceToPlayer / 1.2f;

        if (isMoving && targetBoxes.Length == 4)
        {
            MoveToTarget();
            LookAtPlayer();
        }
        /*if(isMoving && targetBoxes.Length == finalHallwayCount)
        {
            //runHallwayChanger.finalHallway();
        }*/

        if (distanceToPlayer < 5)
        {
            // Player is close enough, triggering death or something else
            if (timerIsRunning == false)
            {


                AkSoundEngine.PostEvent("Play_Death_Jumpscare", Player);
                jumpscareManager.TriggerJumpscare();


                timerIsRunning = true;
            }
        }

        if (timerIsRunning == true)
        {
            loadTimer -= Time.deltaTime;


            if (loadTimer <= 0)
            {

                UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");

            }

        }
    }

    // Function to move the object towards the current target BoxCollider
    private void MoveToTarget()
    {
        Debug.Log("Moving Towards " + currentTargetIndex);
        // Get the position of the current target BoxCollider
        Vector3 targetPosition = targetBoxes[currentTargetIndex].transform.position;

        // Move towards the target using MoveTowards for smooth movement
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, MoveSpeed * Time.deltaTime);

        // Check if the object has reached the target position
        if (transform.position == targetPosition)
        {
            Debug.Log("Index " + currentTargetIndex + "is done");
            // Move to the next target BoxCollider (loop back to 0 if we reach the last one)
            currentTargetIndex = (currentTargetIndex + 1) % targetBoxes.Length;

            Debug.Log("Moving on to index:" + currentTargetIndex);
        }
    }

    // Function to rotate the object to face the player
    private void LookAtPlayer()
    {
        if (player != null)
        {
            // Calculate the direction from the object to the player
            Vector3 directionToPlayer = player.transform.position - transform.position;

            // Calculate the rotation needed to look at the player only on the y axis
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);

            // Adjust the rotation by -90 degrees on the y-axis
            targetRotation *= Quaternion.Euler(0, 90, 0);

            // Smoothly rotate the object towards the player using Slerp with increased rotation speed
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Getter and Setter for moveSpeed with a cap of 30
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = Mathf.Clamp(value, 5f, monsterMaxSpeed); }  // Clamps the speed between 5 and monsterMaxSpeed
    }

    // Method to reverse the target path (this method will reverse the order of targets in the array)
    public void ReverseTargetPath()
    {
        // Reverse the order of the targetBoxes array
        System.Array.Reverse(targetBoxes);

        // Find the next target index in the original array (before reversing)
        int nextTargetIndex = (currentTargetIndex + 1) % targetBoxes.Length;

        // Set the new starting index to be one less than the next target index (or wrap around to 3 if next is 0)
        currentTargetIndex = (nextTargetIndex - 1 + targetBoxes.Length) % targetBoxes.Length;

        // If the next target was 0, move to the last target (index 3)
        if (currentTargetIndex == 0)
        {
            currentTargetIndex = targetBoxes.Length - 1;
        }

        // Debug log to check the target reversal and new index
        Debug.Log($"Target path reversed. New starting index: {currentTargetIndex}");
    }
}
