using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingScript : MonoBehaviour
{
    public Transform[] waypoints; // Array to store the waypoints
    public float speed = 5f; // Speed of the prefab
    public float acceleration = 2f; // Acceleration of the prefab
    public float deceleration = 2f; // Deceleration of the prefab

    private int currentWaypointIndex = 0; // Index of the current waypoint
    private bool isIdle = false; // Flag to indicate if the prefab is idle

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial position of the prefab to the first waypoint
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the prefab is idle
        if (isIdle)
        {
            // Do nothing if the prefab is idle
            return;
        }

        // Calculate the direction to the current waypoint
        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;

        // Calculate the distance to the current waypoint
        float distance = direction.magnitude;

        // Check if the prefab has reached the current waypoint
        if (distance <= 0.1f)
        {
            // Check if the current waypoint is the second or last waypoint
            if (currentWaypointIndex == 1 || currentWaypointIndex == waypoints.Length - 1)
            {
                // Set the prefab to idle
                isIdle = true;
                return;
            }

            // Move to the next waypoint
            currentWaypointIndex++;
        }

        // Calculate the desired velocity based on the speed and direction
        float desiredSpeed = speed;

        // Check if the prefab is approaching or leaving an idle waypoint
        if (currentWaypointIndex == 1 || currentWaypointIndex == waypoints.Length - 2)
        {
            // Calculate the distance to the idle waypoint
            float idleDistance = Vector3.Distance(waypoints[1].position, waypoints[waypoints.Length - 2].position);

            // Calculate the distance to the current waypoint
            float currentDistance = Vector3.Distance(waypoints[currentWaypointIndex].position, transform.position);

            // Calculate the ratio of the current distance to the idle distance
            float ratio = currentDistance / idleDistance;

            // Calculate the desired speed based on the ratio and acceleration/deceleration
            desiredSpeed = speed * (1f - Mathf.Abs(2f * ratio - 1f));

            // Check if the prefab is leaving the idle waypoint
            if (currentWaypointIndex == 1 && ratio > 0.5f)
            {
                // Decelerate the prefab
                desiredSpeed *= deceleration;
            }

            // Check if the prefab is approaching the idle waypoint
            if (currentWaypointIndex == waypoints.Length - 2 && ratio < 0.5f)
            {
                // Accelerate the prefab
                desiredSpeed *= acceleration;
            }
        }

        // Calculate the velocity based on the desired speed and direction
        Vector3 velocity = direction.normalized * desiredSpeed;

        // Move the prefab towards the current waypoint
        transform.position += velocity * Time.deltaTime;
    }
}
