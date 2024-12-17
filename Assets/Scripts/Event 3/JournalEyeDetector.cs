using System.Collections;
using Mediapipe.Unity;
using UnityEngine;

/// <summary>
/// Manages the eye-tracking interactions for a journal-related event.
/// Handles light flickering, patient movement, and triggering jumpscares based on player's gaze and input.
/// </summary>
public class JournalEyeDetector : GazeActivation
{
    // This property is not a serialized field and will not display in the Inspector.
    public override float ActivationTime => 3f;

    [Header("Eye-Tracking Settings")]
    [Tooltip("Reference to the EyeDetector GameObject.")]
    public GameObject EyeDetector;

    [Header("Light Flickering Settings")]
    [Tooltip("Reference to the LightManager script.")]
    public LightManager lightManager;

    [Tooltip("Static lights in the scene.")]
    [SerializeField] private Light[] staticLights;

    [Header("Patient and Text Settings")]
    [Tooltip("The patient GameObject.")]
    public GameObject patient;

    [Tooltip("Reference to the BlurryText script.")]
    public BlurryText blurryText;

    [Header("Positions")]
    [Tooltip("First position for the patient.")]
    private Vector3 humanPosition1 = new Vector3(0.189999998f, 1.429f, 6.21000004f);

    [Tooltip("Second position for the patient.")]
    private Vector3 humanPosition2 = new Vector3(0.349999994f, 1.2f, 0.629999995f);

    [Header("Event Settings")]
    [Tooltip("Number of times the player can look at the journal before triggering a jumpscare.")]
    public int jumpScareCount = 4;

    public int lookAtCount = 0; // Tracks how many times the player has looked at the journal
    private bool eyetrackingActivated = false; // Tracks if eye-tracking is enabled
    public bool jumpScare = false; // Tracks if the jumpscare has been triggered
    private bool jumpScareTriggered = false; // Ensures the jumpscare sound plays only once
    private int rCount = 0; // Tracks how many times the R key has been pressed

    /// <summary>
    /// Initializes script variables and finds necessary components.
    /// </summary>
    void Start()
    {
        blurryText = GetComponentInChildren<BlurryText>();
    }

    /// <summary>
    /// Activates eye-tracking and moves the patient to the first position.
    /// </summary>
    public void ActivateEyetracking()
    {
        eyetrackingActivated = true;
        patient.transform.position = humanPosition1;
        Debug.Log("Eye-tracking activated. Patient moved to position 1.");
    }

    /// <summary>
    /// Called when the player looks at the journal for the required duration.
    /// Handles light flickering, patient visibility, and jumpscare activation.
    /// </summary>
    public override void OnLookedAt()
    {
        Debug.Log("Player looked at EyeDetector.");

        if (!lightManager.flickeringOn)
        {
            blurryText.PauseFadeOut(true);

            Debug.Log("Processing lookAtCount.");
            if (lookAtCount < jumpScareCount)
            {
                lightManager.StartFlicker(4f, 1f, true);
                Debug.Log($"lookAtCount: {lookAtCount}");
                if (patient != null)
                    patient.transform.localPosition = humanPosition1;
            }
            else if (lookAtCount > jumpScareCount)
            {
                Destroy(patient);
            }
            else
            {
                lightManager.StartFlicker(15f, 1f, true);
                Debug.Log("JumpScare triggered!");
            }

            lookAtCount++;
        }
    }

    /// <summary>
    /// Updates the game state, including handling key inputs and jumpscare logic.
    /// </summary>
    private void Update()
    {
        if (eyetrackingActivated)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                rCount++;
                lookAtCount = -1;
            }

            if (rCount > 1 && !jumpScare)
            {
                StartCoroutine(DestroyPatient());
                jumpScare = true;
            }
        }
    }

    /// <summary>
    /// Coroutine to delay the destruction of the patient GameObject.
    /// </summary>
    private IEnumerator DestroyPatient()
    {
        yield return new WaitForSeconds(1f);
        Destroy(patient);
    }

    /// <summary>
    /// Handles patient visibility and movement during light flickering and jumpscares.
    /// </summary>
    private void FixedUpdate()
    {
        if (eyetrackingActivated)
        {
            int randomNumber = Random.Range(0, 3);

            if (lightManager.flickeringOn && !jumpScare)
            {
                Debug.Log($"lightsOn: {lightManager.lightsOn}, randomNumber: {randomNumber}");
                patient.SetActive(lightManager.lightsOn || randomNumber == 0);
            }
            else if (!lightManager.flickeringOn && patient != null)
            {
                patient.SetActive(false);
            }
        }

        if (jumpScare && patient != null)
        {
            patient.SetActive(true);
            float speed = 30f;

            if (!jumpScareTriggered)
            {
                AkSoundEngine.PostEvent("Play_Cell_Jumpscare", patient);
                jumpScareTriggered = true;
            }

            patient.transform.localPosition = Vector3.MoveTowards(patient.transform.localPosition, humanPosition2, speed * Time.deltaTime);
        }
    }
}