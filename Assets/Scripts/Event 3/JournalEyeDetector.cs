using System.Collections;
using Mediapipe.Unity;
using UnityEngine;

public class JournalEyeDetector : GazeActivation
{
    //Eyetracking elements
    public override float ActivationTime => 2f;
    public GameObject EyeDetector;
    //Flickering lights
    public LightManager lightManager;  // Reference to the LightManager script
    [SerializeField] Light[] staticLights;

    private Vector3 humanPosition1 = new Vector3(0.340000004f, -0.113922141f, 5.99638224f);
    private Vector3 humanPosition2 = new Vector3(0.349999994f, -0.0900000036f, 0.629999995f);
    public GameObject patient;
    public BlurryText blurryText;  // Reference to the SpriteFade script

    public int lookAtCount;
    public int jumpScareCount;  

    void Start()
    {
        // Find blurryText in children
        blurryText = GetComponentInChildren<BlurryText>();
        jumpScareCount = 4;
    }

    public void ActivateEyetracking()
    {
        patient.transform.position = humanPosition1;
        Debug.Log("Moving patient");
    }

    public override void OnLookedAt()
    {
        Debug.Log("Looked at Eyedetector");

        if (lightManager.flickeringOn == false)
        {
            blurryText.PauseFadeOut(true);

            Debug.Log("Checking looked at count");
            if (lookAtCount != jumpScareCount)
            {
                lightManager.StartFlicker(4f, 1f, true);
                Debug.Log("lookAtCount: " + lookAtCount);
                patient.transform.position = humanPosition2;
            }
            else { lightManager.StartFlicker(15f, 1f, true); Debug.Log("JumpScare"); }
            lookAtCount++;
        }

    }

    void FixedUpdate()
    {
        if(lightManager.flickeringOn && lookAtCount > 0)
        {
            Debug.Log("lightsOn value: " + lightManager.lightsOn);
            if (lightManager.lightsOn == false && Random.Range(0, 1) == 0)
                patient.SetActive(true);
            else patient.SetActive(false);
        }
        /*
        if (jumpScare && patient != null)
        {
            float speed = 10f; // Units per second
            patient.transform.localPosition = Vector3.MoveTowards(patient.transform.localPosition, humanPosition2, speed * Time.deltaTime);
        }*/
    }
}
