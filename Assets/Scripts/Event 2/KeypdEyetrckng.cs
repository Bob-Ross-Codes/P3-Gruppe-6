using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypdEyetrckng : GazeActivation
{
    public KeypadController keypadController;

    [SerializeField] private GameObject flashingImages;

    public override float ActivationTime => 2.5f;

    public override void OnLookedAt()
    { 
        keypadController.LookAtKeypad();
        if(flashingImages)
        flashingImages.SetActive(false);
        Debug.Log("Looked At Keypad");

    }
}
