//made using ChatGPT and Copilot

using System.Collections;
using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    private Transform[] waypoints;     // Array of waypoints for the path
    private int currentWaypointIndex = 0; // Tracks the current waypoint
    private float speed;               // Speed of movement
    private bool isWaiting = false;    // To check if the monster is in idle mode
    private float idleTimeAtFirstWaypoint;
    private float idleTimeAtLastWaypoint;
    private int lastWaypointIndex = 1;
    private int idleWaypointIndex = 1; // Index of the waypoint where the monster should idle
    private float PositionIndex = 0.1f;
    private float slowdownFactor = 0.5f; // Factor to slow down the movement speed

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

        // Calculate the current movement speed with slowdown effect
        float currentSpeed = isSlowingDown() ? speed * slowdownFactor : speed;

        // Move the monster towards the current waypoint
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, currentSpeed * Time.deltaTime);

        // Check if the monster reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < PositionIndex)
        {
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                StartCoroutine(IdleAtWaypoint(idleTimeAtLastWaypoint, true));
            }
            else if (currentWaypointIndex == idleWaypointIndex)
            {
                StartCoroutine(IdleAtWaypoint(idleTimeAtFirstWaypoint));
            }
            else
            {
                currentWaypointIndex++;
            }
        }
    }

    private bool isSlowingDown()
    {
        // Check if the monster is close to the idle waypoint
        if (currentWaypointIndex == idleWaypointIndex - 1)
        {
            float distanceToIdleWaypoint = Vector3.Distance(transform.position, waypoints[idleWaypointIndex].position);
            return distanceToIdleWaypoint <= PositionIndex;
        }
        return false;
    }

    private IEnumerator IdleAtWaypoint(float idleTime, bool isLastWaypoint = false)
    {
        isWaiting = true;
        yield return new WaitForSeconds(idleTime);
        isWaiting = false;

        if (isLastWaypoint)
        {
            StartCoroutine(MoveBackwards());
        }
        else
        {
            currentWaypointIndex++;
        }
    }

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

        Destroy(gameObject);
        AkSoundEngine.PostEvent("Stop_Monster_Sounds", gameObject);
    }
}

