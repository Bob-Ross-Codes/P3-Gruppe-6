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
        AkSoundEngine.PostEvent("Stop_Whispers", gameObject);
    }
    public void StartSound()
    {
        AkSoundEngine.PostEvent("Play_Whispers", gameObject);
    }
}
