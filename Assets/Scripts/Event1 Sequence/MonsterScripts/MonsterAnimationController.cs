using UnityEngine;

/// <summary>
/// Controls the animation of the monster by calculating its movement speed
/// and updating the corresponding Animator parameter.
/// </summary>
public class MonsterAnimationController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Animator component controlling the monster's animations.")]
    public Animator animator;

    private Vector3 lastPosition; // Tracks the monster's position from the last frame

    /// <summary>
    /// Initializes the last position to the monster's starting position.
    /// </summary>
    void Start()
    {
        lastPosition = transform.position; // Set the initial last position
    }

    /// <summary>
    /// Calculates the monster's movement speed and updates the Animator parameter.
    /// </summary>
    void Update()
    {
        // Calculate movement speed as the distance traveled since the last frame
        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;

        // Update the "Speed" parameter in the Animator
        animator.SetFloat("Speed", speed);

        // Store the current position for the next frame's calculation
        lastPosition = transform.position;
    }
}