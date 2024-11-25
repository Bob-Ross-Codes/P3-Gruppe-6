//QuickFix på chat
using UnityEngine;

public class MonsterSoundMovement : MonoBehaviour
{
    public Transform player;          // Reference to the player's Transform
    public float baseSpeed = 2f;      // Base speed when far from the player
    public float maxSpeed = 5f;       // Maximum speed when near the player
    public float speedFactor = 1f;    // Factor to adjust how speed scales with distance

    private void Update()
    {
        // Ensure the player reference is assigned
        if (player != null)
        {
            // Calculate the distance between the monster and the player
            float distance = Vector3.Distance(transform.position, player.position);

            // Calculate a dynamic speed based on the distance
            // Speed is inversely proportional to distance (closer -> slower)
            float dynamicSpeed = Mathf.Clamp(baseSpeed + (distance * speedFactor), baseSpeed, maxSpeed);

            // Move the monster towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, dynamicSpeed * Time.deltaTime);
        }
    }
}
