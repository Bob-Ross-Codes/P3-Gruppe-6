using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2EyeDetector : GazeActivation
{
    public override float ActivationTime => 0.2f;
    [SerializeField] private Journal2EyeDetector journal2EyeDetector;

    public override void OnLookedAt()
    {
        journal2EyeDetector.StopSound();
    }

}
