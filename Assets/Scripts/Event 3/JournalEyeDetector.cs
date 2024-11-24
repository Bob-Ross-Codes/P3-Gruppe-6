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
    public bool jumpScare;
    private bool eyetrackingActivated;

    void Start()
    {
        // Find blurryText in children
        blurryText = GetComponentInChildren<BlurryText>();
        eyetrackingActivated = false;
        jumpScareCount = 3;
        jumpScare = false;
    }

    public void ActivateEyetracking()
    {
        eyetrackingActivated = true;
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
            if (lookAtCount < jumpScareCount)
            {
                lightManager.StartFlicker(4f, 1f, true);
                Debug.Log("lookAtCount: " + lookAtCount);
                patient.transform.localPosition = humanPosition1;
            }
            else if (lookAtCount > jumpScareCount)
                Destroy(patient);
            else { lightManager.StartFlicker(15f, 1f, true); Debug.Log("JumpScare"); }
            lookAtCount++;
        }

    }

    void FixedUpdate()
    {
        if (eyetrackingActivated)
        {
            int randomNumber = Random.Range(0, 3);
            if (lightManager.flickeringOn && !jumpScare)
            {
                Debug.Log("lightsOn value: " + lightManager.lightsOn);
                Debug.Log("random NUmber: " + randomNumber);
                if (lightManager.lightsOn == false && randomNumber == 0)
                    patient.SetActive(true);
                else patient.SetActive(false);
            }
            if (!lightManager.flickeringOn)
                patient.SetActive(false);
        }

        if (jumpScare && patient != null)
        {
            patient.SetActive(true);
            float speed = 300f; // Units per second
            patient.transform.localPosition = Vector3.MoveTowards(patient.transform.localPosition, humanPosition2, speed * Time.deltaTime);
        }
    }
}
