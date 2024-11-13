using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSequenceController : MonoBehaviour
{
    public GameObject monster; // Assign the existing monster GameObject in the Inspector
    public Event1Sequence event1Sequence; // Reference to the Event1Sequence script


    public void OnPlayerHidden()
    {

        monster.SetActive(true);
        event1Sequence.StartCoroutine("MoveToNextWaypoint");

    }
}
