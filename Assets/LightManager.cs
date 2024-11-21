using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private Light[] allLights;  // Array of lights
    private Light[] lightsToFlicker;  // Time elapsed since flickering started
    [SerializeField] private Light handlight;  // Time elapsed since flickering started

    private float flickerDuration;  // Total duration of flickering effect (2 seconds)
    private float darkInterval;  // Interval when lights are off
    private float lightInterval;  // Interval when lights are on
    public bool flickeringOn;
    public bool lightsOn = false;

    public void StartFlicker(float duration, float speed, bool handLight)
    {
        if (handlight)
    {
        lightsToFlicker = new List<Light>(allLights).ToArray();  // Convert to array
        List<Light> tempList = new List<Light>(lightsToFlicker); // Optionally use List later if needed
        tempList.Add(handlight); // Add handlight to temp list
        lightsToFlicker = tempList.ToArray(); // Convert back to array
    }
    else
        lightsToFlicker = new List<Light>(allLights).ToArray();  // Convert to array

        StartCoroutine(Flickering(duration, speed, handLight));
        flickeringOn = true;
    }
    public void StopFlicker()
    {
        flickeringOn = false;
    }

    public IEnumerator Flickering(float duration, float speed, bool handLight)
    {
        float elapsedTime = 0;
        if (flickeringOn)
        {
            flickerDuration = duration;

            while (elapsedTime < flickerDuration && flickeringOn)
            {
                darkInterval = Random.Range(0.1f, 0.4f);
                lightInterval = Random.Range(0.05f, 0.1f);

                // Turn off all lights (darker state)
                foreach (var light in lightsToFlicker)
                light.enabled = false;
                lightsOn = false;
                
                yield return new WaitForSeconds(darkInterval * speed);
                elapsedTime += darkInterval;

                // Turn on all lights (brief light state)
                foreach (var light in lightsToFlicker)
                {
                    light.enabled = true;
                    AkSoundEngine.PostEvent("Flickering_Lights", gameObject);
                }
                lightsOn = true;

                yield return new WaitForSeconds(lightInterval * speed);
                elapsedTime += lightInterval;
            }
            foreach (var light in lightsToFlicker){
                    light.enabled = true; lightsOn = true;}
                
            flickeringOn = false;
        }
        else {flickeringOn = false; yield break;}
    }
}
