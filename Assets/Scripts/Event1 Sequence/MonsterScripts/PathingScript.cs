using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the movement of a prefab along a series of waypoints with adjustable speed, acceleration, and deceleration.
/// </summary>
public class PathingScript : MonoBehaviour
{
    [Header("Waypoints Settings")]
    [Tooltip("Array of waypoints that the prefab will move between.")]
    public Transform[] waypoints;

    [Header("Movement Settings")]
    [Tooltip("The base speed of the prefab.")]
    public float speed = 5f;

    [Tooltip("The acceleration factor when approaching a waypoint.")]
    public float acceleration = 2f;

    [Tooltip("The deceleration factor when leaving a waypoint.")]
    public float deceleration = 2f;

    // Index of the current waypoint
    private int currentWaypointIndex = 0;

    // Flag to indicate if the prefab is idle
    private bool isIdle = false;

    /// <summary>
    /// Sets the initial position of the prefab to the first waypoint.
    /// </summary>
    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
        }
        else
        {
            Debug.LogWarning("Waypoints array is empty! The prefab cannot move.");
        }
    }

    /// <summary>
    /// Updates the prefab's movement along the waypoints.
    /// </summary>
    void Update()
    {
        if (isIdle || waypoints.Length == 0)
        {
            // Do nothing if the prefab is idle or there are no waypoints
            return;
        }

        // Calculate the direction to the current waypoint
        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;

        // Calculate the distance to the current waypoint
        float distance = direction.magnitude;

        // Check if the prefab has reached the current waypoint
        if (distance <= 0.1f)
        {
            // If at the second or last waypoint, set to idle
            if (currentWaypointIndex == 1 || currentWaypointIndex == waypoints.Length - 1)
            {
                isIdle = true;
                return;
            }

            // Move to the next waypoint
            currentWaypointIndex++;
        }

        // Calculate the desired speed for the prefab
        float desiredSpeed = CalculateDesiredSpeed(direction);

        // Move the prefab towards the current waypoint
        Vector3 velocity = direction.normalized * desiredSpeed;
        transform.position += velocity * Time.deltaTime;
    }

    /// <summary>
    /// Calculates the desired speed based on the prefab's position relative to the waypoints and its acceleration/deceleration.
    /// </summary>
    /// <param name="direction">The direction vector to the current waypoint.</param>
    /// <returns>The calculated speed value.</returns>
    private float CalculateDesiredSpeed(Vector3 direction)
    {
        float desiredSpeed = speed;

        // Adjust speed near idle waypoints
        if (currentWaypointIndex == 1 || currentWaypointIndex == waypoints.Length - 2)
        {
            float idleDistance = Vector3.Distance(waypoints[1].position, waypoints[waypoints.Length - 2].position);
            float currentDistance = Vector3.Distance(waypoints[currentWaypointIndex].position, transform.position);
            float ratio = currentDistance / idleDistance;

            // Adjust speed based on proximity to idle waypoint
            desiredSpeed *= (1f - Mathf.Abs(2f * ratio - 1f));

            if (currentWaypointIndex == 1 && ratio > 0.5f)
            {
                desiredSpeed *= deceleration; // Decelerate when leaving the first idle waypoint
            }

            if (currentWaypointIndex == waypoints.Length - 2 && ratio < 0.5f)
            {
                desiredSpeed *= acceleration; // Accelerate when approaching the last idle waypoint
            }
        }

        return desiredSpeed;
    }
}