using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a sequence of events where an object moves between waypoints, rotates, and idles at specified points.
/// Triggers additional behaviors upon sequence completion.
/// </summary>
public class Event1Sequence : MonoBehaviour
{
    [Header("Waypoint Settings")]
    [Tooltip("List of waypoints the object will move between.")]
    public List<Transform> waypoints;

    [Tooltip("Speed at which the object moves.")]
    public float speed = 2.0f;

    [Tooltip("Amount of rotation (in degrees) on the y-axis at the final waypoint.")]
    public float rotationAmount = 100.0f;

    [Header("Sequence Controls")]
    [Tooltip("Index of the current waypoint the object is moving toward.")]
    [SerializeField] private int currentWaypointIndex = 0;

    [Header("GameObject Triggers")]
    [Tooltip("GameObject to activate after the sequence is complete.")]
    public GameObject agroControlller;

    [Tooltip("Optional GameObject to deactivate during the sequence.")]
    public GameObject deAgro;

    /// <summary>
    /// Starts the sequence if waypoints are available.
    /// </summary>
    void Start()
    {
        if (waypoints.Count > 0)
        {
            StartCoroutine(MoveToNextWaypoint());
        }
    }

    /// <summary>
    /// Coroutine that handles movement between waypoints, idle behavior, and rotation.
    /// Activates specific GameObjects upon completion.
    /// </summary>
    private IEnumerator MoveToNextWaypoint()
    {
        while (currentWaypointIndex <= waypoints.Count)
        {
            // Target the current waypoint
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);

            // Move towards the waypoint
            while (distanceToWaypoint > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
                distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
                yield return null;
            }

            // Rotate at the last waypoint
            if (currentWaypointIndex == waypoints.Count - 1)
            {
                transform.Rotate(0, rotationAmount, 0);
            }

            // Idle at specified waypoints
            if (currentWaypointIndex == 1 || currentWaypointIndex == 4)
            {
                yield return new WaitForSeconds(6.0f);
            }

            // Increment waypoint index
            currentWaypointIndex++;

            // Additional idle behavior at waypoint 4
            if (currentWaypointIndex == 4)
            {
                yield return new WaitForSeconds(6.0f);
            }
        }

        Debug.Log("Sequence complete");

        // Activate the agro controller upon completion
        if (agroControlller != null)
        {
            agroControlller.SetActive(true);
        }

        // Optional deAgro activation (commented out)
        //if (deAgro != null)
        //{
        //    deAgro.SetActive(true);
        //}
    }
}