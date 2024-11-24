using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PasswrdEyetrckng : GazeActivation
{
    public KeypadController keypadController;
    [SerializeField] private GameObject flashingImages;

    public override float ActivationTime => 0.1f;
    public override void OnLookedAt()
    {
        if(!flashingImages)
        flashingImages.SetActive(true);
        keypadController.LookAtPassword();
        Debug.Log("Looked At Password");
    }
}
