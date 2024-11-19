using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypdEyetrckng : GazeActivation
{
    public KeypadController keypadController;

    private float activationFloat;
    private void Awake()
    {
        activationFloat = 2f;
    }
    public override float ActivationTime => activationFloat;
    public override void OnLookedAt()
    { 
        keypadController.LookAtKeypad();
        Debug.Log("Looked At Keypad");

        activationFloat += 0.5f;
    }
}
