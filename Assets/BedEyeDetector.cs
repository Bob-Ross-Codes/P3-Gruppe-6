using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BedEyeDetector : GazeActivation
{
    public JournalEyeDetector journalEyeDetector;
    public override float ActivationTime => 0.3f;

    public override void OnLookedAt()
    {
        journalEyeDetector.spinBed = false;
        journalEyeDetector.rotationSpeed = 1f;  
        journalEyeDetector.accelerationRate = 1f;
    Debug.Log("spinBed = false");
    }

}
