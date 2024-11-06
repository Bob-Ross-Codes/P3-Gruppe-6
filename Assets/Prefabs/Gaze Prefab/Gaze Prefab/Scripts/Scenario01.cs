using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenario01 : GazeActivation
{
    // Provide the specific activation time for this scenario
    public override float ActivationTime => 2.0f; // Example: 2 seconds

    public override void OnLookedAt()
    {
        // Code to execute when the object is looked at for the required time
        Debug.Log("Scenario01 activated!");
    }
}
