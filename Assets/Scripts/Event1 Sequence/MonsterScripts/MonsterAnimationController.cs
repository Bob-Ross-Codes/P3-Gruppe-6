using UnityEngine;

public class MonsterAnimationController : MonoBehaviour
{
    public Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        // Initialize last position to the monster's starting position
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate movement speed by comparing the current position with the last position
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        // Update the "Speed" parameter in the Animator
        animator.SetFloat("Speed", speed);

        // Update last position for the next frame
        lastPosition = transform.position;
    }
}

