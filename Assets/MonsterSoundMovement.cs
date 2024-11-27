//QuickFix på chat
using UnityEngine;

public class MonsterSoundMovement : MonoBehaviour
{
    public Transform player;          // Reference to the player's Transform
    public float maxSpeed= 2f;      // Base speed when far from the player
    public float baseSpeed = 0.1f;       // Maximum speed when near the player
    public float speedFactor = 0.5f;    // Factor to adjust how speed scales with distance
    private bool MonsterSoundPlaying = false;

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

            if (!MonsterSoundPlaying && distance >= 5f && distance < 35f)
            {
                AkSoundEngine.SetRTPCValue("RTPCMonsterState", 0, gameObject);
                AkSoundEngine.PostEvent("Play_Monster_Sounds", gameObject);
                MonsterSoundPlaying = true;
            }
            else if (distance >= 35f && distance < 5f) {
                AkSoundEngine.PostEvent("Stop_Monster_Sounds", gameObject);
                MonsterSoundPlaying = false;
                }


        }
    }
}
