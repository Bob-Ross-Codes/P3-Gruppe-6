using System.Collections;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Transform[] waypoints;     // Array of waypoints for the path
    private int currentWaypointIndex = 0; // Tracks the current waypoint
    private float speed;               // Speed of movement
    private bool isWaiting = false;    // To check if the monster is in idle mode
    [SerializeField] private int timer = 20;            // Time to wait before resuming movement
    private float idleTimeAtFirstWaypoint;
    private float idleTimeAtLastWaypoint;
    private int lastWaypointIndex = 1;
    private int BacktrackIndex = 2;
    private float PositionIndex = 0.1f;
   

    public void Initialize(Transform[] pathWaypoints, float movementSpeed, float idleTimeAtFirstWaypoint, float idleTimeAtLastWaypoint)
    {
        waypoints = pathWaypoints;
        speed = movementSpeed;
        this.idleTimeAtFirstWaypoint = idleTimeAtFirstWaypoint;
        this.idleTimeAtLastWaypoint = idleTimeAtLastWaypoint;
        transform.position = waypoints[currentWaypointIndex].position;
    }
    void Update()
    {
        if (waypoints == null || waypoints.Length == currentWaypointIndex || isWaiting) return;

        // Move the monster towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // Check if the monster reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < PositionIndex)
        {
            currentWaypointIndex = 0;
            
            if (currentWaypointIndex >= waypoints.Length)
            {
                StartCoroutine(IdleAtWaypoint(idleTimeAtFirstWaypoint));
            }
            else if (currentWaypointIndex == waypoints.Length - lastWaypointIndex)
            {
                StartCoroutine(IdleAtWaypoint(idleTimeAtLastWaypoint, true));
            }
            else
            {
                currentWaypointIndex++;
            }
        }
    }

    private IEnumerator IdleAtWaypoint(float idleTime, bool isLastWaypoint = false)
    {
        isWaiting = true;
        yield return new WaitForSeconds(idleTime);
        isWaiting = false;

        if (isLastWaypoint)
        {
            // Move backwards along the waypoints
            StartCoroutine(MoveBackwards());
        }
        else
        {
            currentWaypointIndex++;
        }
    }

    private IEnumerator MoveBackwards()
    {
        for (int i = waypoints.Length - BacktrackIndex; i >= currentWaypointIndex; i--)  // Skip last waypoint
        {
            while (Vector3.Distance(transform.position, waypoints[i].position) > PositionIndex)
            {
                transform.position = Vector3.MoveTowards(transform.position, waypoints[i].position, speed * Time.deltaTime);
                yield return null;
            }
        }

        Destroy(gameObject);  // Destroy the monster after it exits the room
    }
}

