using System.Collections;
using UnityEditor.EventSystems;
using UnityEngine;

public class Door1PatientPeak : MonoBehaviour
{

    public Light[] lightsToFlicker;                             // Array of lights to flicker

    [SerializeField] private Transform player;
    [SerializeField] private Transform door;
    [SerializeField] private GameObject patient;

    private Vector3 initialPosition = new Vector3(0.189999998f, -0.0900000036f, 0.529999971f);
    private Vector3 newPosition = new Vector3(0.313501358f, -1.13949442f, -9.2357254f);

    private float distanceToPlayer;
    private float interactionRange = 3f;                        // Set the interaction range
    private bool triggered;

    void Start()
    {
        triggered = false;
        patient.SetActive(true);
        patient.transform.localPosition = initialPosition;
        Debug.Log("Patient at: " + patient.transform.position);
        Debug.Log("Target position: " + initialPosition);

        foreach (var light in lightsToFlicker)
        {
            light.enabled = true;                                                   //Turn on lights
        }
    }

    private void FixedUpdate()
    {
        distanceToPlayer = Vector3.Distance(player.position, door.position);   //If close to the door

        if (distanceToPlayer < interactionRange && !triggered)
        {
            Debug.Log("Patient dissapearing");
            foreach (var light in lightsToFlicker)                                  //Turn off lights
            {
                light.enabled = false;
                AkSoundEngine.PostEvent("Flickering_Lights", gameObject);
            }
            StartCoroutine(WaitAndLight(3f));                                       //Start waiter
            triggered = true;
        }
    }


    private IEnumerator WaitAndLight(float time)
    {
        yield return new WaitForSeconds(time);                  //Wait
        patient.SetActive(false);                               //Patient dissapears
        patient.transform.localPosition = newPosition;
        foreach (var light in lightsToFlicker)
        {   if(light != null)
            light.enabled = true;                               //Turn on lights
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