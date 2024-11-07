using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BedEyeDetector : GazeActivation
{
    public override float ActivationTime => 0.3f;
    public JournalEyeDetector journalEyeDetector;

    [SerializeField] public Material blurMaterial; // Assign your blur material here
    private float blurSize = 0f; // Start with no blur
    private float maxBlur = 10f; // Define the maximum blur size
    private float fadeSpeed = 0.5f; // Adjust to control fade speed

    public override void OnLookedAt()
    {
        journalEyeDetector.spinBed = false;
        journalEyeDetector.rotationSpeed = 1f;  
        journalEyeDetector.accelerationRate = 1f;
        Debug.Log("spinBed = false");


        //FADE JOURNAL TO BLUR
        for (int i = 0; blurSize < maxBlur; i++)
        {
            blurSize += Time.deltaTime * fadeSpeed;
            blurMaterial.SetFloat("_BlurSize", blurSize);
        }
    }
}


