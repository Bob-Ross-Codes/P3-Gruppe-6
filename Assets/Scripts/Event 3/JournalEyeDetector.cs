using System.Collections;
using Mediapipe.Unity;
using UnityEngine;

public class JournalEyeDetector : GazeActivation
{
    //Eyetracking elements
    public override float ActivationTime => 5f;
    public GameObject EyeDetector;

    //Flickering lights
    public Light[] lightsToFlicker;  // Array of lights to flicker
    public float flickerDuration = 4f;  // Total duration of flickering effect (2 seconds)
    public float darkInterval;  // Interval when lights are off
    public float lightInterval;  // Interval when lights are on
    public int flickerCount = 1;
    public bool flickering;
    private bool finish;
    
    //Jumpscare
    public bool jumpScare;
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
    }

    public override void OnLookedAt()
    {
        Debug.Log("Journal Eye detector looked at");
        blurryText.PauseFadeOut(true);
        //Only run if no flickering
        if (!flickering && !finish)
        {
            Debug.Log("Eyes detected, starting to flicker");
            StartCoroutine(FlickerLight());     // Start the flickering coroutine
        }
    }

    private IEnumerator FlickerLight()
    {
        flickering = true;                      //Keep track of flickering
        Debug.Log("Running flickerCount " + flickerCount);

        patient.SetActive(true);
        patient.transform.localPosition = humanPosition1;

        if (patient != null && flickerCount != 3)
        {
            flickerDuration = 4;
        }
        else if (patient != null && flickerCount == 3)
        {
            flickerDuration = 15;
        }else
        {
            Debug.LogError("Patient is not assigned!");
        }

        float elapsedTime = 0f;  // Track the total time of flickering

        // Loop for the flicker effect until the duration is reached
        while (elapsedTime < flickerDuration && flickering)
        {
            darkInterval = Random.Range(0.1f, 0.4f);
            lightInterval = Random.Range(0.05f, 0.1f);

            // Turn off all lights (darker state)
            foreach (var light in lightsToFlicker)
            {
                light.enabled = false;
            }

            // Wait for the dark interval
            yield return new WaitForSeconds(darkInterval);
            elapsedTime += darkInterval;

            // Turn on all lights (brief light state)
            foreach (var light in lightsToFlicker)
            {
                light.enabled = true;
                AkSoundEngine.PostEvent("Flickering_Lights", gameObject);
            }

            // Wait for the light interval
            yield return new WaitForSeconds(lightInterval);
            elapsedTime += lightInterval;
        }
        
        StartCoroutine(patientGone());

        // Increment flickerCount after completing the loop
        flickerCount++;
        Debug.Log("FlickerCount " + (flickerCount-1) + " is done,next up is " + flickerCount);

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
        Destroy(patient);
        flickerCount++;

    }

    private IEnumerator patientGone()
    {
            // Ensure all lights are on when the flickering stops (optional)
            foreach (var light in lightsToFlicker)
            {
                light.enabled = false;
            }

            yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds

            // Now, deactivate the flicker effect prefab after the last darkInterval
            patient.SetActive(false);

            yield return new WaitForSeconds(0.2f);  // Wait for 0.2 seconds

            // Turn off all lights after the wait
            foreach (var light in lightsToFlicker)
            {
                light.enabled = true;
            }

            if (flickerCount == 3) finish = true; 
    }

    void FixedUpdate()
    {
        if (jumpScare && patient != null)
        {
            float speed = 10f; // Units per second
            patient.transform.localPosition = Vector3.MoveTowards(patient.transform.localPosition, humanPosition2, speed * Time.deltaTime);
        }
    }
}
