using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agro : GazeActivation
{
    // GazeActivation properties
    public override float ActivationTime => 1f;
    public Animator monsterAnimator;

    public GameObject event1SequenceObject;

    private Event1Sequence event1SequenceScript;

    void Start()
    {
        // Get the Event1Sequence component from the GameObject
        event1SequenceScript = event1SequenceObject.GetComponent<Event1Sequence>();
    }

    public override void OnLookedAt()
    {
        // Ensure the event1SequenceScript is not null before accessing its properties
        if (event1SequenceScript != null && event1SequenceScript.agro)
        {
            monsterAnimator.SetTrigger("Agro"); // Jumpscare animation #Uhyggeligt
        }
    }
}
