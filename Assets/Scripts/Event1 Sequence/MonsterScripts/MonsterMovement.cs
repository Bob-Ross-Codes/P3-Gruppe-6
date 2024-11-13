//made using ChatGPT and Copilot

using UnityEngine;
using System.Collections;
public class MonsterMovement : MonoBehaviour
{
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float speed;
    private bool isWaiting = false;
    private float idleTimeAtFirstWaypoint;
    private float idleTimeAtLastWaypoint;
    private int idleWaypointIndex = 1;
    private float PositionIndex = 0.1f;
    private float slowdownFactor = 0.5f;

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

        float currentSpeed = isSlowingDown() ? speed * slowdownFactor : speed;
        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, currentSpeed * Time.deltaTime);

        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
        if (direction != Vector3.zero)
        {
            // Create a rotation towards the waypoint, with an offset of 90 degrees on Y
            Quaternion toRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
        }

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
                RotateTowardsNextWaypoint();
            }
        }
    }

    private bool isSlowingDown()
    {
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
            RotateTowardsNextWaypoint();
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

    private void RotateTowardsNextWaypoint()
    {
        if (currentWaypointIndex < waypoints.Length - 1)
        {
            Vector3 direction = waypoints[currentWaypointIndex + 1].position - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 90, 0); // Add Y offset here as well
                transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 5f);
            }
        }
    }
}




