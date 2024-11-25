using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypdEyetrckng : GazeActivation
{
    public KeypadController keypadController;

    private float activationFloat;
    [SerializeField] private GameObject flashingImages;
    private void Awake()
    {
        activationFloat = 2f;
    }
    public override float ActivationTime => activationFloat;
    public override void OnLookedAt()
    { 
        keypadController.LookAtKeypad();
        if(flashingImages)
        flashingImages.SetActive(false);
        Debug.Log("Looked At Keypad");

        activationFloat += 1f;
    }
}
