// QuickFix p� chat
using UnityEngine;

public class MonsterSoundMovement : MonoBehaviour
{
    public Transform player;        // Reference to the player's Transform
    public float maxSpeed = 2f;     // Maximum movement speed
    public float baseSpeed = 0.1f;  // Minimum movement speed
    public float speedFactor = 0.5f;// How quickly speed increases with distance

    private bool MonsterSoundPlaying = false;

    private void Update()
    {
        if (player != null)
        {
            // Calculate the distance between the monster and the player
            float distance = Vector3.Distance(transform.position, player.position);

            // Calculate dynamic speed: closer means slower, farther means faster (capped by maxSpeed)
            float dynamicSpeed = Mathf.Clamp(baseSpeed + (distance * speedFactor), baseSpeed, maxSpeed);

            // Move the monster towards the player's position
            transform.position = Vector3.MoveTowards(transform.position, player.position, dynamicSpeed * Time.deltaTime);

            // If not playing and distance is between 5 and 35, start sounds
            if (!MonsterSoundPlaying && distance >= 5f && distance < 35f)
            {
                AkSoundEngine.SetRTPCValue("RTPCMonsterState", 0, gameObject);
                AkSoundEngine.PostEvent("Play_Monster_Sounds", gameObject);
                MonsterSoundPlaying = true;
            }
            // If sounds are playing and distance is outside the 5 to 35 range, stop sounds
            else if (MonsterSoundPlaying && (distance < 5f || distance >= 35f))
            {
                AkSoundEngine.PostEvent("Stop_Monster_Sounds", gameObject);
                MonsterSoundPlaying = false;
            }
        }
    }
}