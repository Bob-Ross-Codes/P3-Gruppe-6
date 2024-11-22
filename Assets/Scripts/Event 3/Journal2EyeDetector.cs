using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Journal2EyeDetector : GazeActivation
{
    public override float ActivationTime => 2f;
    public override void OnLookedAt()
    {
        StartSound();
    }
    public void StopSound()
    {
        Debug.Log("StopWhispers");
    }
    public void StartSound()
    {
        Debug.Log("StartWhispers");
    }
}
