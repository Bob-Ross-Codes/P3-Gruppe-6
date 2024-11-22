using System.Collections;
using UnityEditor.EventSystems;
using UnityEngine;

public class Door1PatientPeak : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform door;
    [SerializeField] private GameObject patient;
    [SerializeField] private LightManager lightManager;
    [SerializeField] private JournalEyeDetector journalEyeDetector;

    private Vector3 initialPosition = new Vector3(0.189999998f, -0.0900000036f, 0.529999971f);
    private Vector3 newPosition = new Vector3(0.313501358f, -1.13949442f, -9.2357254f);

    private float distanceToPlayer;
    private float interactionRange = 3f;                        // Set the interaction range
    private bool triggered;
    private bool finish;

    void Start()
    {
        triggered = false;
        patient.SetActive(true);
        finish = false;
        patient.transform.localPosition = initialPosition;
    }

    private void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(player.position, door.position);   //If close to the door

        if (distanceToPlayer < interactionRange && !triggered)
        {
            lightManager.StartFlicker(3.5f, 0.9f, true);
            triggered = true;
            finish = true;
        }
        if (finish && lightManager.flickeringOn == false)
        {
            lightManager.StartFlicker(1f, 1.3f, true);
            if (lightManager.lightsOn == false)
            patient.SetActive(false);                               //Patient dissapears
            finish = false;
            journalEyeDetector.ActivateEyetracking();
        }
    }
}
/* FirstPersonController at hospital
 * Vector3(-1.70000005,3.56999993,0.899999976)
 * Quaternion(0,0.707106829,0,0.707106829)
 * 
 * FirstPersonController at Mainsion
 * Vector3(-148.600006,3.56999993,83.8000031)
 * Quaternion(0,0.707106829,0,-0.707106829)
 */