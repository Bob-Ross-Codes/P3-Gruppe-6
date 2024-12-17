using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the monster's behavior during the Event1 sequence.
/// Activates the monster and triggers its movement when the player hides.
/// </summary>
public class MonsterSequenceController : MonoBehaviour
{
    [Header("Monster Settings")]
    [Tooltip("Reference to the existing monster GameObject.")]
    public GameObject monster;

    [Header("Event Sequence Reference")]
    [Tooltip("Reference to the Event1Sequence script that manages the monster's movement.")]
    public Event1Sequence event1Sequence;

    /// <summary>
    /// Called when the player successfully hides. Activates the monster and starts its movement to the next waypoint.
    /// </summary>
    public void OnPlayerHidden()
    {
        // Activate the monster GameObject
        if (monster != null)
        {
            monster.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Monster GameObject is not assigned.");
        }

        // Start the monster's movement to the next waypoint
        if (event1Sequence != null)
        {
            event1Sequence.StartCoroutine("MoveToNextWaypoint");
        }
        else
        {
            Debug.LogWarning("Event1Sequence script reference is not assigned.");
        }
    }
}