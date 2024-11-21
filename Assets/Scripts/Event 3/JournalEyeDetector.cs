using System.Collections;
using Mediapipe.Unity;
using UnityEngine;

public class JournalEyeDetector : GazeActivation
{
    //Eyetracking elements
    public override float ActivationTime => 3f;
    public GameObject EyeDetector;
    //Flickering lights
    public LightManager lightManager;  // Reference to the LightManager script
    [SerializeField] Light[] staticLights;

    public int flickerCount = 1;
    public int countForJumpscare = 4;

    //Jumpscare
    public bool jumpScare;
    private bool finish;
    private bool flickering;
    private Vector3 humanPosition1 = new Vector3(0.340000004f, -0.113922141f, 5.99638224f);
    private Vector3 humanPosition2 = new Vector3(0.349999994f, -0.0900000036f, 0.629999995f);
    public GameObject patient; // Prefab to instantiate when flickering starts

    //Blurry text
    public BlurryText blurryText;  // Reference to the SpriteFade script

    void Start()
    {        
        // Find blurryText in children
        blurryText = GetComponentInChildren<BlurryText>();
        finish = false;
        jumpScare = false;
        flickering = false;
    }

    public override void OnLookedAt()
    {
        if (flickering || finish) return; // Prevent duplicate calls
        patient.SetActive(false);
        Debug.Log("Journal Eye detector looked at");
        blurryText.PauseFadeOut(true);
        Debug.Log("Eyes detected, starting to flicker");
        StartCoroutine(FlickerLight());     // Start the flickering coroutine
    }

private IEnumerator FlickerLight()
    {
        flickering = true;
        patient.transform.localPosition = humanPosition1;

        Debug.Log("Flickercount: " + flickerCount);
            if (patient != null && flickerCount != countForJumpscare)
            {            
                lightManager.StartFlicker(5, 1, false);
                yield return null;
                while (lightManager.flickeringOn == false)
                {
                if (lightManager.lightsOn == false)
                        patient.SetActive(Random.Range(0, 2) == 0);
                    else
                        patient.SetActive(false);
                }
            }
            else if (flickerCount == countForJumpscare)
            {
                lightManager.StartFlicker(15f, 0.7f, false);
                StartCoroutine(JumpScare());
                yield return null;
                while (lightManager.flickeringOn == false)
                        patient.SetActive(Random.Range(0, 2) == 0);
            }

        // Increment flickerCount after completing the loop
        flickerCount++;
        flickering = false;
    }

    public void StartPatientGone()
    {
        StartCoroutine(patientGone());
        Debug.Log("Preparing Patient Gone");
    }
    public void StartJumpScare()
    {
        StartCoroutine(JumpScare());
        Debug.Log("Preparing Jump Scare");
    }

    public IEnumerator JumpScare()
    {
        Debug.Log("Running jumpscare");
        jumpScare = true;
        Debug.Log("Jumpscare running, waiting 2 seconds");
        yield return new WaitForSeconds(2);
        Debug.Log("Jumpscare done, patient leaving");
        
        StartCoroutine(patientGone());
        //Destroy(patient);
    }

    private IEnumerator patientGone()
    {
        //Flicker light and pateint gone
        if (flickerCount == countForJumpscare) finish = true;
        lightManager.StartFlicker(4, 1, false);

        yield return new WaitForSeconds(2);
        while (lightManager.flickeringOn == false)
        {
            if (lightManager.lightsOn == false) {
                patient.SetActive(Random.Range(0, 2) == 0); yield return new WaitForSeconds(0.1f);}
            else
                patient.SetActive(false);                
            patient.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (jumpScare && patient != null)
        {
            float speed = 10f; // Units per second
            patient.transform.localPosition = Vector3.MoveTowards(patient.transform.localPosition, humanPosition2, speed * Time.deltaTime);
        }
    }
    private void Update()
    {
        foreach (var light in staticLights) light.enabled = true; //keep these lights on
    }
}
