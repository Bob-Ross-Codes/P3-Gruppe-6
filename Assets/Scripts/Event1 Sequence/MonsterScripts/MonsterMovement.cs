// Made using ChatGPT and Copilot

using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the monster's movement along a set of waypoints, including idle behavior at specific waypoints,
/// smooth rotation towards waypoints, and backward movement when the sequence completes.
/// </summary>
public class MonsterMovement : MonoBehaviour
{
    private Transform[] waypoints; // Array of waypoints defining the monster's path
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private float speed; // Movement speed of the monster
    private bool isWaiting = false; // Tracks if the monster is currently waiting at a waypoint
    private float idleTimeAtFirstWaypoint; // Idle time at the first waypoint
    private float idleTimeAtLastWaypoint; // Idle time at the last waypoint
    private int idleWaypointIndex = 1; // The waypoint index where the monster will idle
    private float PositionIndex = 0.1f; // Threshold distance to consider the monster as having reached a waypoint
    private float slowdownFactor = 0.5f; // Speed reduction factor for slowing down near idle waypoints

    /// <summary>
    /// Initializes the monster's path and movement parameters.
    /// </summary>
    /// <param name="pathWaypoints">Array of waypoints for the monster's path.</param>
    /// <param name="movementSpeed">Speed of the monster's movement.</param>
    /// <param name="idleTimeAtFirstWaypoint">Idle duration at the first waypoint.</param>
    /// <param name="idleTimeAtLastWaypoint">Idle duration at the last waypoint.</param>
    public void Initialize(Transform[] pathWaypoints, float movementSpeed, float idleTimeAtFirstWaypoint, float idleTimeAtLastWaypoint)
    {
        waypoints = pathWaypoints;
        speed = movementSpeed;
        this.idleTimeAtFirstWaypoint = idleTimeAtFirstWaypoint;
        this.idleTimeAtLastWaypoint = idleTimeAtLastWaypoint;
        transform.position = waypoints[currentWaypointIndex].position; // Set initial position
    }

    /// <summary>
    /// Updates the monster's position and rotation each frame, moving it towards the next waypoint.
    /// </summary>
    void Update()
    {
        if (waypoints == null || waypoints.Length == currentWaypointIndex || isWaiting) return;

        // Adjust speed if the monster is slowing down
        float currentSpeed = isSlowingDown() ? speed * slowdownFactor : speed;
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, currentSpeed * Time.deltaTime);

        // Smoothly rotate the monster to face the next waypoint
        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
        }

        // Check if the monster has reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < PositionIndex)
        {
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                StartCoroutine(IdleAtWaypoint(idleTimeAtLastWaypoint, true)); // Idle at the last waypoint
            }
            else if (currentWaypointIndex == idleWaypointIndex)
            {
                StartCoroutine(IdleAtWaypoint(idleTimeAtFirstWaypoint)); // Idle at the first waypoint
            }
            else
            {
                currentWaypointIndex++; // Move to the next waypoint
                RotateTowardsNextWaypoint();
            }
        }
    }

    /// <summary>
    /// Determines whether the monster should slow down near an idle waypoint.
    /// </summary>
    /// <returns>True if the monster is slowing down, false otherwise.</returns>
    private bool isSlowingDown()
    {
        if (currentWaypointIndex == idleWaypointIndex - 1)
        {
            float distanceToIdleWaypoint = Vector3.Distance(transform.position, waypoints[idleWaypointIndex].position);
            return distanceToIdleWaypoint <= PositionIndex;
        }
        return false;
    }

    /// <summary>
    /// Makes the monster idle at a waypoint for a specified duration.
    /// </summary>
    /// <param name="idleTime">Duration of the idle behavior.</param>
    /// <param name="isLastWaypoint">True if the monster is idling at the last waypoint.</param>
    private IEnumerator IdleAtWaypoint(float idleTime, bool isLastWaypoint = false)
    {
        isWaiting = true;
        yield return new WaitForSeconds(idleTime);
        isWaiting = false;

        if (isLastWaypoint)
        {
            StartCoroutine(MoveBackwards()); // Move backwards to the starting position
        }
        else
        {
            currentWaypointIndex++;
            RotateTowardsNextWaypoint();
        }
    }

    /// <summary>
    /// Moves the monster backward along the waypoints after completing the forward path.
    /// </summary>
    private IEnumerator MoveBackwards()
    {
        for (int i = waypoints.Length - 2; i >= 0; i--)
        {
            while (Vector3.Distance(transform.position, waypoints[i].position) > PositionIndex)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[i].position, speed * Time.deltaTime);
                yield return null;
            }
        }

        Destroy(gameObject); // Destroy the monster when it reaches the starting position
    }

    /// <summary>
    /// Smoothly rotates the monster to face the next waypoint.
    /// </summary>
    private void RotateTowardsNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length - 1)
        {
            Vector3 direction = waypoints[currentWaypointIndex + 1].position - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 5f);
            }
        }
    }
}