using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1Sequence : MonoBehaviour
{
    public List<Transform> waypoints; // List of waypoints
    public float speed = 2.0f; // Speed of movement
    public float rotationAmount = 100.0f; // Amount of rotation on the y-axis

    [SerializeField] private int currentWaypointIndex = 0;

    public GameObject agroControlller;
    public GameObject deAgro;

    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Count > 0)
        {
            StartCoroutine(MoveToNextWaypoint());
        }
    }

    // Coroutine to move to the next waypoint
    IEnumerator MoveToNextWaypoint()
    {
        while (currentWaypointIndex <= waypoints.Count)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);

            while (distanceToWaypoint > 0.1f)
            {
                //   Debug.Log("Moving to waypoint " + currentWaypointIndex);
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);
                distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);

                yield return null;
            }

            if (currentWaypointIndex == waypoints.Count - 1)
            {
                //  Debug.Log("Rotating at last waypoint");
                transform.Rotate(0, rotationAmount, 0);
            }

            if (currentWaypointIndex == 1 || currentWaypointIndex == 4)
            {
                //Debug.Log("Idling at waypoint " + currentWaypointIndex);
                yield return new WaitForSeconds(6.0f); // Idle for 2 seconds

            }

            currentWaypointIndex++;
            if (currentWaypointIndex == 4)
            {
                yield return new WaitForSeconds(6.0f);
            }
     
            
        }

        Debug.Log("Sequence complete");
        agroControlller.SetActive(true);
        //deAgro.SetActive(true);
    }
}
