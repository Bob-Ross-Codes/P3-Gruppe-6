/// <summary>
/// Handles door interaction and patient disappearance based on player proximity.
/// When the player is near the door, this script triggers light flickering and eventually makes the patient disappear.
/// It also activates eye-tracking after the sequence finishes.
/// </summary>
using UnityEngine;

public class Door1PatientPeak : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the player's transform.")]
    [SerializeField] private Transform player;

    [Tooltip("Reference to the door's transform.")]
    [SerializeField] private Transform door;

    [Tooltip("The patient GameObject that will be made visible and then disappear.")]
    [SerializeField] private GameObject patient;

    [Tooltip("Reference to the LightManager script controlling the flickering lights.")]
    [SerializeField] private LightManager lightManager;

    [Tooltip("Reference to the JournalEyeDetector script for activating eye-tracking.")]
    [SerializeField] private JournalEyeDetector journalEyeDetector;

    [Header("Patient Position Settings")]
    [Tooltip("The patient's initial position relative to its parent.")]
    private Vector3 initialPosition = new Vector3(0.19f, -0.09f, 0.53f);

    [Tooltip("A new position for the patient (currently unused).")]
    private Vector3 newPosition = new Vector3(0.31f, -1.14f, -9.24f); // Not used in this script

    [Header("Interaction Settings")]
    [Tooltip("Distance from the door at which the interaction is triggered.")]
    [SerializeField] private float interactionRange = 3f;

    private bool triggered = false; // Has the interaction sequence started?
    private bool finish = false;    // Is the flicker sequence finished and ready for the next step?

    /// <summary>
    /// Sets the patient active and positions it at the initial location when the game starts.
    /// </summary>
    private void Start()
    {
        patient.SetActive(true);
        patient.transform.localPosition = initialPosition;
    }

    /// <summary>
    /// Checks the distance between the player and the door every fixed frame.
    /// Triggers light flickering when the player is close enough and handles the sequence after flickering stops.
    /// </summary>
    private void FixedUpdate()
    {
        float distanceToPlayer = Vector3.Distance(player.position, door.position);

        // Trigger interaction if the player is close and not yet triggered
        if (distanceToPlayer < interactionRange && !triggered)
        {
            // Start the initial flicker sequence
            lightManager.StartFlicker(3.5f, 0.9f, true);
            triggered = true;
            finish = true;
        }

        // After the flicker stops, run the next flicker and make the patient disappear
        if (finish && !lightManager.flickeringOn)
        {
            lightManager.StartFlicker(1f, 1.3f, true);

            // When lights are off, hide the patient
            if (!lightManager.lightsOn)
            {
                patient.SetActive(false);
            }

            finish = false;

            // Activate eye-tracking after completing the sequence
            journalEyeDetector.ActivateEyetracking();
        }
    }
}