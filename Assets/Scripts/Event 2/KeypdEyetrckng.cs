using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypdEyetrckng : GazeActivation
{
    public KeypadController keypadController;
    public override float ActivationTime => 2f;
    public override void OnLookedAt()
    { 
        keypadController.LookAtKeypad();
    }
}
