using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayEnabler : MonoBehaviour
{
    [SerializeField] private GameObject[] hallways; // Reference to an array of hallway GameObjects.
    [SerializeField] private GameObject[] hallwayEnablers; // Reference to an array of hallway GameObjects.
    [SerializeField] private int hallwayToEnable = 1; // Number of hallways to enable.
    [SerializeField] private LightManager lightManager;

    private void Start()
    {
        for (int i = 0; i < hallways.Length; i++)
        {
            if (i != 0) // Check if the current index is not 0
            {
                hallways[i].SetActive(false); // Deactivate all hallways except the first one
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the tag "HallwayEnabler".
        if (other.CompareTag("HallwayEnabler"))
        {
            lightManager.allLights = FindObjectsOfType<Light>();

            // Ensure the array is not null and has at least one element.
            if (hallways != null && hallways.Length >= hallwayToEnable)
            {

                if (hallwayToEnable <= hallways.Length)
                {
                    if (hallways[hallwayToEnable] != null)
                    {
                        hallways[hallwayToEnable + 1].SetActive(true);
                    }
                }

                if (hallwayToEnable > 0)
                {
                    hallways[hallwayToEnable - 1].SetActive(false);
                }

                Destroy(hallwayEnablers[hallwayToEnable]);

                hallwayToEnable++;
            }
            else
            {
                Debug.LogWarning("Hallways array is empty or not set.");
            }
        }
    }
}
