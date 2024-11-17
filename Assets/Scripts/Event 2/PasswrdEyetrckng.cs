using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswrdEyetrckng : GazeActivation
{
    public KeypadController keypadController;
    public override float ActivationTime => 0.1f;
    public override void OnLookedAt()
    {
        keypadController.LookAtPassword();
    }
}
